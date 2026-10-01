using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace ModularPipelines.Analyzers;

/// <summary>
/// Reports diagnostic MP0020 when one member body calls <c>WithSkipWhen</c> more than once on the same
/// <c>ModuleConfigurationBuilder</c>. Repeated skip conditions are OR-ed, which differs from V3, where a
/// later call replaced an earlier one. Calls are reported when they sit in different statements of one statement
/// list, or run unconditionally in one statement such as a fluent chain. Calls that share an <c>if</c>, switch,
/// conditional, or loop construct are not compared, which accepts some missed reports to avoid false ones.
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

                        // Every call in a fluent chain starts at the receiver, so order by the method name instead.
                        var ordered = calls.OrderBy(call => GetMethodNameLocation(call).SourceSpan.Start).ToList();
                        for (var i = 1; i < ordered.Count; i++)
                        {
                            var repeatedCall = ordered[i];
                            if (ordered.Take(i).Any(earlier => CanRunOnSameBuilder(entry.Key, earlier, repeatedCall, builderType)))
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
    /// Returns whether both calls can configure the same builder in one pass. The calls must sit in different
    /// statements of their innermost common statement list, or both run unconditionally within one statement or
    /// expression body. Branches, loops, and jumps are not analyzed further, so calls that share one <c>if</c>,
    /// switch, or conditional are not reported.
    /// </summary>
    private static bool CanRunOnSameBuilder(
        object builderKey,
        IInvocationOperation earlier,
        IInvocationOperation later,
        INamedTypeSymbol builderType)
    {
        if (earlier.SemanticModel is not { } semanticModel
            || GetSequentialRange(earlier.Syntax, later.Syntax) is not var (first, last))
        {
            return false;
        }

        return GetWritableSymbol(builderKey) is not { } symbol
            || !IsWrittenBetween(symbol, first, last, semanticModel, builderType);
    }

    /// <summary>
    /// Returns the statements of the innermost common statement list that hold each call, or the shared statement or
    /// expression body when both calls run unconditionally within it. Returns <see langword="null"/> otherwise.
    /// </summary>
    private static (SyntaxNode First, SyntaxNode Last)? GetSequentialRange(SyntaxNode earlier, SyntaxNode later)
    {
        var container = GetCommonStatementContainer(earlier, later);
        if (container is null)
        {
            return null;
        }

        if (container is ArrowExpressionClauseSyntax arrow)
        {
            return BothRunUnconditionallyIn(arrow, earlier, later) ? (arrow, arrow) : null;
        }

        var first = GetStatementIn(container, earlier);
        var last = GetStatementIn(container, later);
        if (first is null || last is null)
        {
            return null;
        }

        if (first == last)
        {
            return BothRunUnconditionallyIn(first, earlier, later) ? (first, last) : null;
        }

        return IsInsideDeferredCode(earlier, first) || IsInsideDeferredCode(later, last) ? null : (first, last);
    }

    /// <summary>
    /// Returns the innermost statement list or expression body that contains both calls, or <see langword="null"/>
    /// when the calls only share a member declaration or nothing at all.
    /// </summary>
    private static SyntaxNode? GetCommonStatementContainer(SyntaxNode earlier, SyntaxNode later)
    {
        var earlierAncestors = new HashSet<SyntaxNode>(earlier.Ancestors());
        var container = later.Ancestors().FirstOrDefault(earlierAncestors.Contains);
        while (container is not (null or BlockSyntax or SwitchSectionSyntax or CompilationUnitSyntax or ArrowExpressionClauseSyntax))
        {
            container = container is MemberDeclarationSyntax ? null : container.Parent;
        }

        return container;
    }

    private static bool BothRunUnconditionallyIn(SyntaxNode anchor, SyntaxNode earlier, SyntaxNode later) =>
        GetUnconditionalAnchor(earlier) == anchor && GetUnconditionalAnchor(later) == anchor;

    private static StatementSyntax? GetStatementIn(SyntaxNode list, SyntaxNode call) =>
        call.Ancestors()
            .OfType<StatementSyntax>()
            .FirstOrDefault(statement => statement.Parent == list
                || (statement.Parent is GlobalStatementSyntax global && global.Parent == list));

    /// <summary>
    /// Returns the statement or expression body that contains <paramref name="call"/> when the call runs every time
    /// that statement runs, or <see langword="null"/> when a conditional construct or lambda sits between them.
    /// </summary>
    private static SyntaxNode? GetUnconditionalAnchor(SyntaxNode call)
    {
        foreach (var ancestor in call.Ancestors())
        {
            switch (ancestor)
            {
                case ConditionalExpressionSyntax or SwitchExpressionSyntax or ConditionalAccessExpressionSyntax
                    or AnonymousFunctionExpressionSyntax:
                    return null;
                case BinaryExpressionSyntax binary when binary.IsKind(SyntaxKind.LogicalAndExpression)
                    || binary.IsKind(SyntaxKind.LogicalOrExpression)
                    || binary.IsKind(SyntaxKind.CoalesceExpression):
                    return null;
                case AssignmentExpressionSyntax assignment when assignment.IsKind(SyntaxKind.CoalesceAssignmentExpression):
                    return null;
                case ArrowExpressionClauseSyntax or StatementSyntax:
                    return ancestor;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns whether a lambda or local function sits between <paramref name="call"/> and
    /// <paramref name="statement"/>; such code may run any number of times, or not at all.
    /// </summary>
    private static bool IsInsideDeferredCode(SyntaxNode call, SyntaxNode statement) =>
        call.Ancestors()
            .TakeWhile(ancestor => ancestor != statement)
            .Any(ancestor => ancestor is AnonymousFunctionExpressionSyntax or LocalFunctionStatementSyntax);

    private static ISymbol? GetWritableSymbol(object builderKey) =>
        builderKey switch
        {
            ReassignedLocalKey reassigned => reassigned.Local,
            ILocalSymbol local => local,
            IParameterSymbol parameter => parameter,
            _ => null,
        };

    /// <summary>
    /// Returns whether <paramref name="symbol"/> may be written from the first statement through the last one. Writes
    /// in that range but off the path between the calls also count, which keeps the check conservative. Assignments
    /// such as <c>builder = builder.WithSkipWhen(...)</c> keep the same builder, so they are not counted as writes.
    /// </summary>
    private static bool IsWrittenBetween(
        ISymbol symbol,
        SyntaxNode first,
        SyntaxNode last,
        SemanticModel semanticModel,
        INamedTypeSymbol builderType)
    {
        var dataFlow = (first, last) switch
        {
            (StatementSyntax firstStatement, StatementSyntax lastStatement) =>
                semanticModel.AnalyzeDataFlow(firstStatement, lastStatement),
            (ArrowExpressionClauseSyntax arrow, _) => semanticModel.AnalyzeDataFlow(arrow.Expression),
            _ => null,
        };

        return dataFlow is null
            || !dataFlow.Succeeded
            || (dataFlow.WrittenInside.Contains(symbol, SymbolEqualityComparer.Default)
                && !OnlyReassignsSameBuilder(symbol, first, last, semanticModel, builderType));
    }

    /// <summary>
    /// Returns whether every write to <paramref name="symbol"/> from the first node through the last one assigns a
    /// builder chain that starts from <paramref name="symbol"/> itself. Any other possible write, or no recognized
    /// write at all, returns <see langword="false"/> so the caller stays conservative.
    /// </summary>
    private static bool OnlyReassignsSameBuilder(
        ISymbol symbol,
        SyntaxNode first,
        SyntaxNode last,
        SemanticModel semanticModel,
        INamedTypeSymbol builderType)
    {
        var range = TextSpan.FromBounds(first.SpanStart, last.Span.End);
        var selfAssignments = 0;
        foreach (var identifier in first.SyntaxTree.GetRoot().DescendantNodes(range).OfType<IdentifierNameSyntax>())
        {
            if (!range.Contains(identifier.Span)
                || identifier.Identifier.ValueText != symbol.Name
                || semanticModel.GetOperation(identifier) is not { } reference
                || !SymbolEqualityComparer.Default.Equals(GetReferencedSymbol(reference), symbol))
            {
                continue;
            }

            if (reference.Parent is ISimpleAssignmentOperation assignment && assignment.Target == reference)
            {
                if (!SymbolEqualityComparer.Default.Equals(GetReferencedSymbol(UnwrapChain(assignment.Value, builderType)), symbol))
                {
                    return false;
                }

                selfAssignments++;
            }
            else if (IsPossibleWrite(reference))
            {
                return false;
            }
        }

        return selfAssignments > 0;
    }

    private static bool IsPossibleWrite(IOperation reference) =>
        reference.Parent switch
        {
            IAssignmentOperation assignment => assignment.Target == reference,
            ITupleOperation or IIncrementOrDecrementOperation => true,
            IArgumentOperation argument => argument.Parameter?.RefKind is not RefKind.None,
            _ => reference.Syntax.Parent is RefExpressionSyntax,
        };

    private static ISymbol? GetReferencedSymbol(IOperation operation) =>
        operation switch
        {
            ILocalReferenceOperation local => local.Local,
            IParameterReferenceOperation parameter => parameter.Parameter,
            _ => null,
        };

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
