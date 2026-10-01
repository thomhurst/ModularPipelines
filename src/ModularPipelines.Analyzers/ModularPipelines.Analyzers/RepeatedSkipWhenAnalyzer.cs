using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace ModularPipelines.Analyzers;

/// <summary>
/// Reports diagnostic MP0020 when one member body calls <c>WithSkipWhen</c> more than once on the same
/// <c>ModuleConfigurationBuilder</c>. Repeated skip conditions are OR-ed, which differs from V3, where a
/// later call replaced an earlier one. Calls in mutually exclusive branches (<c>if</c>/<c>else</c>, switch
/// sections or arms, conditional expressions) are not counted against each other, because only one of them runs.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public sealed class RepeatedSkipWhenAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MP0020";

    private const string BuilderMetadataName = "ModularPipelines.ModuleConfigurationBuilder";
    private const string WithSkipWhenMethodName = "WithSkipWhen";
    private const int MaxAliasDepth = 8;

    public static DiagnosticDescriptor Rule { get; } =
        DiagnosticDescriptorFactory.Create(
            DiagnosticId,
            nameof(Resources.RepeatedSkipWhenAnalyzerTitle),
            nameof(Resources.RepeatedSkipWhenAnalyzerMessageFormat),
            nameof(Resources.RepeatedSkipWhenAnalyzerDescription),
            severity: DiagnosticSeverity.Info);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterCompilationStartAction(compilationContext =>
        {
            var builderType = compilationContext.Compilation.GetTypeByMetadataName(BuilderMetadataName);
            if (builderType is null)
            {
                return;
            }

            compilationContext.RegisterOperationBlockStartAction(blockContext =>
            {
                var callsByBuilder = new ConcurrentDictionary<object, ConcurrentBag<IInvocationOperation>>();

                blockContext.RegisterOperationAction(
                    operationContext =>
                    {
                        var invocation = (IInvocationOperation) operationContext.Operation;
                        if (invocation.TargetMethod.Name != WithSkipWhenMethodName
                            || !SymbolEqualityComparer.Default.Equals(invocation.TargetMethod.ContainingType, builderType)
                            || invocation.Instance is null)
                        {
                            return;
                        }

                        var builderKey = GetBuilderKey(invocation.Instance, builderType);
                        callsByBuilder.GetOrAdd(builderKey, _ => []).Add(invocation);
                    },
                    OperationKind.Invocation);

                blockContext.RegisterOperationBlockEndAction(endContext =>
                {
                    foreach (var calls in callsByBuilder.Values)
                    {
                        if (calls.Count < 2)
                        {
                            continue;
                        }

                        var ordered = calls.OrderBy(call => call.Syntax.SpanStart).ToList();
                        for (var i = 1; i < ordered.Count; i++)
                        {
                            var repeatedCall = ordered[i];
                            if (ordered.Take(i).Any(earlier => !AreMutuallyExclusive(earlier.Syntax, repeatedCall.Syntax)))
                            {
                                endContext.ReportDiagnostic(Diagnostic.Create(Rule, GetMethodNameLocation(repeatedCall)));
                            }
                        }
                    }
                });
            });
        });
    }

    /// <summary>
    /// Follows a fluent chain of builder calls back to the builder it started from, so calls on the same
    /// parameter, local, field, or chain share one key. Locals that are initialized from a builder and never
    /// reassigned are followed to their initializer, so an alias shares the key of the builder it aliases.
    /// </summary>
    private static object GetBuilderKey(IOperation instance, INamedTypeSymbol builderType)
    {
        var current = instance;
        var aliasDepth = 0;
        while (true)
        {
            current = Unwrap(current);
            if (current is IInvocationOperation { Instance: not null } chainedCall
                && SymbolEqualityComparer.Default.Equals(chainedCall.TargetMethod.ContainingType, builderType)
                && SymbolEqualityComparer.Default.Equals(chainedCall.Type, builderType))
            {
                current = chainedCall.Instance;
                continue;
            }

            if (current is ILocalReferenceOperation localReference
                && aliasDepth < MaxAliasDepth
                && TryGetAliasedBuilder(localReference, builderType) is { } aliased)
            {
                aliasDepth++;
                current = aliased;
                continue;
            }

            break;
        }

        return current switch
        {
            IParameterReferenceOperation parameter => parameter.Parameter,
            ILocalReferenceOperation local => local.Local,
            IFieldReferenceOperation { Instance: null or IInstanceReferenceOperation } field => field.Field,
            IPropertyReferenceOperation { Instance: null or IInstanceReferenceOperation } property => property.Property,
            _ => current,
        };
    }

    private static IOperation? TryGetAliasedBuilder(ILocalReferenceOperation localReference, INamedTypeSymbol builderType)
    {
        var local = localReference.Local;
        if (!SymbolEqualityComparer.Default.Equals(local.Type, builderType)
            || local.DeclaringSyntaxReferences.Length != 1
            || local.DeclaringSyntaxReferences[0].GetSyntax() is not VariableDeclaratorSyntax { Initializer.Value: { } initializer } declarator
            || localReference.SemanticModel is not { } semanticModel
            || initializer.SyntaxTree != semanticModel.SyntaxTree)
        {
            return null;
        }

        var scope = declarator.FirstAncestorOrSelf<BlockSyntax>();
        if (scope is null || IsReassigned(scope, local, semanticModel))
        {
            return null;
        }

        return semanticModel.GetOperation(initializer);
    }

    private static bool IsReassigned(SyntaxNode scope, ILocalSymbol local, SemanticModel semanticModel)
    {
        foreach (var node in scope.DescendantNodes())
        {
            var target = node switch
            {
                AssignmentExpressionSyntax assignment => assignment.Left,
                ArgumentSyntax { RefKindKeyword.RawKind: not 0 } argument => argument.Expression,
                _ => null,
            };

            if (target is IdentifierNameSyntax identifier
                && identifier.Identifier.ValueText == local.Name
                && SymbolEqualityComparer.Default.Equals(semanticModel.GetSymbolInfo(identifier).Symbol, local))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns whether two calls sit in different branches of the same <c>if</c>/<c>else</c>, switch statement,
    /// switch expression, or conditional expression, so at most one of them runs.
    /// </summary>
    private static bool AreMutuallyExclusive(SyntaxNode first, SyntaxNode second)
    {
        var firstAncestors = new HashSet<SyntaxNode>(first.AncestorsAndSelf());
        var common = second.AncestorsAndSelf().FirstOrDefault(firstAncestors.Contains);

        return common switch
        {
            IfStatementSyntax ifStatement => ifStatement.Else is { } elseClause
                && IsWithinBranches(first, second, ifStatement.Statement, elseClause),
            ConditionalExpressionSyntax conditional =>
                IsWithinBranches(first, second, conditional.WhenTrue, conditional.WhenFalse),
            SwitchStatementSyntax switchStatement =>
                IsWithinDifferentBranches(first, second, switchStatement.Sections),
            SwitchExpressionSyntax switchExpression =>
                IsWithinDifferentBranches(first, second, switchExpression.Arms),
            _ => false,
        };
    }

    private static bool IsWithinBranches(SyntaxNode first, SyntaxNode second, SyntaxNode whenTrue, SyntaxNode whenFalse) =>
        (whenTrue.Span.Contains(first.Span) && whenFalse.Span.Contains(second.Span))
        || (whenFalse.Span.Contains(first.Span) && whenTrue.Span.Contains(second.Span));

    private static bool IsWithinDifferentBranches(SyntaxNode first, SyntaxNode second, IEnumerable<SyntaxNode> branches)
    {
        var firstBranch = branches.FirstOrDefault(branch => branch.Span.Contains(first.Span));
        var secondBranch = branches.FirstOrDefault(branch => branch.Span.Contains(second.Span));
        return firstBranch is not null && secondBranch is not null && firstBranch != secondBranch;
    }

    private static IOperation Unwrap(IOperation operation)
    {
        while (operation is IConversionOperation { IsImplicit: true } or IParenthesizedOperation)
        {
            operation = operation switch
            {
                IConversionOperation conversion => conversion.Operand,
                IParenthesizedOperation parenthesized => parenthesized.Operand,
                _ => operation,
            };
        }

        return operation;
    }

    private static Location GetMethodNameLocation(IInvocationOperation invocation)
    {
        var syntax = invocation.Syntax;
        if (syntax is InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax memberAccess })
        {
            return memberAccess.Name.GetLocation();
        }

        return syntax.GetLocation();
    }
}
