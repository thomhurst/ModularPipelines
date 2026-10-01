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
                var reassignedLocals = new ConcurrentDictionary<ILocalSymbol, bool>(SymbolEqualityComparer.Default);

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

                        // A null key means the builder cannot be identified statically, so the call is not grouped.
                        if (GetBuilderKey(invocation.Instance, builderType, reassignedLocals) is { } builderKey)
                        {
                            callsByBuilder.GetOrAdd(builderKey, _ => []).Add(invocation);
                        }
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
    /// Returns <see langword="null"/> for a local that is written after its declaration, because which builder
    /// such a local holds at each call depends on control flow.
    /// </summary>
    private static object? GetBuilderKey(
        IOperation instance,
        INamedTypeSymbol builderType,
        ConcurrentDictionary<ILocalSymbol, bool> reassignedLocals)
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

            if (current is ILocalReferenceOperation localReference)
            {
                if (IsReassigned(localReference, reassignedLocals))
                {
                    return null;
                }

                if (aliasDepth < MaxAliasDepth && TryGetAliasedBuilder(localReference, builderType) is { } aliased)
                {
                    aliasDepth++;
                    current = aliased;
                    continue;
                }
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
            || local.DeclaringSyntaxReferences[0].GetSyntax() is not VariableDeclaratorSyntax { Initializer.Value: { } initializer }
            || localReference.SemanticModel is not { } semanticModel
            || initializer.SyntaxTree != semanticModel.SyntaxTree)
        {
            return null;
        }

        return semanticModel.GetOperation(initializer);
    }

    private static bool IsReassigned(
        ILocalReferenceOperation localReference,
        ConcurrentDictionary<ILocalSymbol, bool> reassignedLocals) =>
        reassignedLocals.GetOrAdd(localReference.Local, local => IsWrittenAfterDeclaration(local, localReference.SemanticModel));

    /// <summary>
    /// Uses data-flow analysis to find any write to <paramref name="local"/> other than its declaration, including
    /// assignments, deconstruction, <c>ref</c>/<c>out</c> arguments, and writes inside lambdas or local functions.
    /// Locals whose declaration cannot be analyzed are treated as reassigned.
    /// </summary>
    private static bool IsWrittenAfterDeclaration(ILocalSymbol local, SemanticModel? semanticModel)
    {
        if (semanticModel is null
            || local.DeclaringSyntaxReferences.Length != 1
            || local.DeclaringSyntaxReferences[0].GetSyntax() is not { } declaration
            || declaration.SyntaxTree != semanticModel.SyntaxTree
            || declaration.FirstAncestorOrSelf<StatementSyntax>() is not { } declaringStatement)
        {
            return true;
        }

        foreach (var region in GetRegionsAfterDeclaration(declaringStatement))
        {
            var dataFlow = region is StatementSyntax statement
                ? semanticModel.AnalyzeDataFlow(statement)
                : semanticModel.AnalyzeDataFlow((ExpressionSyntax) region);

            if (dataFlow is null || !dataFlow.Succeeded)
            {
                return true;
            }

            if (dataFlow.WrittenInside.Contains(local, SymbolEqualityComparer.Default))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns the statements and expressions within the local's scope that run after its declaration: the
    /// statements following the declaring statement, statements nested inside it, and a <c>for</c> loop's condition
    /// and incrementors. None of these regions contain the declaration's own write.
    /// </summary>
    private static IEnumerable<SyntaxNode> GetRegionsAfterDeclaration(StatementSyntax declaringStatement)
    {
        var siblings = declaringStatement.Parent switch
        {
            BlockSyntax block => block.Statements,
            SwitchSectionSyntax section => section.Statements,
            _ => default(SyntaxList<StatementSyntax>?),
        };

        if (siblings is { } statements)
        {
            foreach (var statement in statements.Skip(statements.IndexOf(declaringStatement) + 1))
            {
                yield return statement;
            }
        }
        else if (declaringStatement.Parent is GlobalStatementSyntax { Parent: CompilationUnitSyntax compilationUnit } global)
        {
            foreach (var member in compilationUnit.Members.Skip(compilationUnit.Members.IndexOf(global) + 1))
            {
                if (member is GlobalStatementSyntax following)
                {
                    yield return following.Statement;
                }
            }
        }

        var nestedStatements = declaringStatement
            .DescendantNodes(node => node == declaringStatement || node is not StatementSyntax)
            .OfType<StatementSyntax>();
        foreach (var nested in nestedStatements)
        {
            yield return nested;
        }

        if (declaringStatement is ForStatementSyntax forStatement)
        {
            if (forStatement.Condition is { } condition)
            {
                yield return condition;
            }

            foreach (var incrementor in forStatement.Incrementors)
            {
                yield return incrementor;
            }
        }
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
                !ContainsGoto(switchStatement)
                && IsWithinDifferentBranches(first, second, switchStatement.Sections),
            SwitchExpressionSyntax switchExpression =>
                IsWithinDifferentBranches(first, second, switchExpression.Arms),
            _ => false,
        };
    }

    /// <summary>
    /// A <c>goto case</c>, <c>goto default</c>, or labeled <c>goto</c> can transfer control from one switch section
    /// into another, so sections of a switch that contains any <c>goto</c> are not treated as mutually exclusive.
    /// </summary>
    private static bool ContainsGoto(SwitchStatementSyntax switchStatement) =>
        switchStatement.DescendantNodes().OfType<GotoStatementSyntax>().Any();

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
