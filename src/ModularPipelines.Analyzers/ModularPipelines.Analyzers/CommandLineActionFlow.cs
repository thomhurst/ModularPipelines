using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace ModularPipelines.Analyzers;

internal sealed class CommandLineActionFlow(
    ControlFlowGraph graph,
    IInvocationOperation invocation,
    Dictionary<TextSpan, IInvocationOperation> setters)
{
    private readonly HashSet<TextSpan> _reaching = [];
    private readonly Dictionary<string, HashSet<TextSpan>> _inputs = [];
    private readonly Queue<(int Block, ImmutableArray<int> Continuation)> _queue = [];

    public static IEnumerable<IInvocationOperation> GetReachingSetters(
        IOperation root,
        IInvocationOperation invocation,
        Dictionary<TextSpan, IInvocationOperation> setters)
    {
        if (setters.Count == 0 || CreateGraph(root) is not { } rootGraph
            || FindGraph(rootGraph, invocation.Syntax.Span) is not { } graph)
        {
            return [];
        }

        return new CommandLineActionFlow(graph, invocation, setters).Analyze();
    }

    private IEnumerable<IInvocationOperation> Analyze()
    {
        Enqueue(0, [], []);
        while (_queue.Count > 0)
        {
            var (ordinal, continuation) = _queue.Dequeue();
            var block = graph.Blocks[ordinal];
            var state = new HashSet<TextSpan>(_inputs[Key(ordinal, continuation)]);
            foreach (var operation in block.Operations)
            {
                Visit(operation, state, block);
            }

            if (block.BranchValue is { } branchValue)
            {
                Visit(branchValue, state, block);
            }

            Follow(block.FallThroughSuccessor, state, continuation);
            Follow(block.ConditionalSuccessor, state, continuation);
        }

        return _reaching.Select(span => setters[span]);
    }

    private void Enqueue(int ordinal, ImmutableArray<int> continuation, HashSet<TextSpan> state)
    {
        var key = Key(ordinal, continuation);
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

        _queue.Enqueue((ordinal, continuation));
    }

    private void Visit(IOperation operation, HashSet<TextSpan> state, BasicBlock block)
    {
        // Delegate bodies have their own graph and execute only when invoked.
        if (operation is IFlowAnonymousFunctionOperation)
        {
            return;
        }

        foreach (var child in operation.ChildOperations)
        {
            Visit(child, state, block);
        }

        if (operation is not IInvocationOperation call)
        {
            return;
        }

        if (setters.ContainsKey(call.Syntax.Span))
        {
            state.Clear();
            state.Add(call.Syntax.Span);
        }
        else
        {
            if (call.Syntax.Span == invocation.Syntax.Span)
            {
                _reaching.UnionWith(state);
            }

            // Calls can throw before the following statement is reached.
            FollowException(block, null, state);
        }
    }

    private void Follow(ControlFlowBranch? branch, HashSet<TextSpan> state, ImmutableArray<int> continuation)
    {
        if (branch is null || !IsFeasible(branch))
        {
            return;
        }

        if (branch.Semantics is ControlFlowBranchSemantics.Throw or ControlFlowBranchSemantics.Rethrow)
        {
            var thrown = branch.Source.BranchValue;
            while (thrown is IConversionOperation conversion)
            {
                thrown = conversion.Operand;
            }

            FollowException(branch.Source, thrown is IObjectCreationOperation ? thrown.Type : null, state);
            return;
        }

        if (branch.Semantics == ControlFlowBranchSemantics.StructuredExceptionHandling)
        {
            if (!continuation.IsEmpty && !IsInRegion(branch.Source, ControlFlowRegionKind.Filter))
            {
                Enqueue(continuation[0], continuation.RemoveAt(0), state);
            }

            return;
        }

        if (branch.Destination is not { } destination)
        {
            return;
        }

        FollowDestination(branch, destination, state, continuation);
    }

    private void FollowDestination(
        ControlFlowBranch branch,
        BasicBlock destination,
        HashSet<TextSpan> state,
        ImmutableArray<int> continuation)
    {
        var remaining = branch.Semantics == ControlFlowBranchSemantics.Return ? [] : continuation;
        if (!remaining.IsEmpty && IsInRegion(branch.Source, ControlFlowRegionKind.Filter)
            && IsInRegion(destination, ControlFlowRegionKind.Catch))
        {
            // Filters run during exception search, before intervening finally blocks unwind.
            Enqueue(remaining[0], remaining.RemoveAt(0).Add(destination.Ordinal), state);
            return;
        }

        if (branch.FinallyRegions.IsEmpty)
        {
            Enqueue(destination.Ordinal, remaining, state);
            return;
        }

        var cleanup = branch.FinallyRegions.Select(static region => region.FirstBlockOrdinal)
            .Append(destination.Ordinal).Concat(remaining).ToImmutableArray();
        Enqueue(cleanup[0], cleanup.RemoveAt(0), state);
    }

    private void FollowException(BasicBlock source, ITypeSymbol? exceptionType, HashSet<TextSpan> state)
    {
        var cleanup = ImmutableArray.CreateBuilder<int>();
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

            foreach (var handler in group.NestedRegions.Where(static nested => nested.Kind is
                         ControlFlowRegionKind.Catch or ControlFlowRegionKind.FilterAndHandler))
            {
                var caught = handler.Kind == ControlFlowRegionKind.FilterAndHandler
                    ? handler.NestedRegions.First(static nested => nested.Kind == ControlFlowRegionKind.Catch).ExceptionType
                    : handler.ExceptionType;
                if (!CanCatch(caught, exceptionType))
                {
                    continue;
                }

                if (handler.Kind == ControlFlowRegionKind.FilterAndHandler)
                {
                    Enqueue(handler.FirstBlockOrdinal, cleanup.ToImmutable(), state);
                }
                else
                {
                    var path = cleanup.ToImmutable().Add(handler.FirstBlockOrdinal);
                    Enqueue(path[0], path.RemoveAt(0), state);
                }
                if (handler.Kind == ControlFlowRegionKind.Catch && exceptionType is not null)
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

    private static string Key(int block, ImmutableArray<int> continuation) =>
        block + ":" + string.Join(",", continuation);

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
        _ => null,
    };

    private static ControlFlowGraph? FindGraph(ControlFlowGraph graph, TextSpan invocation)
    {
        var operations = graph.Blocks.SelectMany(static block => block.Operations
                .Concat(block.BranchValue is { } value ? [value] : []))
            .SelectMany(static operation => operation.DescendantsAndSelf()).ToArray();
        if (operations.OfType<IInvocationOperation>().Any(call => call.Syntax.Span == invocation))
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
