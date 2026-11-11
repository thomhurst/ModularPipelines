using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace ModularPipelines.Analyzers;

internal sealed class ReachingDefinitionFlow(
    ControlFlowGraph graph,
    IOperation invocation,
    LocalDefinitionTransfer transfer,
    bool analyzeReceiverTypes = true,
    bool completingReturn = false,
    IOperation? operationRoot = null)
{
    private readonly IOperation _operationRoot = operationRoot ?? GetOperationRoot(invocation);
    private readonly ImmutableArray<TextSpan> _collectionCallSpans =
        OperationExceptionClassifier.GetEarlyCollectionCallSpans(operationRoot ?? GetOperationRoot(invocation));

    private readonly HashSet<TextSpan> _reaching = [];
    private readonly Dictionary<string, HashSet<TextSpan>> _inputs = [];
    private readonly Queue<(int Block, ImmutableArray<int> Continuation, FilterContext? Filter,
        ImmutableDictionary<int, ITypeSymbol?> Exceptions)> _queue = [];
    private ImmutableDictionary<int, ITypeSymbol?> _activeExceptions = ImmutableDictionary<int, ITypeSymbol?>.Empty;

    private OperationExceptionClassifier? _classifier;

    private OperationExceptionClassifier CreateClassifier() => _classifier ??= new(_operationRoot.SemanticModel,
        value => analyzeReceiverTypes && IsKnownNonNull(value, [with(SymbolEqualityComparer.Default)]),
        value => analyzeReceiverTypes && HasExactArrayType(value, value.Type, [with(SymbolEqualityComparer.Default)]));

    // Query one callable only. Unknown calls can throw, but their bodies never transfer local writes.
    // A selected return contributes only after its finally blocks complete without abandoning it.
    public static bool TryGetReachingLocalValues(IOperation root, IOperation operation, ILocalSymbol local,
        out ImmutableArray<IOperation> values, out bool inheritsIncomingValue, bool analyzeReceiverTypes = true)
    {
        values = [];
        inheritsIncomingValue = false;
        if (CreateGraph(root) is not { } rootGraph
            || FindGraph(rootGraph, (operation is IReturnOperation { ReturnedValue: { } returnedValue } ? returnedValue : operation).Syntax.Span) is not { } graph)
        {
            return false;
        }

        var transfer = new LocalDefinitionTransfer(root, local);
        var flow = new ReachingDefinitionFlow(graph, operation, transfer, analyzeReceiverTypes,
            operationRoot: root);
        flow.Analyze();
        return transfer.TryGetValues(flow._reaching, out values, out inheritsIncomingValue);
    }

    private void Analyze()
    {
        Enqueue(0, [], LocalDefinitionTransfer.CreateIncomingState());
        AnalyzePendingBlocks();
    }

    private void AnalyzePendingBlocks()
    {
        while (_queue.Count > 0)
        {
            var (ordinal, continuation, filter, exceptions) = _queue.Dequeue();
            _activeExceptions = exceptions;
            var block = graph.Blocks[ordinal];
            var state = new HashSet<TextSpan>(_inputs[Key(ordinal, continuation, filter, _activeExceptions)]);
            if (completingReturn && block.Kind == BasicBlockKind.Exit)
            {
                _reaching.UnionWith(state);
            }

            foreach (var operation in block.Operations)
            {
                Visit(operation, state, block, continuation, filter);
            }

            if (block.BranchValue is { } branchValue)
            {
                Visit(branchValue, state, block, continuation, filter);
            }

            Follow(block.FallThroughSuccessor, state, continuation, filter);
            Follow(block.ConditionalSuccessor, state, continuation, filter);
        }
    }

    private void Enqueue(int ordinal, ImmutableArray<int> continuation, HashSet<TextSpan> state,
        FilterContext? filter = null, ImmutableDictionary<int, ITypeSymbol?>? exceptions = null)
    {
        exceptions ??= _activeExceptions;
        var key = Key(ordinal, continuation, filter, exceptions);
        if (!_inputs.TryGetValue(key, out var previous))
        {
            _inputs.Add(key, [.. state]);
        }
        else
        {
            var count = previous.Count;
            previous.UnionWith(state);
            if (count == previous.Count)
            {
                return;
            }
        }

        _queue.Enqueue((ordinal, continuation, filter, exceptions));
    }

    private void Visit(IOperation operation, HashSet<TextSpan> state, BasicBlock block,
        ImmutableArray<int> continuation, FilterContext? filter)
    {
        if (state.Count == 0)
        {
            return;
        }

        // Delegate bodies have their own graph and execute only when invoked.
        if (operation is IFlowAnonymousFunctionOperation)
        {
            return;
        }

        FollowEarlyCollectionExceptions(operation, state, block, continuation, filter);

        if (operation is IDeconstructionAssignmentOperation deconstruction)
        {
            VisitDeconstructionTarget(deconstruction.Target, state, block, continuation, filter);
            Visit(deconstruction.Value, state, block, continuation, filter);
            if (state.Count == 0)
            {
                return;
            }

            if (operation.Syntax.Span == invocation.Syntax.Span)
            {
                _reaching.UnionWith(state);
            }

            // Deconstruct calls and element conversions precede stores; later stores see earlier local writes.
            if (deconstruction.Syntax is AssignmentExpressionSyntax syntax
                && _operationRoot.SemanticModel is { } model
                && CanDeconstructionMutateReceiver(model.GetDeconstructionInfo(syntax)))
            {
                transfer.InvalidateStorage(deconstruction.Value, state);
            }

            FollowOperationExceptions(deconstruction, state, block, continuation, filter);
            StoreDeconstructionTarget(deconstruction.Target, deconstruction.Value, state, block, continuation, filter);
            return;
        }

        // Compound assignment reads its previous targets before evaluating the right-hand side.
        var addedTargets = transfer.CaptureDelegateInputs(operation, state);
        foreach (var child in operation.ChildOperations)
        {
            Visit(child, state, block, continuation, filter);
        }

        if (state.Count == 0)
        {
            return;
        }

        if (operation.Syntax.Span == invocation.Syntax.Span)
        {
            _reaching.UnionWith(state);
        }

        // A callee can mutate ref/out storage or retained refs before throwing, after all arguments evaluate.
        transfer.InvalidateByReferenceArguments(operation, state);
        foreach (var argument in operation.ChildOperations.OfType<IArgumentOperation>())
        {
            if (argument.Value.Type?.IsRefLikeType == true)
            {
                transfer.InvalidateStorage(argument.Value, state);
            }
        }

        InvalidateMutableReceiver(operation, state);
        // A setter may mutate storage and then throw. Ordinary local stores occur only on success.
        var target = operation switch
        {
            IAssignmentOperation assignment => assignment.Target,
            IIncrementOrDecrementOperation increment => increment.Target,
            IEventAssignmentOperation subscription => subscription.EventReference,
            _ => null,
        };
        if (target is not null && target is not ILocalReferenceOperation)
        {
            transfer.InvalidateStorage(target, state);
        }

        FollowOperationExceptions(operation, state, block, continuation, filter);
        transfer.ApplyStore(operation, state, addedTargets);
    }

    private void InvalidateMutableReceiver(IOperation operation, HashSet<TextSpan> state)
    {
        var (receiver, method) = operation switch
        {
            IInvocationOperation call => (call.Instance, call.TargetMethod),
            IPropertyReferenceOperation property when property.Parent is not ISimpleAssignmentOperation assignment
                || !ReferenceEquals(assignment.Target, property)
                || property.Property.ReturnsByRef || property.Property.ReturnsByRefReadonly =>
                (property.Instance, property.Property.GetMethod),
            _ => ((IOperation?) null, (IMethodSymbol?) null),
        };
        // Definite reference receivers mutate the object, not the local holding it. Readonly
        // methods/structs cannot mutate storage; conversions operate on a temporary copy.
        if (receiver is { Type.IsReferenceType: false } && method is { IsReadOnly: false }
            && receiver.Type is not INamedTypeSymbol { IsReadOnly: true }
            && receiver is not IConversionOperation)
        {
            transfer.InvalidateStorage(receiver, state);
        }
    }

    private static bool CanDeconstructionMutateReceiver(DeconstructionInfo info) =>
        info.Method is { IsReadOnly: false }
        || (!info.Nested.IsDefaultOrEmpty && info.Nested.Any(CanDeconstructionMutateReceiver));

    private void VisitDeconstructionTarget(IOperation target, HashSet<TextSpan> state, BasicBlock block,
        ImmutableArray<int> continuation, FilterContext? filter)
    {
        if (target is ITupleOperation or IDeclarationExpressionOperation)
        {
            foreach (var child in target.ChildOperations)
            {
                VisitDeconstructionTarget(child, state, block, continuation, filter);
            }
        }
        else if (target is IPropertyReferenceOperation { Property.ReturnsByRef: true })
        {
            Visit(target, state, block, continuation, filter);
        }
        else
        {
            // Receivers and indices are evaluated now; the actual store happens after the RHS.
            foreach (var child in target.ChildOperations)
            {
                Visit(child, state, block, continuation, filter);
            }
        }
    }

    private void StoreDeconstructionTarget(IOperation target, IOperation? value, HashSet<TextSpan> state,
        BasicBlock block, ImmutableArray<int> continuation, FilterContext? filter)
    {
        if (state.Count == 0)
        {
            return;
        }

        if (target is IDeclarationExpressionOperation declaration)
        {
            target = declaration.Expression;
        }

        if (target is ITupleOperation tuple)
        {
            StoreDeconstructionTuple(tuple, value, state, block, continuation, filter);
            return;
        }

        transfer.ApplyTargetStore(target, value, state);
        if (target is not ILocalReferenceOperation
            && target is not IPropertyReferenceOperation { Property.ReturnsByRef: true })
        {
            FollowOperationExceptions(target, state, block, continuation, filter, deconstructionStore: true, storedValue: value);
        }
    }

    private void StoreDeconstructionTuple(ITupleOperation tuple, IOperation? value, HashSet<TextSpan> state,
        BasicBlock block, ImmutableArray<int> continuation, FilterContext? filter)
    {
        while (value is IConversionOperation conversion)
        {
            value = conversion.Operand;
        }

        var values = value as ITupleOperation;
        for (var index = 0; index < tuple.Elements.Length; index++)
        {
            StoreDeconstructionTarget(tuple.Elements[index], values?.Elements[index], state, block,
                continuation, filter);
        }
    }

    private void FollowOperationExceptions(IOperation operation, HashSet<TextSpan> state, BasicBlock block,
        ImmutableArray<int> continuation, FilterContext? filter, bool deconstructionStore = false,
        IOperation? storedValue = null)
    {
        foreach (var exceptionName in CreateClassifier().Classify(operation, deconstructionStore, storedValue))
        {
            FollowException(block, exceptionName is null ? null : _operationRoot.SemanticModel?.Compilation.GetTypeByMetadataName(exceptionName), state, continuation, filter);
        }
    }

    private bool HasExactArrayType(IOperation value, ITypeSymbol? arrayType, HashSet<ILocalSymbol> visited)
    {
        while (value is IConversionOperation { OperatorMethod: null } conversion)
        {
            value = conversion.Operand;
        }

        if (value is IArrayCreationOperation)
        {
            return SymbolEqualityComparer.Default.Equals(value.Type, arrayType);
        }

        if (value is not ILocalReferenceOperation reference || !visited.Add(reference.Local))
        {
            return false;
        }

        var root = _operationRoot;

        // Disable receiver proofs in the nested flow to avoid recursive array/field analysis.
        var known = !HasUntrackedWrites(root, reference.Local, value)
            && TryGetReachingLocalValues(root, value, reference.Local, out var values, out var incoming,
                analyzeReceiverTypes: false)
            && !incoming && !values.IsEmpty
            && values.All(candidate => HasExactArrayType(candidate, arrayType, visited));
        visited.Remove(reference.Local);
        return known;
    }

    private void FollowEarlyCollectionExceptions(IOperation operation, HashSet<TextSpan> state, BasicBlock block,
        ImmutableArray<int> continuation, FilterContext? filter)
    {
        // Constructors, Add, and spread iteration can run before later element evaluations.
        // Use source spans because CFG branching can hoist elements out of the collection operation.
        if (_collectionCallSpans.Any(span => span.Contains(operation.Syntax.Span)))
        {
            FollowException(block, null, state, continuation, filter);
        }
    }

    private bool IsKnownNonNull(IOperation value, HashSet<ILocalSymbol> visited)
    {
        while (value is IConversionOperation conversion && PreservesNonNull(conversion))
        {
            value = conversion.Operand;
        }

        if (value is IObjectCreationOperation or IInstanceReferenceOperation || IsNonNullBoxing(value))
        {
            return true;
        }

        if (value is not ILocalReferenceOperation reference || !visited.Add(reference.Local))
        {
            return false;
        }

        var root = _operationRoot;

        // Reuse reaching definitions without recursively proving receivers in that analysis.
        var known = HasOnlyNonNullDefinitions(root, value, reference.Local, visited);
        visited.Remove(reference.Local);
        return known;
    }

    private static bool PreservesNonNull(IConversionOperation conversion) =>
        conversion.Conversion is { IsIdentity: true } or { IsReference: true }
            && (!conversion.IsTryCast || conversion.Conversion.IsImplicit);

    private static bool IsNonNullableValueType(ITypeSymbol? type) =>
        type is { IsValueType: true }
            and not INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T };

    private static bool IsNonNullBoxing(IOperation value) =>
        value is IConversionOperation { Conversion.IsUserDefined: false, Type.IsReferenceType: true } conversion
            && (!conversion.IsTryCast || conversion.Conversion.IsImplicit)
            && IsNonNullableValueType(conversion.Operand.Type);

    private bool HasOnlyNonNullDefinitions(IOperation root, IOperation value, ILocalSymbol localSymbol,
        HashSet<ILocalSymbol> visited) =>
        !HasUntrackedWrites(root, localSymbol, value)
            && TryGetReachingLocalValues(root, value, localSymbol, out var values, out var incoming,
                analyzeReceiverTypes: false)
            && !incoming && !values.IsEmpty && values.All(candidate => IsKnownNonNull(candidate, visited));

    private static bool HasUntrackedWrites(IOperation root, ILocalSymbol local, IOperation use)
    {
        var storage = new LocalDefinitionTransfer(root, local);
        return root.DescendantsAndSelf().Any(operation => operation switch
        {
            IArgumentOperation { Parameter.RefKind: RefKind.Ref or RefKind.Out } argument =>
                storage.ReferencesStorage(argument.Value),
            IVariableDeclaratorOperation { Symbol.RefKind: not RefKind.None, Initializer: { } initializer } =>
                storage.ReferencesStorage(initializer.Value),
            ISimpleAssignmentOperation { IsRef: true } assignment => storage.ReferencesStorage(assignment.Value),
            IIncrementOrDecrementOperation increment => storage.ReferencesStorage(increment.Target),
            IAssignmentOperation assignment when storage.ReferencesStorage(assignment.Target) =>
                assignment is not ISimpleAssignmentOperation { Target: ILocalReferenceOperation }
                || GetEnclosingCallback(assignment) != GetEnclosingCallback(use),
            _ => false,
        });
    }

    private static TextSpan? GetEnclosingCallback(IOperation operation) =>
        operation.Syntax.Ancestors().FirstOrDefault(static syntax => syntax is
            AnonymousFunctionExpressionSyntax or LocalFunctionStatementSyntax)?.Span;

    private void Follow(ControlFlowBranch? branch, HashSet<TextSpan> state, ImmutableArray<int> continuation,
        FilterContext? filter)
    {
        if (state.Count == 0 || branch is null || !IsFeasible(branch))
        {
            return;
        }

        if (branch.Semantics == ControlFlowBranchSemantics.Return)
        {
            if (completingReturn)
            {
                // A handler returning another value abandons the selected return.
                return;
            }

            CompleteSelectedReturn(branch, state, continuation, filter);
        }

        if (FollowExceptionalBranch(branch, state, continuation, filter))
        {
            return;
        }

        if (branch.Destination is { } destination)
        {
            FollowDestination(branch, destination, state, continuation, filter);
        }
    }

    private bool FollowExceptionalBranch(ControlFlowBranch branch, HashSet<TextSpan> state,
        ImmutableArray<int> continuation, FilterContext? filter)
    {
        if (branch.Semantics is ControlFlowBranchSemantics.Throw or ControlFlowBranchSemantics.Rethrow)
        {
            var exceptionType = branch.Semantics == ControlFlowBranchSemantics.Rethrow ? GetActiveExceptionType(branch.Source) : GetThrownType(branch.Source.BranchValue);
            FollowException(branch.Source, exceptionType, state, continuation, filter);
            return true;
        }

        if (branch.Semantics == ControlFlowBranchSemantics.StructuredExceptionHandling)
        {
            FollowStructuredExceptionHandling(state, continuation, filter);
            return true;
        }

        return false;
    }

    private void CompleteSelectedReturn(ControlFlowBranch branch, HashSet<TextSpan> state,
        ImmutableArray<int> continuation, FilterContext? filter)
    {
        if (invocation is not IReturnOperation { ReturnedValue: { } returnedValue }
            || branch.Source.BranchValue?.Syntax.Span != returnedValue.Syntax.Span
            || branch.Destination is not { } destination)
        {
            return;
        }

        var completion = new ReachingDefinitionFlow(graph, invocation, transfer,
            analyzeReceiverTypes, completingReturn: true, operationRoot: _operationRoot)
        {
            _activeExceptions = _activeExceptions,
        };
        completion.FollowDestination(branch, destination, state, continuation, filter);
        completion.AnalyzePendingBlocks();
        _reaching.UnionWith(completion._reaching);
    }

    private void FollowStructuredExceptionHandling(HashSet<TextSpan> state, ImmutableArray<int> continuation,
        FilterContext? filter)
    {
        if (filter is not null)
        {
            RejectFilter(filter, state);
        }
        else if (!continuation.IsEmpty)
        {
            Enqueue(continuation[0], continuation.RemoveAt(0), state);
        }
    }

    private static ITypeSymbol? GetThrownType(IOperation? thrown)
    {
        while (thrown is IConversionOperation conversion)
        {
            thrown = conversion.Operand;
        }

        return thrown is IObjectCreationOperation ? thrown.Type : null;
    }

    private void FollowDestination(
        ControlFlowBranch branch,
        BasicBlock destination,
        HashSet<TextSpan> state,
        ImmutableArray<int> continuation,
        FilterContext? filter)
    {
        var remaining = branch.Semantics == ControlFlowBranchSemantics.Return ? [] : continuation;
        if (filter is not null && IsInRegion(destination, ControlFlowRegionKind.Catch))
        {
            // Filters run during exception search, before intervening finally blocks unwind.
            EnqueueHandler(filter.Source, destination.Ordinal, filter.Cleanup, filter.Continuation, state,
                filter.ExceptionType);
            return;
        }

        if (branch.FinallyRegions.IsEmpty)
        {
            Enqueue(destination.Ordinal, remaining, state, filter);
            return;
        }

        var cleanup = branch.FinallyRegions.Select(static region => region.FirstBlockOrdinal)
            .Append(destination.Ordinal).Concat(remaining).ToImmutableArray();
        Enqueue(cleanup[0], cleanup.RemoveAt(0), state, filter);
    }

    private void FollowException(BasicBlock source, ITypeSymbol? exceptionType, HashSet<TextSpan> state,
        ImmutableArray<int> continuation, FilterContext? filter = null)
    {
        // An exception in a filter rejects that filter; it does not replace the original exception.
        if (filter is not null)
        {
            RejectFilter(filter, state);
            return;
        }

        SearchException(source, exceptionType, state, continuation);
    }

    private void RejectFilter(FilterContext filter, HashSet<TextSpan> state) =>
        SearchException(filter.Source, filter.ExceptionType, state, filter.Continuation, filter.Handler);

    private void SearchException(BasicBlock source, ITypeSymbol? exceptionType, HashSet<TextSpan> state,
        ImmutableArray<int> continuation, ControlFlowRegion? rejectedHandler = null)
    {
        var cleanup = ImmutableArray.CreateBuilder<int>();
        for (var region = source.EnclosingRegion; region is not null && region != graph.Root.EnclosingRegion;
             region = region.EnclosingRegion)
        {
            if (region.Kind != ControlFlowRegionKind.Try || region.EnclosingRegion is not { } group)
            {
                continue;
            }

            if (group.Kind == ControlFlowRegionKind.TryAndFinally)
            {
                cleanup.Add(group.NestedRegions.First(static nested => nested.Kind == ControlFlowRegionKind.Finally).FirstBlockOrdinal);
                continue;
            }

            if (SearchHandlers(source, exceptionType, group, cleanup.ToImmutable(), continuation, state,
                    ref rejectedHandler))
            {
                return;
            }
        }

        FollowEscapingCleanup(state, cleanup.ToImmutable());
    }

    private bool SearchHandlers(BasicBlock source, ITypeSymbol? exceptionType, ControlFlowRegion group,
        ImmutableArray<int> cleanup, ImmutableArray<int> continuation, HashSet<TextSpan> state, ref ControlFlowRegion? rejectedHandler)
    {
        foreach (var handler in GetHandlers(group))
        {
            if (rejectedHandler is not null)
            {
                if (handler == rejectedHandler)
                {
                    rejectedHandler = null;
                }

                continue;
            }

            if (TryHandler(source, exceptionType, handler, cleanup, continuation, state))
            {
                return true;
            }
        }

        return false;
    }

    private void FollowEscapingCleanup(HashSet<TextSpan> state,
        ImmutableArray<int> cleanup)
    {
        if (!cleanup.IsEmpty)
        {
            Enqueue(cleanup[0], cleanup.RemoveAt(0), state);
        }
    }

    private bool TryHandler(BasicBlock source, ITypeSymbol? exceptionType, ControlFlowRegion handler,
        ImmutableArray<int> cleanup, ImmutableArray<int> continuation, HashSet<TextSpan> state)
    {
        var caught = handler.Kind == ControlFlowRegionKind.FilterAndHandler
            ? handler.NestedRegions.First(static nested => nested.Kind == ControlFlowRegionKind.Catch).ExceptionType
            : handler.ExceptionType;
        if (!CanCatch(caught, exceptionType))
        {
            return false;
        }

        if (handler.Kind == ControlFlowRegionKind.FilterAndHandler)
        {
            var filter = new FilterContext(source, exceptionType, handler, cleanup, continuation);
            Enqueue(handler.FirstBlockOrdinal, [], state, filter);
        }
        else
        {
            EnqueueHandler(source, handler.FirstBlockOrdinal, cleanup, continuation, state, exceptionType);
        }

        // An unknown exception may skip a typed handler, but never an unfiltered catch-all.
        // A matching filter continues the search only from its rejected branch.
        return exceptionType is not null || caught is null
            || caught.SpecialType == SpecialType.System_Object || caught.ToDisplayString() == "System.Exception";
    }

    private void EnqueueHandler(BasicBlock source, int handler, ImmutableArray<int> cleanup,
        ImmutableArray<int> continuation, HashSet<TextSpan> state, ITypeSymbol? exceptionType)
    {
        var remaining = GetHandlerContinuation(source, graph.Blocks[handler], continuation);
        var path = cleanup.Add(handler).AddRange(remaining);
        // Keep each catch's exception separately: a nested handler must not replace an outer rethrow.
        var catchRegion = graph.Blocks[handler].EnclosingRegion;
        while (catchRegion.Kind != ControlFlowRegionKind.Catch && catchRegion.EnclosingRegion is { } parent)
        {
            catchRegion = parent;
        }

        var exceptions = _activeExceptions.SetItem(catchRegion.FirstBlockOrdinal, exceptionType);
        Enqueue(path[0], path.RemoveAt(0), state, exceptions: exceptions);
    }

    private ITypeSymbol? GetActiveExceptionType(BasicBlock source)
    {
        for (var region = source.EnclosingRegion; region is not null; region = region.EnclosingRegion)
        {
            if (region.Kind == ControlFlowRegionKind.Catch)
            {
                return _activeExceptions.TryGetValue(region.FirstBlockOrdinal, out var type) ? type : null;
            }
        }

        return null;
    }

    private ImmutableArray<int> GetHandlerContinuation(BasicBlock source, BasicBlock handler,
        ImmutableArray<int> continuation)
    {
        var handlerFinally = GetEnclosingFinally(handler);
        if (handlerFinally is null)
        {
            return [];
        }

        if (GetEnclosingFinally(source) == handlerFinally)
        {
            return continuation;
        }

        // A handler in an outer finally abandons destinations inside that finally,
        // but the outer finally must still resume its own pending destination.
        return [
            .. continuation.SkipWhile(ordinal => ordinal >= handlerFinally.FirstBlockOrdinal
            && ordinal <= handlerFinally.LastBlockOrdinal)
        ];
    }

    private ControlFlowRegion? GetEnclosingFinally(BasicBlock block)
    {
        for (var region = block.EnclosingRegion; region is not null && region != graph.Root.EnclosingRegion;
             region = region.EnclosingRegion)
        {
            if (region.Kind == ControlFlowRegionKind.Finally)
            {
                return region;
            }
        }

        return null;
    }

    private static IEnumerable<ControlFlowRegion> GetHandlers(ControlFlowRegion group) =>
        group.NestedRegions.Where(static nested => nested.Kind is
            ControlFlowRegionKind.Catch or ControlFlowRegionKind.FilterAndHandler);

    private static string Key(int block, ImmutableArray<int> continuation, FilterContext? filter,
        ImmutableDictionary<int, ITypeSymbol?> exceptions) =>
        block + ":" + string.Join(",", continuation) + ":" + filter?.Key + ":"
            + string.Join(";", exceptions.OrderBy(static pair => pair.Key)
                .Select(static pair => pair.Key + "=" + pair.Value?.ToDisplayString()));

    private sealed class FilterContext(BasicBlock source, ITypeSymbol? exceptionType, ControlFlowRegion handler,
        ImmutableArray<int> cleanup, ImmutableArray<int> continuation)
    {
        public BasicBlock Source { get; } = source;

        public ITypeSymbol? ExceptionType { get; } = exceptionType;

        public ControlFlowRegion Handler { get; } = handler;

        public ImmutableArray<int> Cleanup { get; } = cleanup;

        public ImmutableArray<int> Continuation { get; } = continuation;

        public string Key { get; } = source.Ordinal + ":" + handler.FirstBlockOrdinal + ":"
            + exceptionType?.ToDisplayString() + ":" + string.Join(",", continuation);
    }

    private bool IsInRegion(BasicBlock block, ControlFlowRegionKind kind)
    {
        // A nested handler takes precedence over any outer handler's region.
        for (var region = block.EnclosingRegion; region is not null && region != graph.Root.EnclosingRegion;
             region = region.EnclosingRegion)
        {
            if (region.Kind is ControlFlowRegionKind.Filter or ControlFlowRegionKind.Catch or ControlFlowRegionKind.Finally)
            {
                return region.Kind == kind;
            }
        }

        return false;
    }

    private static bool CanCatch(ITypeSymbol? caught, ITypeSymbol? thrown)
    {
        if (caught is null || thrown is null)
        {
            return true;
        }

        for (var type = thrown; type is not null; type = type.BaseType)
        {
            if (SymbolEqualityComparer.Default.Equals(type, caught))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsFeasible(ControlFlowBranch branch)
    {
        var source = branch.Source;
        return source.ConditionKind == ControlFlowConditionKind.None
            || source.BranchValue?.ConstantValue is not { HasValue: true, Value: bool value }
            || branch.IsConditionalSuccessor == (value == (source.ConditionKind == ControlFlowConditionKind.WhenTrue));
    }

    private static IOperation GetOperationRoot(IOperation operation)
    {
        while (operation.Parent is { } parent)
        {
            operation = parent;
        }

        return operation;
    }

    private static ControlFlowGraph? CreateGraph(IOperation root) => root switch
    {
        IMethodBodyOperation method => ControlFlowGraph.Create(method),
        IConstructorBodyOperation constructor => ControlFlowGraph.Create(constructor),
        IBlockOperation block => ControlFlowGraph.Create(block),
        IFieldInitializerOperation field => ControlFlowGraph.Create(field),
        IPropertyInitializerOperation property => ControlFlowGraph.Create(property),
        _ => null,
    };

    private static ControlFlowGraph? FindGraph(ControlFlowGraph graph, TextSpan invocation)
    {
        var operations = graph.Blocks.SelectMany(static block => block.Operations
                .Concat(block.BranchValue is { } value ? [value] : []))
            .SelectMany(static operation => operation.DescendantsAndSelf()).ToArray();
        if (operations.Any(operation => operation.Syntax.Span == invocation))
        {
            return graph;
        }

        foreach (var local in graph.LocalFunctions)
        {
            if (FindGraph(graph.GetLocalFunctionControlFlowGraph(local), invocation) is { } nested)
            {
                return nested;
            }
        }

        foreach (var anonymous in operations.OfType<IFlowAnonymousFunctionOperation>())
        {
            if (FindGraph(graph.GetAnonymousFunctionControlFlowGraph(anonymous), invocation) is { } nested)
            {
                return nested;
            }
        }

        return null;
    }
}
