using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
                    foreach (var entry in callsByBuilder)
                    {
                        var calls = entry.Value;
                        if (calls.Count < 2)
                        {
                            continue;
                        }

                        var ordered = calls.OrderBy(call => call.Syntax.SpanStart).ToList();
                        for (var i = 1; i < ordered.Count; i++)
                        {
                            var repeatedCall = ordered[i];
                            if (ordered.Take(i).Any(earlier => CanRunOnSameBuilder(entry.Key, earlier, repeatedCall)))
                            {
                                endContext.ReportDiagnostic(Diagnostic.Create(Rule, GetMethodNameLocation(repeatedCall)));
                            }
                        }
                    }
                });
            });
        });
    }

    private static bool CanRunOnSameBuilder(object builderKey, IInvocationOperation earlier, IInvocationOperation later)
    {
        if (earlier.SemanticModel is not { } semanticModel
            || AreMutuallyExclusive(earlier.Syntax, later.Syntax, semanticModel))
        {
            return false;
        }

        return builderKey is not ReassignedLocalKey reassigned
            || !IsWrittenBetween(reassigned.Local, earlier.Syntax, later.Syntax, semanticModel);
    }

    /// <summary>
    /// Follows a fluent chain of builder calls back to the builder it started from, so calls on the same
    /// parameter, local, field, or chain share one key. Locals that are initialized from a builder and never
    /// reassigned are followed to their initializer, so an alias shares the key of the builder it aliases.
    /// A local that is written after its declaration gets a <see cref="ReassignedLocalKey"/>, so its calls are
    /// compared only when no write to the local can happen between them. Returns <see langword="null"/> when such a
    /// local is reached through an alias, because the alias captured whichever builder the local held at that point.
    /// </summary>
    private static object? GetBuilderKey(
        IOperation instance,
        INamedTypeSymbol builderType,
        ConcurrentDictionary<ILocalSymbol, bool> reassignedLocals)
    {
        var current = UnwrapChain(instance, builderType);
        for (var aliasDepth = 0; current is ILocalReferenceOperation localReference; aliasDepth++)
        {
            if (IsReassigned(localReference, reassignedLocals))
            {
                return aliasDepth == 0 ? new ReassignedLocalKey(localReference.Local) : null;
            }

            if (aliasDepth >= MaxAliasDepth || TryGetAliasedBuilder(localReference, builderType) is not { } aliased)
            {
                break;
            }

            current = UnwrapChain(aliased, builderType);
        }

        return ToKey(current);
    }

    private static IOperation UnwrapChain(IOperation operation, INamedTypeSymbol builderType)
    {
        var current = Unwrap(operation);
        while (current is IInvocationOperation { Instance: { } chainedInstance } chainedCall
            && SymbolEqualityComparer.Default.Equals(chainedCall.TargetMethod.ContainingType, builderType)
            && SymbolEqualityComparer.Default.Equals(chainedCall.Type, builderType))
        {
            current = Unwrap(chainedInstance);
        }

        return current;
    }

    private static object ToKey(IOperation current) =>
        current switch
        {
            IParameterReferenceOperation parameter => parameter.Parameter,
            ILocalReferenceOperation local => local.Local,
            IFieldReferenceOperation { Instance: null or IInstanceReferenceOperation } field => field.Field,
            IPropertyReferenceOperation { Instance: null or IInstanceReferenceOperation } property => property.Property,
            _ => current,
        };

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
    /// Returns whether <paramref name="local"/> may be written between two calls. The analyzed region spans the
    /// statements, within the innermost statement list containing both calls, from the one holding the first call to
    /// the one holding the second. Writes in that range but off the path between the calls also count, which keeps
    /// the check conservative. Calls that cannot be placed in one statement list are treated as separated by a write.
    /// </summary>
    private static bool IsWrittenBetween(ILocalSymbol local, SyntaxNode first, SyntaxNode second, SemanticModel semanticModel)
    {
        var firstAncestors = new HashSet<SyntaxNode>(first.AncestorsAndSelf());
        var common = second.AncestorsAndSelf().First(firstAncestors.Contains);

        var statements = common switch
        {
            BlockSyntax block => block.Statements,
            SwitchSectionSyntax section => section.Statements,
            _ => default(SyntaxList<StatementSyntax>?),
        };

        StatementSyntax? firstStatement;
        StatementSyntax? lastStatement;
        if (statements is { } list)
        {
            firstStatement = list.FirstOrDefault(statement => statement.Span.Contains(first.Span));
            lastStatement = list.FirstOrDefault(statement => statement.Span.Contains(second.Span));
        }
        else
        {
            firstStatement = lastStatement = common.FirstAncestorOrSelf<StatementSyntax>();
        }

        if (firstStatement is null || lastStatement is null)
        {
            return true;
        }

        var dataFlow = semanticModel.AnalyzeDataFlow(firstStatement, lastStatement);
        return dataFlow is null
            || !dataFlow.Succeeded
            || dataFlow.WrittenInside.Contains(local, SymbolEqualityComparer.Default);
    }

    /// <summary>
    /// Returns the statements and expressions within the local's scope that run after its declaration: the
    /// statements following the declaring statement, statements nested inside it, and a <c>for</c> loop's condition
    /// and incrementors. None of these regions contain the declaration's own write.
    /// </summary>
    private static IEnumerable<SyntaxNode> GetRegionsAfterDeclaration(StatementSyntax declaringStatement) =>
        GetFollowingStatements(declaringStatement)
            .Concat(GetNestedStatements(declaringStatement))
            .Concat(GetForLoopExpressions(declaringStatement));

    private static IEnumerable<SyntaxNode> GetFollowingStatements(StatementSyntax declaringStatement) =>
        declaringStatement.Parent switch
        {
            BlockSyntax block => block.Statements.Skip(block.Statements.IndexOf(declaringStatement) + 1),
            SwitchSectionSyntax section => section.Statements.Skip(section.Statements.IndexOf(declaringStatement) + 1),
            GlobalStatementSyntax { Parent: CompilationUnitSyntax compilationUnit } global => compilationUnit.Members
                .Skip(compilationUnit.Members.IndexOf(global) + 1)
                .OfType<GlobalStatementSyntax>()
                .Select(following => following.Statement),
            _ => [],
        };

    private static IEnumerable<SyntaxNode> GetNestedStatements(StatementSyntax declaringStatement) =>
        declaringStatement
            .DescendantNodes(node => node == declaringStatement || node is not StatementSyntax)
            .OfType<StatementSyntax>();

    private static IEnumerable<SyntaxNode> GetForLoopExpressions(StatementSyntax declaringStatement)
    {
        if (declaringStatement is not ForStatementSyntax forStatement)
        {
            return [];
        }

        return forStatement.Condition is { } condition
            ? forStatement.Incrementors.Prepend(condition)
            : forStatement.Incrementors;
    }

    /// <summary>
    /// Returns whether two calls sit in different branches of the same <c>if</c>/<c>else</c>, switch statement,
    /// switch expression, or conditional expression, so at most one of them runs.
    /// </summary>
    private static bool AreMutuallyExclusive(SyntaxNode first, SyntaxNode second, SemanticModel semanticModel)
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
                AreInExclusiveSections(switchStatement, first, second, semanticModel),
            SwitchExpressionSyntax switchExpression =>
                IsWithinDifferentBranches(first, second, switchExpression.Arms),
            _ => false,
        };
    }

    /// <summary>
    /// Returns whether two calls sit in different sections of a switch statement and neither section can reach the
    /// other through <c>goto case</c>, <c>goto default</c>, or a labeled <c>goto</c>.
    /// </summary>
    private static bool AreInExclusiveSections(
        SwitchStatementSyntax switchStatement,
        SyntaxNode first,
        SyntaxNode second,
        SemanticModel semanticModel)
    {
        var sections = switchStatement.Sections;
        var firstSection = sections.FirstOrDefault(section => section.Span.Contains(first.Span));
        var secondSection = sections.FirstOrDefault(section => section.Span.Contains(second.Span));
        if (firstSection is null || secondSection is null || firstSection == secondSection)
        {
            return false;
        }

        return !CanReach(switchStatement, firstSection, secondSection, semanticModel)
            && !CanReach(switchStatement, secondSection, firstSection, semanticModel);
    }

    private static bool CanReach(
        SwitchStatementSyntax switchStatement,
        SwitchSectionSyntax source,
        SwitchSectionSyntax target,
        SemanticModel semanticModel)
    {
        var visited = new HashSet<SwitchSectionSyntax> { source };
        var pending = new Stack<SwitchSectionSyntax>();
        pending.Push(source);
        while (pending.Count > 0)
        {
            foreach (var next in GetGotoTargets(switchStatement, pending.Pop(), semanticModel))
            {
                if (next == target)
                {
                    return true;
                }

                if (visited.Add(next))
                {
                    pending.Push(next);
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Returns the sections of <paramref name="switchStatement"/> that a <c>goto</c> in <paramref name="section"/> can
    /// transfer control to. <c>goto case</c> and <c>goto default</c> owned by a nested switch are ignored. A
    /// <c>goto case</c> whose value cannot be matched to a section is treated as reaching every section.
    /// </summary>
    private static IEnumerable<SwitchSectionSyntax> GetGotoTargets(
        SwitchStatementSyntax switchStatement,
        SwitchSectionSyntax section,
        SemanticModel semanticModel)
    {
        foreach (var gotoStatement in section.DescendantNodes().OfType<GotoStatementSyntax>())
        {
            foreach (var target in GetGotoTargets(switchStatement, gotoStatement, semanticModel))
            {
                yield return target;
            }
        }
    }

    private static IEnumerable<SwitchSectionSyntax> GetGotoTargets(
        SwitchStatementSyntax switchStatement,
        GotoStatementSyntax gotoStatement,
        SemanticModel semanticModel)
    {
        if (gotoStatement.IsKind(SyntaxKind.GotoStatement))
        {
            return gotoStatement.Expression is IdentifierNameSyntax label
                ? switchStatement.Sections.Where(candidate => candidate.DescendantNodes()
                    .OfType<LabeledStatementSyntax>()
                    .Any(labeled => labeled.Identifier.ValueText == label.Identifier.ValueText))
                : [];
        }

        if (gotoStatement.FirstAncestorOrSelf<SwitchStatementSyntax>() != switchStatement)
        {
            return [];
        }

        if (gotoStatement.IsKind(SyntaxKind.GotoDefaultStatement))
        {
            return switchStatement.Sections.Where(candidate => candidate.Labels.Any(label => label is DefaultSwitchLabelSyntax));
        }

        return gotoStatement.Expression is { } value
            ? GetCaseTargets(switchStatement, value, semanticModel)
            : switchStatement.Sections;
    }

    private static IEnumerable<SwitchSectionSyntax> GetCaseTargets(
        SwitchStatementSyntax switchStatement,
        ExpressionSyntax value,
        SemanticModel semanticModel)
    {
        var constant = semanticModel.GetConstantValue(value);
        if (!constant.HasValue)
        {
            return switchStatement.Sections;
        }

        var matches = switchStatement.Sections
            .Where(candidate => candidate.Labels
                .OfType<CaseSwitchLabelSyntax>()
                .Any(label => semanticModel.GetConstantValue(label.Value) is { HasValue: true } labelValue
                    && Equals(labelValue.Value, constant.Value)))
            .ToList();

        return matches.Count > 0 ? matches : switchStatement.Sections;
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

    /// <summary>
    /// Groups calls on a local that is written after its declaration. Two such calls share a builder only when no
    /// write to the local can happen between them.
    /// </summary>
    private sealed class ReassignedLocalKey(ILocalSymbol local) : IEquatable<ReassignedLocalKey>
    {
        public ILocalSymbol Local { get; } = local;

        public bool Equals(ReassignedLocalKey? other) =>
            other is not null && SymbolEqualityComparer.Default.Equals(Local, other.Local);

        public override bool Equals(object? obj) => Equals(obj as ReassignedLocalKey);

        public override int GetHashCode() => SymbolEqualityComparer.Default.GetHashCode(Local);
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
