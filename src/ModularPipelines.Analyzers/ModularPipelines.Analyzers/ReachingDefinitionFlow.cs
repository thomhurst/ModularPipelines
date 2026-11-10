using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace ModularPipelines.Analyzers;

internal sealed class ReachingDefinitionFlow(
    ControlFlowGraph graph,
    IOperation invocation,
    IOperation? command,
    Dictionary<TextSpan, IOperation> definitions,
    ILocalSymbol? local = null,
    bool analyzeFieldReceivers = true)
{
    // A path without a local definition still reads the capture at the enclosing execution site.
    private static readonly TextSpan IncomingValue = new(int.MaxValue, 0);
    private readonly HashSet<TextSpan> _reaching = [];
    private readonly Dictionary<string, HashSet<TextSpan>> _inputs = [];
    private readonly Queue<(int Block, ImmutableArray<int> Continuation, FilterContext? Filter)> _queue = [];

    public static IEnumerable<IInvocationOperation> GetReachingSetters(
        IOperation root,
        IInvocationOperation invocation,
        IOperation command,
        Dictionary<TextSpan, IInvocationOperation> setters)
    {
        if (setters.Count == 0 || CreateGraph(root) is not { } rootGraph
            || FindGraph(rootGraph, invocation.Syntax.Span) is not { } graph)
        {
            return [];
        }

        return new ReachingDefinitionFlow(graph, invocation, command,
            setters.ToDictionary(static pair => pair.Key, static pair => (IOperation) pair.Value))
            .Analyze().Cast<IInvocationOperation>();
    }

    public static bool TryGetReachingLocalValues(IOperation root, IOperation operation, ILocalSymbol local,
        out ImmutableArray<IOperation> values, out bool inheritsIncomingValue, bool analyzeFieldReceivers = true)
    {
        values = [];
        inheritsIncomingValue = false;
        if (CreateGraph(root) is not { } rootGraph
            || FindGraph(rootGraph, operation.Syntax.Span) is not { } graph)
        {
            return false;
        }

        var definitions = root.DescendantsAndSelf().Select(candidate => candidate switch
        {
            IVariableDeclaratorOperation { Initializer: { } initializer } declarator
                when SymbolEqualityComparer.Default.Equals(declarator.Symbol, local) => initializer.Value,
            ISimpleAssignmentOperation { Target: ILocalReferenceOperation target } assignment
                when SymbolEqualityComparer.Default.Equals(target.Local, local) => assignment.Value,
            _ => null,
        }).OfType<IOperation>().GroupBy(static value => value.Syntax.Span)
            .ToDictionary(static group => group.Key, static group => group.First());
        var flow = new ReachingDefinitionFlow(graph, operation, null, definitions, local, analyzeFieldReceivers);
        values = [.. flow.Analyze()];
        inheritsIncomingValue = flow._reaching.Contains(IncomingValue);
        return true;
    }

    private IEnumerable<IOperation> Analyze()
    {
        Enqueue(0, [], local is null ? [] : [IncomingValue]);
        while (_queue.Count > 0)
        {
            var (ordinal, continuation, filter) = _queue.Dequeue();
            var block = graph.Blocks[ordinal];
            var state = new HashSet<TextSpan>(_inputs[Key(ordinal, continuation, filter)]);
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

        return _reaching.Where(span => span != IncomingValue).Select(span => definitions[span]);
    }

    private void Enqueue(int ordinal, ImmutableArray<int> continuation, HashSet<TextSpan> state, FilterContext? filter = null)
    {
        var key = Key(ordinal, continuation, filter);
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

        _queue.Enqueue((ordinal, continuation, filter));
    }

    private void Visit(IOperation operation, HashSet<TextSpan> state, BasicBlock block,
        ImmutableArray<int> continuation, FilterContext? filter)
    {
        // Delegate bodies have their own graph and execute only when invoked.
        if (operation is IFlowAnonymousFunctionOperation)
        {
            return;
        }

        foreach (var child in operation.ChildOperations)
        {
            Visit(child, state, block, continuation, filter);
        }

        if (TryApplyCommandSetter(operation, state))
        {
            return;
        }

        if (operation.Syntax.Span == invocation.Syntax.Span)
        {
            _reaching.UnionWith(state);
        }

        var throwsNullReference = IsThrowingFieldAccess(operation);
        if (throwsNullReference || CanThrow(operation, block))
        {
            var exceptionType = throwsNullReference
                ? invocation.SemanticModel?.Compilation.GetTypeByMetadataName("System.NullReferenceException")
                : null;
            FollowException(block, exceptionType, state, continuation, filter);
        }

        ApplyDefinition(operation, state);
    }

    private bool TryApplyCommandSetter(IOperation operation, HashSet<TextSpan> state)
    {
        if (local is not null || operation is not IInvocationOperation call || !definitions.ContainsKey(call.Syntax.Span))
        {
            return false;
        }

        state.Clear();
        state.Add(call.Syntax.Span);
        return true;
    }

    private void ApplyDefinition(IOperation operation, HashSet<TextSpan> state)
    {
        if (local is not null && operation is ISimpleAssignmentOperation { Target: ILocalReferenceOperation target } assignment
            && SymbolEqualityComparer.Default.Equals(target.Local, local)
            && definitions.ContainsKey(assignment.Value.Syntax.Span))
        {
            state.Clear();
            state.Add(assignment.Value.Syntax.Span);
        }

        if (command is not null && operation is IObjectCreationOperation && operation.Syntax.Span == command.Syntax.Span)
        {
            // Re-executing an allocation creates a fresh command, including on loop back-edges.
            // Constructor exceptions retain the previous instance's state on their exception path.
            state.Clear();
        }
    }

    private static bool CanThrow(IOperation operation, BasicBlock block)
    {
        if (IsThrowingAccessOrCall(operation))
        {
            return true;
        }

        return operation switch
        {
            // The explicit throw edge below carries the constructed exception's type.
            IObjectCreationOperation => block.FallThroughSuccessor?.Semantics != ControlFlowBranchSemantics.Throw
                || block.BranchValue?.Syntax.Span != operation.Syntax.Span,
            IConversionOperation conversion => CanConversionThrow(conversion),
            IBinaryOperation binary => CanBinaryThrow(binary),
            IUnaryOperation unary => unary.IsChecked || unary.OperatorMethod is not null,
            _ => false,
        };
    }

    private static bool IsThrowingAccessOrCall(IOperation operation) => operation is
        IInvocationOperation or IPropertyReferenceOperation or IArrayElementReferenceOperation
        or IArrayCreationOperation or IAwaitOperation or IDynamicInvocationOperation
        or IDynamicIndexerAccessOperation or IDynamicMemberReferenceOperation;

    private bool IsThrowingFieldAccess(IOperation operation) => operation switch
    {
        // A simple field write checks the receiver after evaluating the assigned value.
        ISimpleAssignmentOperation { Target: IFieldReferenceOperation field } => HasNullableFieldReceiver(field),
        IFieldReferenceOperation field => HasNullableFieldReceiver(field)
            && (field.Parent is not ISimpleAssignmentOperation assignment || !ReferenceEquals(assignment.Target, field)),
        _ => false,
    };

    private bool HasNullableFieldReceiver(IFieldReferenceOperation field) =>
        field.Instance is { Type.IsReferenceType: true } and not IInstanceReferenceOperation
        && (!analyzeFieldReceivers || !IsKnownNonNull(field.Instance, [with(SymbolEqualityComparer.Default)]));

    private bool IsKnownNonNull(IOperation value, HashSet<ILocalSymbol> visited)
    {
        while (value is IConversionOperation { OperatorMethod: null } conversion)
        {
            value = conversion.Operand;
        }

        if (value is IObjectCreationOperation or IInstanceReferenceOperation)
        {
            return true;
        }

        if (value is not ILocalReferenceOperation reference || !visited.Add(reference.Local))
        {
            return false;
        }

        var root = invocation;
        while (root.Parent is { } parent)
        {
            root = parent;
        }

        // Reuse reaching definitions without recursively proving field receivers in that analysis.
        var known = !HasUntrackedWrites(root, reference.Local)
            && TryGetReachingLocalValues(root, value, reference.Local, out var values, out var incoming,
                analyzeFieldReceivers: false)
            && !incoming && !values.IsEmpty && values.All(candidate => IsKnownNonNull(candidate, visited));
        visited.Remove(reference.Local);
        return known;
    }

    private static bool HasUntrackedWrites(IOperation root, ILocalSymbol local) =>
        root.DescendantsAndSelf().Any(operation => operation switch
        {
            IArgumentOperation { Parameter.RefKind: RefKind.Ref or RefKind.Out } argument =>
                ReferencesLocal(argument.Value, local),
            IVariableDeclaratorOperation { Symbol.RefKind: not RefKind.None, Initializer: { } initializer } =>
                ReferencesLocal(initializer.Value, local),
            IIncrementOrDecrementOperation increment => TargetsLocal(increment.Target, local),
            IAssignmentOperation assignment when TargetsLocal(assignment.Target, local) =>
                assignment is not ISimpleAssignmentOperation { Target: ILocalReferenceOperation }
                || assignment.Syntax.Ancestors().Any(static syntax => syntax is
                    AnonymousFunctionExpressionSyntax or LocalFunctionStatementSyntax),
            _ => false,
        });

    private static bool TargetsLocal(IOperation target, ILocalSymbol local) => target switch
    {
        ILocalReferenceOperation reference => SymbolEqualityComparer.Default.Equals(reference.Local, local),
        ITupleOperation tuple => tuple.Elements.Any(element => TargetsLocal(element, local)),
        _ => false,
    };

    private static bool ReferencesLocal(IOperation operation, ILocalSymbol local) =>
        operation.DescendantsAndSelf().OfType<ILocalReferenceOperation>()
            .Any(reference => SymbolEqualityComparer.Default.Equals(reference.Local, local));

    private static bool CanConversionThrow(IConversionOperation conversion) =>
        conversion.IsChecked || conversion.Conversion.IsUserDefined
        || (!conversion.IsTryCast && !conversion.Conversion.IsImplicit
            && !conversion.Conversion.IsNumeric && !conversion.Conversion.IsIdentity);

    private static bool CanBinaryThrow(IBinaryOperation binary)
    {
        if (binary.OperatorMethod is not null)
        {
            return true;
        }

        var operandType = binary.LeftOperand.Type;
        if (operandType is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } nullable)
        {
            operandType = nullable.TypeArguments[0];
        }

        return operandType?.SpecialType is not (SpecialType.System_Single or SpecialType.System_Double)
            && (binary.IsChecked || binary.OperatorKind is BinaryOperatorKind.Divide or BinaryOperatorKind.Remainder);
    }

    private void Follow(ControlFlowBranch? branch, HashSet<TextSpan> state, ImmutableArray<int> continuation,
        FilterContext? filter)
    {
        if (branch is null || !IsFeasible(branch))
        {
            return;
        }

        if (branch.Semantics is ControlFlowBranchSemantics.Throw or ControlFlowBranchSemantics.Rethrow)
        {
            FollowException(branch.Source, GetThrownType(branch.Source.BranchValue), state, continuation, filter);
            return;
        }

        if (branch.Semantics == ControlFlowBranchSemantics.StructuredExceptionHandling)
        {
            FollowStructuredExceptionHandling(state, continuation, filter);
            return;
        }

        if (branch.Destination is not { } destination)
        {
            return;
        }

        FollowDestination(branch, destination, state, continuation, filter);
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
            EnqueueHandler(filter.Source, destination.Ordinal, filter.Cleanup, filter.Continuation, state);
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
        var skipping = rejectedHandler is not null;
        for (var region = source.EnclosingRegion; region is not null; region = region.EnclosingRegion)
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

            foreach (var handler in GetHandlers(group))
            {
                if (skipping)
                {
                    skipping = handler != rejectedHandler;
                    continue;
                }

                if (TryHandler(source, exceptionType, handler, cleanup.ToImmutable(), continuation, state))
                {
                    return;
                }
            }
        }

        if (cleanup.Count > 0)
        {
            var path = cleanup.ToImmutable();
            Enqueue(path[0], path.RemoveAt(0), state);
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
            EnqueueHandler(source, handler.FirstBlockOrdinal, cleanup, continuation, state);
        }

        // An unknown exception may skip a typed handler, but never an unfiltered catch-all.
        // A matching filter continues the search only from its rejected branch.
        return exceptionType is not null || caught is null
            || caught.SpecialType == SpecialType.System_Object || caught.ToDisplayString() == "System.Exception";
    }

    private void EnqueueHandler(BasicBlock source, int handler, ImmutableArray<int> cleanup,
        ImmutableArray<int> continuation, HashSet<TextSpan> state)
    {
        var remaining = GetHandlerContinuation(source, graph.Blocks[handler], continuation);
        var path = cleanup.Add(handler).AddRange(remaining);
        Enqueue(path[0], path.RemoveAt(0), state);
    }

    private static ImmutableArray<int> GetHandlerContinuation(BasicBlock source, BasicBlock handler,
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
        return [.. continuation.SkipWhile(ordinal => ordinal >= handlerFinally.FirstBlockOrdinal
            && ordinal <= handlerFinally.LastBlockOrdinal)];
    }

    private static ControlFlowRegion? GetEnclosingFinally(BasicBlock block)
    {
        for (var region = block.EnclosingRegion; region is not null; region = region.EnclosingRegion)
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

    private static string Key(int block, ImmutableArray<int> continuation, FilterContext? filter = null) =>
        block + ":" + string.Join(",", continuation) + ":" + filter?.Key;

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

    private static bool IsInRegion(BasicBlock block, ControlFlowRegionKind kind)
    {
        // A nested handler takes precedence over any outer handler's region.
        for (var region = block.EnclosingRegion; region is not null; region = region.EnclosingRegion)
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
