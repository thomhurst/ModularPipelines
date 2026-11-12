using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace ModularPipelines.Analyzers;

/// <summary>Tracks token provenance over actual control-flow edges, including loop headers and cleanup.</summary>
internal sealed class CancellationTokenReachingValues
{
    private static readonly ConditionalWeakTable<IOperation,
        ConcurrentDictionary<IParameterSymbol, Lazy<CancellationTokenReachingValues?>>> Analyses = new();

    private readonly ControlFlowGraph _graph;
    private bool _assumeTupleReadsFlow;
    private readonly SemanticModel _model;
    private readonly OperationExceptionClassifier _exceptions;
    private readonly ImmutableArray<TextSpan> _earlyCollectionCallSpans;
    private readonly HashSet<IMethodSymbol> _visitedMethods;
    private readonly HashSet<IParameterSymbol> _visitedSources;
    private readonly Dictionary<(TextSpan Span, OperationKind Kind), bool> _values = [];

    private CancellationTokenReachingValues(IOperation root, ControlFlowGraph graph, SemanticModel model, HashSet<IMethodSymbol> visitedMethods, HashSet<IParameterSymbol> visitedSources)
    {
        _graph = graph;
        _model = model;
        _visitedMethods = visitedMethods;
        _visitedSources = visitedSources;
        _exceptions = new OperationExceptionClassifier(model);
        _earlyCollectionCallSpans = OperationExceptionClassifier.GetEarlyCollectionCallSpans(root);
    }

    public static bool? TryGetValue(IOperation reference, IParameterSymbol source, HashSet<IMethodSymbol>? visitedMethods = null, HashSet<IParameterSymbol>? visitedSources = null)
    {
        var root = GetAnalysisRoot(reference);
        if (root?.SemanticModel is not { } model || !NeedsControlFlowAnalysis(root, visitedMethods))
        {
            return null;
        }

        // Mutable captures retain their existing proof. Local function effects
        // are not represented by a direct-read CFG result.
        if (HasCapturedSource(root, source))
        {
            return null;
        }

        if (HasUnsupportedTupleRead(root, reference))
        {
            return GetTupleIndependentValue(root, model, reference, [source], visitedMethods, visitedSources);
        }

        var analysis = GetAnalysis(root, model, source, visitedMethods, visitedSources);
        if (analysis is null)
        {
            return null;
        }

        // No evaluated read means this reference is unreachable, including a
        // return whose cleanup and handlers all exit the callable.
        return !analysis._values.TryGetValue((reference.Syntax.Span, reference.Kind), out var value) || value;
    }

    private static bool NeedsControlFlowAnalysis(IOperation root, HashSet<IMethodSymbol>? visitedMethods)
        => visitedMethods is { Count: > 0 }
            || root.DescendantsAndSelf().Any(static operation => operation is ILoopOperation or ITryOperation);

    private static bool HasCapturedSource(IOperation root, IParameterSymbol source)
    {
        return root.DescendantsAndSelf().OfType<IParameterReferenceOperation>().Any(candidate =>
            SymbolEqualityComparer.Default.Equals(candidate.Parameter, source)
            && candidate.Syntax.Ancestors().Any(static syntax => syntax is
                AnonymousFunctionExpressionSyntax or LocalFunctionStatementSyntax));
    }

    internal static bool? TryGetValueFromSources(IOperation reference, HashSet<IParameterSymbol> sources,
        HashSet<IMethodSymbol> visitedMethods, HashSet<IParameterSymbol>? visitedSources)
    {
        if (sources.Count == 0 || GetAnalysisRoot(reference) is not { SemanticModel: { } model } root)
        {
            return null;
        }

        if (sources.Any(source => HasCapturedSource(root, source)))
        {
            return null;
        }

        if (HasUnsupportedTupleRead(root, reference))
        {
            return GetTupleIndependentValue(root, model, reference, sources, visitedMethods, visitedSources);
        }

        var analysis = Create(root, model, sources.First(), visitedMethods, visitedSources, sources);
        return analysis is null ? null : !analysis._values.TryGetValue((reference.Syntax.Span, reference.Kind), out var value) || value;
    }

    private static bool? GetTupleIndependentValue(IOperation root, SemanticModel model, IOperation reference,
        HashSet<IParameterSymbol> sources, HashSet<IMethodSymbol>? visitedMethods, HashSet<IParameterSymbol>? visitedSources)
    {
        // Give unsupported tuple reads the most optimistic provenance. A negative
        // result still proves a replacement independently of tuple contents;
        // otherwise retain the existing tuple proof rather than certifying a read.
        var analysis = Create(root, model, sources.First(), visitedMethods ?? [with(SymbolEqualityComparer.Default)],
            visitedSources, sources, assumeTupleReadsFlow: true);
        if (analysis is not null
            && analysis._values.TryGetValue((reference.Syntax.Span, reference.Kind), out var value)
            && !value)
        {
            return false;
        }

        return null;
    }

    private static bool HasUnsupportedTupleRead(IOperation root, IOperation reference)
    {
        // Scalar states do not model tuple element storage. Defer only values
        // that depend on a tuple read, preserving unrelated loop diagnostics.
        var operations = root.DescendantsAndSelf().ToArray();
        if (!operations.Any(IsTupleCarrierRead))
        {
            return false;
        }

        var writes = GetScalarWrites(operations);
        var visited = new HashSet<ISymbol>(SymbolEqualityComparer.Default);
        var pending = new Stack<IOperation>();
        pending.Push(reference);
        while (pending.Count > 0)
        {
            foreach (var value in pending.Pop().DescendantsAndSelf())
            {
                if (IsTupleCarrierRead(value))
                {
                    return true;
                }

                if (GetScalarSymbol(value) is { } symbol && visited.Add(symbol))
                {
                    foreach (var write in writes[symbol])
                    {
                        pending.Push(write);
                    }
                }
            }
        }

        return false;
    }

    private static ILookup<ISymbol, IOperation> GetScalarWrites(IEnumerable<IOperation> operations)
    {
        return operations.Select(static operation => operation switch
        {
            IVariableDeclaratorOperation { Initializer.Value: { } value } declaration
                => (Symbol: (ISymbol?) declaration.Symbol, Value: value),
            ISimpleAssignmentOperation assignment
                => (Symbol: GetScalarSymbol(assignment.Target), assignment.Value),
            _ => (Symbol: (ISymbol?) null, Value: (IOperation?) null),
        }).Where(static write => write.Symbol is not null && write.Value is not null)
            .ToLookup(static write => write.Symbol!, static write => write.Value!, SymbolEqualityComparer.Default);
    }

    private static ISymbol? GetScalarSymbol(IOperation operation) => operation switch
    {
        ILocalReferenceOperation local => local.Local,
        IParameterReferenceOperation parameter => parameter.Parameter,
        _ => null,
    };

    private static bool IsTupleCarrierRead(IOperation operation) => operation is IFieldReferenceOperation field
        && field.Field.ContainingType.IsTupleType
        && field.Type?.ToDisplayString() is
            "System.Threading.CancellationToken" or "System.Threading.CancellationTokenSource";

    private static CancellationTokenReachingValues? GetAnalysis(IOperation root, SemanticModel model,
        IParameterSymbol source, HashSet<IMethodSymbol>? visitedMethods, HashSet<IParameterSymbol>? visitedSources)
    {
        // Helper proofs depend on their call-chain recursion guard and must not share
        // the context-free cache or re-enter a Lazy analysis under construction.
        return visitedMethods is { Count: > 0 }
            ? Create(root, model, source, visitedMethods, visitedSources)
            : Analyses.GetValue(root, static _ => new(SymbolEqualityComparer.Default))
                .GetOrAdd(source, _ => new Lazy<CancellationTokenReachingValues?>(() => Create(
                    root, model, source, [with(SymbolEqualityComparer.Default)]))).Value;
    }

    private static IOperation? GetAnalysisRoot(IOperation reference)
    {
        var root = reference;
        while (root.Parent is { } parent)
        {
            if (parent is IAnonymousFunctionOperation or ILocalFunctionOperation)
            {
                // Captured helpers have a separate interprocedural analysis.
                return null;
            }

            root = parent;
        }

        return root;
    }

    private static CancellationTokenReachingValues? Create(IOperation root, SemanticModel model, IParameterSymbol source, HashSet<IMethodSymbol> visitedMethods, HashSet<IParameterSymbol>? visitedSources = null, HashSet<IParameterSymbol>? initialSources = null, bool assumeTupleReadsFlow = false)
    {
        ControlFlowGraph? graph;
        try
        {
            graph = ControlFlowGraph.Create(root.Syntax, model);
        }
        catch (ArgumentException)
        {
            // An incomplete declaration may not have a valid CFG yet.
            return null;
        }

        if (graph is null)
        {
            return null;
        }

        var sources = visitedSources is null
            ? [with(SymbolEqualityComparer.Default)]
            : new HashSet<IParameterSymbol>(visitedSources, SymbolEqualityComparer.Default);
        sources.UnionWith(initialSources ?? [source]);
        var analysis = new CancellationTokenReachingValues(root, graph, model, visitedMethods, sources)
        {
            _assumeTupleReadsFlow = assumeTupleReadsFlow,
        };
        var initial = new State();
        initial.Values.UnionWith(initialSources ?? [source]);
        if (source.ContainingSymbol is IMethodSymbol method)
        {
            foreach (var parameter in method.Parameters.Where(static parameter => parameter.RefKind != RefKind.None))
            {
                initial.Aliases[parameter] = new StorageTargets { Symbols = { parameter } };
            }
        }

        analysis.Run(graph.Root, initial, graph.Blocks[0]);
        return analysis;
    }

    // Heap locations cannot alias tracked symbols, but remain unproven for reads.
    // Keep heap membership through joins so a mixed location cannot strongly restore a local.
    private sealed class StorageTargets
    {
        public HashSet<ISymbol> Symbols { get; } = [with(SymbolEqualityComparer.Default)];

        public bool HasHeap { get; set; }

        public bool IsUnknown => Symbols.Count == 0 && !HasHeap;

        public StorageTargets Clone()
        {
            var copy = new StorageTargets { HasHeap = HasHeap };
            copy.Symbols.UnionWith(Symbols);
            return copy;
        }

        public bool Merge(StorageTargets other)
        {
            if (IsUnknown)
            {
                return false;
            }

            if (other.IsUnknown)
            {
                Symbols.Clear();
                HasHeap = false;
                return true;
            }

            var count = Symbols.Count;
            var hadHeap = HasHeap;
            Symbols.UnionWith(other.Symbols);
            HasHeap |= other.HasHeap;
            return count != Symbols.Count || hadHeap != HasHeap;
        }
    }

    // A state contains facts true on every path to this point. Joining can only
    // remove facts, so even mutually dependent copies converge in finite time.
    private sealed class State
    {
        // Peel the initial acyclic pass from later loop iterations. This bounds
        // traversal to two states per block while retaining first-entry facts.
        public bool HasBackEdge { get; set; }

        public HashSet<ISymbol> Values { get; } = [with(SymbolEqualityComparer.Default)];

        public HashSet<CaptureId> Captures { get; } = [];

        public HashSet<CaptureId> SourceOrNullCaptures { get; } = [];

        public HashSet<TextSpan> EnteredCollectionElements { get; } = [];

        public Dictionary<ISymbol, StorageTargets> Aliases { get; } = [with(SymbolEqualityComparer.Default)];

        public Dictionary<CaptureId, StorageTargets> CaptureAliases { get; } = [];

        public State Clone()
        {
            var copy = new State { HasBackEdge = HasBackEdge };
            copy.Values.UnionWith(Values);
            copy.Captures.UnionWith(Captures);
            copy.SourceOrNullCaptures.UnionWith(SourceOrNullCaptures);
            copy.EnteredCollectionElements.UnionWith(EnteredCollectionElements);
            foreach (var alias in Aliases)
            {
                copy.Aliases.Add(alias.Key, alias.Value.Clone());
            }

            foreach (var alias in CaptureAliases)
            {
                copy.CaptureAliases.Add(alias.Key, alias.Value.Clone());
            }

            return copy;
        }

        public bool Merge(State other)
        {
            var count = Values.Count + Captures.Count + SourceOrNullCaptures.Count + EnteredCollectionElements.Count;
            Values.IntersectWith(other.Values);
            Captures.IntersectWith(other.Captures);
            SourceOrNullCaptures.IntersectWith(other.SourceOrNullCaptures);
            EnteredCollectionElements.IntersectWith(other.EnteredCollectionElements);
            var aliasesChanged = MergeAliases(Aliases, other.Aliases);
            aliasesChanged |= MergeAliases(CaptureAliases, other.CaptureAliases);
            return count != Values.Count + Captures.Count + SourceOrNullCaptures.Count + EnteredCollectionElements.Count || aliasesChanged;
        }

        private static bool MergeAliases<TKey>(Dictionary<TKey, StorageTargets> aliases, Dictionary<TKey, StorageTargets> other)
            where TKey : notnull
        {
            var changed = false;
            foreach (var key in aliases.Keys.Concat(other.Keys).Distinct(aliases.Comparer).ToArray())
            {
                if (!aliases.TryGetValue(key, out var targets))
                {
                    // An absent binding on one path makes the location unknown.
                    aliases.Add(key, new StorageTargets());
                    changed = true;
                }
                else
                {
                    changed |= targets.Merge(other.TryGetValue(key, out var incoming) ? incoming : new StorageTargets());
                }
            }

            return changed;
        }
    }

    private State? Run(ControlFlowRegion region, State input, BasicBlock entry, Action<BasicBlock, State>? outside = null)
    {
        return new Traversal(this, region, outside).Run(input, entry);
    }

    private sealed class Traversal(
        CancellationTokenReachingValues analysis,
        ControlFlowRegion region,
        Action<BasicBlock, State>? outside)
    {
        private readonly Dictionary<(int Ordinal, bool HasBackEdge), State> _states = [];
        private readonly Queue<(BasicBlock Block, bool HasBackEdge)> _pending = new();
        private State? _completion;

        public State? Run(State input, BasicBlock entry)
        {
            Enqueue(entry, input);
            while (_pending.Count > 0)
            {
                var (block, hasBackEdge) = _pending.Dequeue();
                var state = _states[(block.Ordinal, hasBackEdge)].Clone();
                foreach (var operation in block.Operations)
                {
                    analysis.Evaluate(operation, state, block, Exceptional);
                }

                var branchValue = block.BranchValue is { } condition
                    && analysis.Evaluate(condition, state, block, Exceptional);

                Follow(block.FallThroughSuccessor, state, branchValue);
                Follow(block.ConditionalSuccessor, state, branchValue);
            }

            return _completion;
        }

        private void Enqueue(BasicBlock block, State state)
        {
            if (block.Ordinal < region.FirstBlockOrdinal || block.Ordinal > region.LastBlockOrdinal)
            {
                outside?.Invoke(block, state);
                return;
            }

            var key = (block.Ordinal, state.HasBackEdge);
            if (!_states.TryGetValue(key, out var existing))
            {
                _states.Add(key, state.Clone());
                _pending.Enqueue((block, state.HasBackEdge));
            }
            else if (existing.Merge(state))
            {
                _pending.Enqueue((block, state.HasBackEdge));
            }
        }

        private void Complete(State state)
        {
            if (_completion is null)
            {
                _completion = state.Clone();
            }
            else
            {
                _completion.Merge(state);
            }
        }

        private void Follow(ControlFlowBranch? branch, State state, bool branchValue)
        {
            if (branch is null || !IsBranchTaken(branch, state))
            {
                return;
            }

            if (branch.Semantics is ControlFlowBranchSemantics.Throw or ControlFlowBranchSemantics.Rethrow)
            {
                Exceptional(branch.Source, state, null);
                return;
            }

            state = state.Clone();
            RefineNonNullCapture(branch, state);
            if (branch.Source.BranchValue is IIsPatternOperation patternTest)
            {
                var matched = branch.IsConditionalSuccessor == (branch.Source.ConditionKind == ControlFlowConditionKind.WhenTrue);
                BindPattern(patternTest.Pattern, branchValue, state, matched);
            }

            var next = RunFinalizers(branch.FinallyRegions, state);
            if (next is null)
            {
                return;
            }

            if (branch.Semantics == ControlFlowBranchSemantics.StructuredExceptionHandling)
            {
                Complete(next);
            }
            else if (branch.Destination is { } destination)
            {
                if (destination.Ordinal <= branch.Source.Ordinal)
                {
                    next.HasBackEdge = true;
                }

                Enqueue(destination, next);
            }
        }

        private static void BindPattern(IPatternOperation pattern, bool value, State state, bool matched)
        {
            switch (pattern)
            {
                case IDeclarationPatternOperation { DeclaredSymbol: { } symbol } when matched:
                    Set(state.Values, symbol, value);
                    break;
                case IRecursivePatternOperation { DeclaredSymbol: { } symbol } when matched:
                    // Only the whole matched value shares its provenance; property values need their own proof.
                    Set(state.Values, symbol, value);
                    break;
                case INegatedPatternOperation negated:
                    BindPattern(negated.Pattern, value, state, !matched);
                    break;
                case IBinaryPatternOperation { OperatorKind: BinaryOperatorKind.And } binary when matched:
                    BindPattern(binary.LeftPattern, value, state, true);
                    BindPattern(binary.RightPattern, value, state, true);
                    break;
            }
        }

        private static void RefineNonNullCapture(ControlFlowBranch branch, State state)
        {
            if (branch.Source.BranchValue is IIsNullOperation { Operand: IFlowCaptureReferenceOperation capture }
                && state.SourceOrNullCaptures.Contains(capture.Id))
            {
                var isNull = branch.IsConditionalSuccessor ==
                    (branch.Source.ConditionKind == ControlFlowConditionKind.WhenTrue);
                if (!isNull)
                {
                    state.Captures.Add(capture.Id);
                }
            }
        }

        private State? RunFinalizers(IEnumerable<ControlFlowRegion> finalizers, State state)
        {
            var next = state.Clone();
            foreach (var finalizer in finalizers)
            {
                next = analysis.Run(finalizer, next, analysis._graph.Blocks[finalizer.FirstBlockOrdinal], Enqueue);
                if (next is null)
                {
                    return null;
                }
            }

            return next;
        }

        private bool IsBranchTaken(ControlFlowBranch branch, State state)
        {
            if (branch.Source.ConditionKind == ControlFlowConditionKind.None)
            {
                return true;
            }

            if (TryGetBranchCondition(branch.Source.BranchValue, state) is not { } value)
            {
                return true;
            }

            var conditionalTaken = branch.Source.ConditionKind == ControlFlowConditionKind.WhenTrue ? value : !value;
            return branch.IsConditionalSuccessor == conditionalTaken;
        }

        private bool? TryGetBranchCondition(IOperation? condition, State state)
        {
            if (condition?.ConstantValue is { HasValue: true, Value: bool constant })
            {
                return constant;
            }

            if (condition is IIsPatternOperation { Pattern: IConstantPatternOperation pattern } test
                && TryGetConstantValue(test.Value, []) is { HasValue: true } subject
                && pattern.Value.ConstantValue is { HasValue: true } expected)
            {
                return Equals(subject.Value, expected.Value);
            }

            // Only the entry state can use the initializer: backedges carry later iterations.
            if (!state.HasBackEdge && TryGetInitialForCondition(condition) is { } initialCondition)
            {
                return initialCondition;
            }

            return condition is IIsNullOperation nullTest && IsKnownNonNull(nullTest.Operand, [])
                ? false
                : null;
        }

        private Optional<object?> TryGetConstantValue(IOperation operation, HashSet<CaptureId> visited)
        {
            if (operation.ConstantValue.HasValue)
            {
                return operation.ConstantValue;
            }

            if (operation is IConversionOperation { OperatorMethod: null } conversion
                && SymbolEqualityComparer.Default.Equals(conversion.Operand.Type, conversion.Type))
            {
                return TryGetConstantValue(conversion.Operand, visited);
            }

            if (operation is not IFlowCaptureReferenceOperation capture || !visited.Add(capture.Id))
            {
                return default;
            }

            var definitions = analysis._graph.Blocks.SelectMany(static block => block.Operations)
                .SelectMany(static operation => operation.DescendantsAndSelf())
                .OfType<IFlowCaptureOperation>().Where(definition => definition.Id.Equals(capture.Id))
                .Select(definition => TryGetConstantValue(definition.Value, [.. visited])).ToArray();
            return definitions.Length > 0 && definitions[0].HasValue && definitions.All(value => value.HasValue && Equals(value.Value, definitions[0].Value)) ? definitions[0] : default;
        }

        private bool? TryGetInitialForCondition(IOperation? condition)
        {
            if (condition is not IBinaryOperation
                {
                    OperatorMethod: null,
                    LeftOperand: ILocalReferenceOperation local,
                    RightOperand.ConstantValue: { HasValue: true, Value: int limit }
                }
                binary
                || local.Local.Type.SpecialType != SpecialType.System_Int32
                || condition.Syntax.FirstAncestorOrSelf<ForStatementSyntax>() is not
                { Declaration.Variables.Count: 1, Condition: { } loopCondition } loop
                || loopCondition.Span != condition.Syntax.Span
                || loop.Declaration!.Variables[0] is not { Initializer.Value: { } initializer } variable
                || !SymbolEqualityComparer.Default.Equals(analysis._model.GetDeclaredSymbol(variable), local.Local)
                || analysis._model.GetConstantValue(initializer) is not { HasValue: true, Value: int initial })
            {
                return null;
            }

            return binary.OperatorKind switch
            {
                BinaryOperatorKind.LessThan => initial < limit,
                BinaryOperatorKind.LessThanOrEqual => initial <= limit,
                BinaryOperatorKind.GreaterThan => initial > limit,
                BinaryOperatorKind.GreaterThanOrEqual => initial >= limit,
                BinaryOperatorKind.Equals => initial == limit,
                BinaryOperatorKind.NotEquals => initial != limit,
                _ => null,
            };
        }

        private bool IsKnownNonNull(IOperation operation, HashSet<CaptureId> visited)
        {
            if (operation.Type is { IsValueType: true } type
                && type.OriginalDefinition.SpecialType != SpecialType.System_Nullable_T)
            {
                return true;
            }

            if (operation is IConversionOperation { OperatorMethod: null, IsTryCast: false } conversion)
            {
                return IsKnownNonNull(conversion.Operand, visited);
            }

            if (operation is not IFlowCaptureReferenceOperation capture || !visited.Add(capture.Id))
            {
                return false;
            }

            var definitions = analysis._graph.Blocks.SelectMany(static block => block.Operations)
                .SelectMany(static operation => operation.DescendantsAndSelf())
                .OfType<IFlowCaptureOperation>().Where(definition => definition.Id.Equals(capture.Id)).ToArray();
            return definitions.Length > 0 && definitions.All(definition =>
                IsKnownNonNull(definition.Value, [.. visited]));
        }

        // Exceptions raised by expressions use the same worklist as ordinary
        // edges. A catch is never entered merely because a return exists.
        private void DispatchException(BasicBlock block, State state, string? exceptionType)
        {
            // CLR handler search evaluates filters before the second-pass unwind.
            // Keep pending cleanup separate until a handler has been selected.
            List<ControlFlowRegion> finalizers = [];
            for (var current = block.EnclosingRegion; current.EnclosingRegion is { } parent; current = parent)
            {
                if (current.Kind != ControlFlowRegionKind.Try)
                {
                    continue;
                }

                if (parent.Kind == ControlFlowRegionKind.TryAndFinally)
                {
                    var finalizer = parent.NestedRegions.First(static nested => nested.Kind == ControlFlowRegionKind.Finally);
                    finalizers.Add(finalizer);
                }
                else if (parent.Kind == ControlFlowRegionKind.TryAndCatch)
                {
                    var unhandled = DispatchHandlers(parent, state, finalizers, exceptionType);
                    if (unhandled is null)
                    {
                        return;
                    }

                    state = unhandled;
                }
            }

            // Unhandled exceptions still execute cleanup, which may itself throw
            // into an enclosing handler and contains reads that must be recorded.
            RunFinalizers(finalizers, state);
        }

        private State? DispatchHandlers(ControlFlowRegion handlers, State state, List<ControlFlowRegion> finalizers, string? exceptionType)
        {
            foreach (var handler in handlers.NestedRegions.Where(static nested => nested.Kind is
                ControlFlowRegionKind.Catch or ControlFlowRegionKind.FilterAndHandler))
            {
                var catchRegion = GetCatchRegion(handler);
                if (exceptionType is not null && !CanCatch(catchRegion.ExceptionType, exceptionType))
                {
                    continue;
                }

                var unhandled = DispatchMatchingHandler(handler, catchRegion, state, exceptionType, EnterHandler);
                if (unhandled is null)
                {
                    return null;
                }

                state = unhandled;
            }

            return state;

            void EnterHandler(BasicBlock entry, State filtered)
            {
                if (RunFinalizers(finalizers, filtered) is { } cleaned)
                {
                    Enqueue(entry, cleaned);
                }
            }
        }

        private State? DispatchMatchingHandler(ControlFlowRegion handler, ControlFlowRegion catchRegion, State state,
            string? exceptionType, Action<BasicBlock, State> enterHandler)
        {
            if (handler.Kind == ControlFlowRegionKind.Catch)
            {
                enterHandler(analysis._graph.Blocks[handler.FirstBlockOrdinal], state);
                return exceptionType is not null || IsCatchAll(handler) ? null : state;
            }

            return DispatchFilter(handler, catchRegion, state, exceptionType, enterHandler);
        }

        private static ControlFlowRegion GetCatchRegion(ControlFlowRegion handler)
        {
            return handler.Kind == ControlFlowRegionKind.Catch
                ? handler
                : handler.NestedRegions.First(static nested => nested.Kind == ControlFlowRegionKind.Catch);
        }

        private State? DispatchFilter(ControlFlowRegion handler, ControlFlowRegion catchRegion, State state,
            string? exceptionType, Action<BasicBlock, State> enterHandler)
        {
            var filter = handler.NestedRegions.First(static nested => nested.Kind == ControlFlowRegionKind.Filter);
            var rejected = analysis.Run(filter, state.Clone(), analysis._graph.Blocks[filter.FirstBlockOrdinal], enterHandler);
            if (exceptionType is not null || IsCatchAll(catchRegion))
            {
                return rejected;
            }

            if (rejected is not null)
            {
                // The exception type can also skip this filter entirely.
                state = state.Clone();
                state.Merge(rejected);
            }

            return state;
        }

        private bool CanCatch(ITypeSymbol? catchType, string exceptionType)
        {
            if (catchType is null || catchType.SpecialType == SpecialType.System_Object)
            {
                return true;
            }

            var thrown = analysis._model.Compilation.GetTypeByMetadataName(exceptionType);
            if (thrown is null)
            {
                return true;
            }

            for (var type = thrown; type is not null; type = type.BaseType)
            {
                if (SymbolEqualityComparer.Default.Equals(type, catchType))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsCatchAll(ControlFlowRegion handler)
        {
            return handler.ExceptionType is null
                || handler.ExceptionType.SpecialType == SpecialType.System_Object
                || SymbolEqualityComparer.Default.Equals(handler.ExceptionType, analysis._model.Compilation.GetTypeByMetadataName("System.Exception"));
        }

        private void Exceptional(BasicBlock block, State state, string? exceptionType)
        {
            // An exception abandons every in-progress collection evaluation.
            // A retry must model constructor/Add/spread calls again.
            state.EnteredCollectionElements.Clear();
            if (region.Kind == ControlFlowRegionKind.Filter)
            {
                // A throwing filter is treated as false, retaining its writes.
                Complete(state);
            }
            else
            {
                DispatchException(block, state, exceptionType);
            }
        }
    }

    private bool Evaluate(IOperation operation, State state, BasicBlock block, Action<BasicBlock, State, string?> exceptional)
    {
        // Enter each element once, including when the CFG hoists a conditional element.
        // Constructor/Add/spread boundaries cannot interrupt the element's nested operations.
        foreach (var span in _earlyCollectionCallSpans)
        {
            if (span.Contains(operation.Syntax.Span) && state.EnteredCollectionElements.Add(span))
            {
                exceptional(block, state.Clone(), null);
            }
        }

        var value = operation switch
        {
            IParameterReferenceOperation parameter => ReadSymbol(parameter.Parameter, state),
            ILocalReferenceOperation local => ReadLocal(local.Local, state),
            IFlowCaptureReferenceOperation capture => state.Captures.Contains(capture.Id),
            IFlowCaptureOperation capture => EvaluateCapture(capture, state, block, exceptional),
            IIsPatternOperation pattern => EvaluatePattern(pattern, state, block, exceptional),
            ISimpleAssignmentOperation assignment => EvaluateAssignment(assignment, state, block, exceptional),
            IDeconstructionAssignmentOperation deconstruction => EvaluateDeconstruction(deconstruction, state, block, exceptional),
            IConversionOperation conversion => EvaluateConversion(conversion, state, block, exceptional),
            IPropertyReferenceOperation property => EvaluateProperty(property, state, block, exceptional),
            IInvocationOperation invocation => EvaluateInvocation(invocation, state, block, exceptional),
            IObjectCreationOperation creation => EvaluateCreation(creation, state, block, exceptional),
            _ => EvaluateOther(operation, state, block, exceptional),
        };
        if (operation is ICollectionExpressionOperation)
        {
            state.EnteredCollectionElements.RemoveWhere(span => operation.Syntax.Span.Contains(span));
        }

        var key = (operation.Syntax.Span, operation.Kind);
        _values[key] = (!_values.TryGetValue(key, out var existing) || existing) && value;

        return value;
    }

    private bool EvaluatePattern(IIsPatternOperation pattern, State state, BasicBlock block, Action<BasicBlock, State, string?> exceptional)
    {
        var value = Evaluate(pattern.Value, state, block, exceptional);
        Evaluate(pattern.Pattern, state, block, exceptional);
        if (pattern.Pattern is IDeclarationPatternOperation { MatchesNull: true, DeclaredSymbol: { } symbol })
        {
            // A var pattern always binds, even when its boolean result is stored rather than branched on.
            Set(state.Values, symbol, value);
        }

        // Carry the subject snapshot to Follow; binding belongs only to the matching successor.
        return value;
    }

    private bool EvaluateCapture(IFlowCaptureOperation capture, State state, BasicBlock block, Action<BasicBlock, State, string?> exceptional)
    {
        var value = Evaluate(capture.Value, state, block, exceptional);
        Set(state.Captures, capture.Id, value);
        Set(state.SourceOrNullCaptures, capture.Id, value || IsSourceOrNull(capture.Value, state));
        state.CaptureAliases[capture.Id] = ResolveStorage(capture.Value, state);
        return value;
    }

    private static bool IsSourceOrNull(IOperation operation, State state) => operation switch
    {
        { ConstantValue: { HasValue: true, Value: null } } => true,
        IConversionOperation { OperatorMethod: null } conversion => IsSourceOrNull(conversion.Operand, state),
        IFlowCaptureReferenceOperation capture => state.SourceOrNullCaptures.Contains(capture.Id),
        _ => false,
    };

    private bool EvaluateAssignment(ISimpleAssignmentOperation assignment, State state, BasicBlock block, Action<BasicBlock, State, string?> exceptional)
    {
        EvaluateTarget(assignment.Target, state, block, exceptional);

        var value = Evaluate(assignment.Value, state, block, exceptional);
        if (assignment.IsRef)
        {
            if (assignment.Target is ILocalReferenceOperation alias)
            {
                state.Aliases[alias.Local] = ResolveStorage(assignment.Value, state);
            }
            else if (assignment.Target is IParameterReferenceOperation parameter)
            {
                state.Aliases[parameter.Parameter] = ResolveStorage(assignment.Value, state);
            }

            // Ref locals and parameters retain their new backing storage.
            // A ref-field rebind changes the field's location, not its referent.
            // Field reads remain unproven and subsequent through-ref stores still
            // invalidate compatible tracked storage conservatively.
        }
        else
        {
            // Provenance tracking and store exceptions are independent: an untracked
            // struct field can be nonthrowing, while array/field stores have known types.
            foreach (var exceptionType in _exceptions.Classify(assignment))
            {
                exceptional(block, state.Clone(), exceptionType);
            }

            StoreTarget(assignment.Target, value, state);
        }

        return value;
    }

    private bool EvaluateDeconstruction(IDeconstructionAssignmentOperation assignment, State state, BasicBlock block, Action<BasicBlock, State, string?> exceptional)
    {
        EvaluateTarget(assignment.Target, state, block, exceptional);
        List<(IOperation Target, bool Value)> stores = [];
        EvaluateDeconstructionValues(assignment.Target, assignment.Value, stores, state, block, exceptional);
        foreach (var exceptionType in _exceptions.Classify(assignment))
        {
            exceptional(block, state.Clone(), exceptionType);
        }

        // Evaluate every RHS element before any store, including swaps and nested tuples.
        foreach (var (target, value) in stores)
        {
            foreach (var exceptionType in _exceptions.Classify(target, deconstructionStore: true))
            {
                exceptional(block, state.Clone(), exceptionType);
            }

            StoreTarget(target, value, state);
        }

        return false;
    }

    private void EvaluateDeconstructionValues(IOperation target, IOperation value, List<(IOperation Target, bool Value)> stores,
        State state, BasicBlock block, Action<BasicBlock, State, string?> exceptional)
    {
        if (target is IDeclarationExpressionOperation declaration)
        {
            target = declaration.Expression;
        }

        if (target is not ITupleOperation targets)
        {
            stores.Add((target, Evaluate(value, state, block, exceptional)));
            return;
        }

        var originalValue = value;
        List<IConversionOperation> conversions = [];
        while (value is IConversionOperation { OperatorMethod: null } conversion)
        {
            conversions.Add(conversion);
            value = conversion.Operand;
        }

        if (value is ITupleOperation values && values.Elements.Length == targets.Elements.Length)
        {
            for (var index = 0; index < targets.Elements.Length; index++)
            {
                EvaluateDeconstructionValues(targets.Elements[index], values.Elements[index], stores, state, block, exceptional);
            }

            TransferConversionExceptions(conversions, state, block, exceptional);
        }
        else
        {
            Evaluate(originalValue, state, block, exceptional);
            AddUnknownStores(target, stores);
        }
    }

    private void TransferConversionExceptions(IEnumerable<IConversionOperation> conversions,
        State state, BasicBlock block, Action<BasicBlock, State, string?> exceptional)
    {
        foreach (var conversion in conversions)
        {
            foreach (var exceptionType in _exceptions.Classify(conversion))
            {
                exceptional(block, state.Clone(), exceptionType);
            }
        }
    }

    private void EvaluateTarget(IOperation target, State state, BasicBlock block, Action<BasicBlock, State, string?> exceptional)
    {
        // Location operands precede the RHS, but dereferencing or invoking a setter
        // happens at the store, after RHS evaluation and preceding tuple stores.
        switch (target)
        {
            case IDeclarationExpressionOperation declaration:
                EvaluateTarget(declaration.Expression, state, block, exceptional);
                break;
            case ITupleOperation tuple:
                foreach (var element in tuple.Elements)
                {
                    EvaluateTarget(element, state, block, exceptional);
                }

                break;
            case IArrayElementReferenceOperation array:
                Evaluate(array.ArrayReference, state, block, exceptional);
                foreach (var index in array.Indices)
                {
                    Evaluate(index, state, block, exceptional);
                }

                break;
            case IPropertyReferenceOperation property:
                if (property.Instance is { } receiver)
                {
                    Evaluate(receiver, state, block, exceptional);
                }

                foreach (var argument in property.Arguments)
                {
                    Evaluate(argument.Value, state, block, exceptional);
                }

                // A ref-return getter computes the storage location before the RHS.
                if (property.Property.ReturnsByRef || property.Property.ReturnsByRefReadonly)
                {
                    exceptional(block, state.Clone(), null);
                }

                break;
            case IInvocationOperation invocation:
                Evaluate(invocation, state, block, exceptional);
                break;
            case IFieldReferenceOperation { Instance: { } fieldReceiver }:
                Evaluate(fieldReceiver, state, block, exceptional);
                break;
            default:
                foreach (var child in target.ChildOperations)
                {
                    Evaluate(child, state, block, exceptional);
                }

                break;
        }
    }

    private static void AddUnknownStores(IOperation target, List<(IOperation Target, bool Value)> stores)
    {
        if (target is IDeclarationExpressionOperation declaration)
        {
            target = declaration.Expression;
        }

        if (target is ITupleOperation tuple)
        {
            foreach (var element in tuple.Elements)
            {
                AddUnknownStores(element, stores);
            }
        }
        else
        {
            stores.Add((target, false));
        }
    }

    private static bool ReadLocal(ILocalSymbol local, State state)
    {
        return local.RefKind == RefKind.None
            ? state.Values.Contains(local)
            : ReadSymbol(local, state);
    }

    private static bool ReadSymbol(ISymbol symbol, State state)
    {
        return state.Aliases.TryGetValue(symbol, out var targets)
            ? !targets.IsUnknown && !targets.HasHeap && targets.Symbols.All(state.Values.Contains)
            : state.Values.Contains(symbol);
    }

    private static StorageTargets ResolveStorage(IOperation reference, State state)
    {
        return reference switch
        {
            ILocalReferenceOperation { Local.RefKind: RefKind.None } local => new StorageTargets { Symbols = { local.Local } },
            ILocalReferenceOperation local when state.Aliases.TryGetValue(local.Local, out var targets) => targets.Clone(),
            IParameterReferenceOperation parameter when state.Aliases.TryGetValue(parameter.Parameter, out var targets) => targets.Clone(),
            IParameterReferenceOperation parameter => new StorageTargets { Symbols = { parameter.Parameter } },
            IFlowCaptureReferenceOperation capture when state.CaptureAliases.TryGetValue(capture.Id, out var targets) => targets.Clone(),
            IConversionOperation conversion => ResolveStorage(conversion.Operand, state),
            IParenthesizedOperation parenthesized => ResolveStorage(parenthesized.Operand, state),
            // Array elements and fields cannot designate tracked stack locals or parameters.
            IArrayElementReferenceOperation or IFieldReferenceOperation { Field.RefKind: RefKind.None } => new StorageTargets { HasHeap = true },
            _ => new StorageTargets(),
        };
    }

    private static void StoreTarget(IOperation target, bool value, State state)
    {
        if (target is IDeclarationExpressionOperation declaration)
        {
            target = declaration.Expression;
        }

        if (target is IDiscardOperation)
        {
            return;
        }

        var directReference = target is ILocalReferenceOperation or IParameterReferenceOperation or IFlowCaptureReferenceOperation;
        if (!directReference && target is not IFieldReferenceOperation { Field.RefKind: not RefKind.None })
        {
            return;
        }

        StoreResolvedTarget(target, value, state);
    }

    private static void StoreResolvedTarget(IOperation target, bool value, State state)
    {
        var targets = ResolveStorage(target, state);
        if (targets.IsUnknown)
        {
            // An unknown ref-return can alias tracked storage of the same type.
            // Type parameters remain conservative because their substitutions may match.
            state.Values.RemoveWhere(symbol => target.Type is null || symbol switch
            {
                ILocalSymbol local => MayAlias(target.Type, local.Type),
                IParameterSymbol parameter => MayAlias(target.Type, parameter.Type),
                _ => true,
            });
        }
        else if (targets.Symbols.Count == 1 && !targets.HasHeap)
        {
            Set(state.Values, targets.Symbols.Single(), value);
        }
        else if (!value)
        {
            state.Values.ExceptWith(targets.Symbols);
        }
    }

    private static bool MayAlias(ITypeSymbol target, ITypeSymbol candidate)
    {
        return target is ITypeParameterSymbol || candidate is ITypeParameterSymbol
            || SymbolEqualityComparer.Default.Equals(target, candidate);
    }

    private bool EvaluateConversion(IConversionOperation conversion, State state, BasicBlock block, Action<BasicBlock, State, string?> exceptional)
    {
        var value = Evaluate(conversion.Operand, state, block, exceptional);
        foreach (var exceptionType in _exceptions.Classify(conversion))
        {
            exceptional(block, state.Clone(), exceptionType);
        }

        return value;
    }

    private bool EvaluateProperty(IPropertyReferenceOperation property, State state, BasicBlock block, Action<BasicBlock, State, string?> exceptional)
    {
        var instanceValue = property.Instance is { } instance && Evaluate(instance, state, block, exceptional);
        foreach (var argument in property.Arguments)
        {
            Evaluate(argument.Value, state, block, exceptional);
        }

        exceptional(block, state.Clone(), null);
        return instanceValue && property.Property.Name == "Token"
            && property.Property.ContainingType.ToDisplayString() == "System.Threading.CancellationTokenSource";
    }

    private bool EvaluateInvocation(IInvocationOperation invocation, State state, BasicBlock block, Action<BasicBlock, State, string?> exceptional)
    {
        var receiverValue = invocation.Instance is { } receiver && Evaluate(receiver, state, block, exceptional);
        var arguments = invocation.Arguments.Select(argument => Evaluate(argument.Value, state, block, exceptional)).ToArray();
        InvalidateRefArguments(invocation.Arguments, state);
        if (!IsBuiltInIteration(invocation))
        {
            exceptional(block, state.Clone(), null);
        }

        return (receiverValue && invocation.TargetMethod is
        { Name: "GetValueOrDefault", Parameters.Length: 0, ContainingType.OriginalDefinition.SpecialType: SpecialType.System_Nullable_T })
            || (invocation.TargetMethod.Name == "CreateLinkedTokenSource"
                && invocation.TargetMethod.ContainingType.ToDisplayString() == "System.Threading.CancellationTokenSource"
                && arguments.Any(static argument => argument))
            || ModuleAuthoringAnalysis.FlowsFromCancellationTokenSourceHelper(invocation, _model.Compilation, arguments, receiverValue, _visitedMethods, _visitedSources);
    }

    private static void InvalidateRefArguments(IEnumerable<IArgumentOperation> arguments, State state)
    {
        foreach (var argument in arguments.Where(static argument => argument.Parameter?.RefKind is RefKind.Ref or RefKind.Out))
        {
            InvalidateArgument(argument.Value, state);
        }
    }

    private bool EvaluateCreation(IObjectCreationOperation creation, State state, BasicBlock block, Action<BasicBlock, State, string?> exceptional)
    {
        foreach (var argument in creation.Arguments)
        {
            Evaluate(argument.Value, state, block, exceptional);
        }

        InvalidateRefArguments(creation.Arguments, state);
        exceptional(block, state.Clone(), null);
        if (creation.Initializer is { } initializer)
        {
            Evaluate(initializer, state, block, exceptional);
        }

        return false;
    }

    private bool EvaluateOther(IOperation operation, State state, BasicBlock block, Action<BasicBlock, State, string?> exceptional)
    {
        if (operation is IAnonymousFunctionOperation or IFlowAnonymousFunctionOperation or ILocalFunctionOperation)
        {
            return false;
        }

        var children = operation.ChildOperations.Select(child => Evaluate(child, state, block, exceptional)).ToArray();
        InvalidateDynamicRefArguments(operation, state);
        foreach (var exceptionType in _exceptions.Classify(operation))
        {
            exceptional(block, state.Clone(), exceptionType);
        }

        return (_assumeTupleReadsFlow && IsTupleCarrierRead(operation))
            || (operation is IArrayInitializerOperation or IArrayCreationOperation or ICollectionExpressionOperation or ISpreadOperation
                && children.Any(static child => child));
    }

    private static void InvalidateDynamicRefArguments(IOperation operation, State state)
    {
        var arguments = operation switch
        {
            IDynamicInvocationOperation invocation => invocation.Arguments,
            IDynamicObjectCreationOperation creation => creation.Arguments,
            _ => [],
        };
        for (var index = 0; index < arguments.Length; index++)
        {
            var refKind = operation switch
            {
                IDynamicInvocationOperation invocation => invocation.GetArgumentRefKind(index),
                IDynamicObjectCreationOperation creation => creation.GetArgumentRefKind(index),
                _ => null,
            };
            if (refKind is RefKind.Ref or RefKind.Out)
            {
                InvalidateArgument(arguments[index], state);
            }
        }
    }

    private static void InvalidateArgument(IOperation argument, State state)
    {
        StoreTarget(argument, false, state);
    }

    private bool IsBuiltInIteration(IInvocationOperation invocation)
    {
        // Roslyn's operation CFG represents array/string foreach using an
        // enumerator too. The compiler emits indexed iteration for these types,
        // so those synthetic MoveNext/Dispose calls cannot throw at runtime.
        return invocation.IsImplicit
            && invocation.Syntax.AncestorsAndSelf().OfType<ForEachStatementSyntax>()
                .Any(loop => loop.Expression.Span.Contains(invocation.Syntax.Span)
                    && _model.GetTypeInfo(loop.Expression).Type is { } type
                    && (type is IArrayTypeSymbol || type.SpecialType == SpecialType.System_String));
    }

    private static void Set<T>(HashSet<T> values, T key, bool value)
    {
        if (value)
        {
            values.Add(key);
        }
        else
        {
            values.Remove(key);
        }
    }
}
