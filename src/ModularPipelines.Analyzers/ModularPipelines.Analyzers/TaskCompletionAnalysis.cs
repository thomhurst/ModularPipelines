using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace ModularPipelines.Analyzers;

/// <summary>
/// Tracks definite completion of stable Task locals and parameters within one callable.
/// Mutable members and captured/ref variables cannot identify the same task reliably.
/// ValueTask is deliberately excluded: awaiting it need not permit a second consumption.
/// </summary>
internal static class TaskCompletionAnalysis
{
    private static readonly ConditionalWeakTable<IOperation, ImmutableHashSet<TextSpan>> CompletedResults = new();

    public static bool IsCompleted(IPropertyReferenceOperation result, CancellationToken cancellationToken)
    {
        var root = (IOperation) result;
        while (root.Parent is not null)
        {
            root = root.Parent;
        }

        return CompletedResults.GetValue(root, operation => Analyze(operation, cancellationToken))
            .Contains(result.Syntax.Span);
    }

    private static ImmutableHashSet<TextSpan> Analyze(IOperation root, CancellationToken cancellationToken)
    {
        var graph = root switch
        {
            IMethodBodyOperation methodBody => ControlFlowGraph.Create(methodBody, cancellationToken),
            IConstructorBodyOperation constructorBody => ControlFlowGraph.Create(constructorBody, cancellationToken),
            IBlockOperation block => ControlFlowGraph.Create(block, cancellationToken),
            _ => null,
        };
        if (graph is null)
        {
            return [];
        }

        var unstable = GetUnstableSymbols(root);
        var completed = ImmutableHashSet.CreateBuilder<TextSpan>();
        AnalyzeGraph(graph, unstable, completed, cancellationToken);
        return completed.ToImmutable();
    }

    private static HashSet<ISymbol> GetUnstableSymbols(IOperation root)
    {
        var unstable = new HashSet<ISymbol>(SymbolEqualityComparer.Default);
        foreach (var operation in root.DescendantsAndSelf())
        {
            if (GetSymbol(operation) is { } referencedSymbol
                && GetEnclosingFunction(operation) is { } function
                && !SymbolEqualityComparer.Default.Equals(referencedSymbol.ContainingSymbol, function))
            {
                unstable.Add(referencedSymbol);
            }

            if (CanAliasSymbol(operation))
            {
                AddReferencedSymbols(operation, unstable);
            }
        }

        return unstable;
    }

    private static bool CanAliasSymbol(IOperation operation) => operation is
        IArgumentOperation { Parameter.RefKind: not RefKind.None }
        or ISimpleAssignmentOperation { IsRef: true }
        or IVariableDeclaratorOperation { Symbol.RefKind: not RefKind.None }
        // Dynamically bound arguments have no statically bound parameter.
        || operation.Syntax.Parent is ArgumentSyntax { RefKindKeyword.RawKind: not 0 };

    private static void AddReferencedSymbols(IOperation operation, HashSet<ISymbol> symbols)
    {
        foreach (var reference in operation.DescendantsAndSelf())
        {
            if (GetSymbol(reference) is { } symbol)
            {
                symbols.Add(symbol);
            }
        }
    }

    private static void AnalyzeGraph(
        ControlFlowGraph graph,
        HashSet<ISymbol> unstable,
        ImmutableHashSet<TextSpan>.Builder completed,
        CancellationToken cancellationToken)
    {
        var regionWrites = GetRegionWrites(graph.Root, graph);
        var outputs = PropagateCompletion(graph, unstable, regionWrites, cancellationToken);
        foreach (var block in graph.Blocks.Where(static block => block.IsReachable))
        {
            var state = GetInput(graph, block, outputs, unstable, regionWrites);
            VisitBlock(block, state, unstable, completed);
        }

        foreach (var local in graph.LocalFunctions)
        {
            AnalyzeGraph(graph.GetLocalFunctionControlFlowGraph(local, cancellationToken), unstable, completed, cancellationToken);
        }

        foreach (var anonymous in GetAnonymousFunctions(graph))
        {
            AnalyzeGraph(graph.GetAnonymousFunctionControlFlowGraph(anonymous, cancellationToken), unstable, completed, cancellationToken);
        }
    }

    private static IEnumerable<IFlowAnonymousFunctionOperation> GetAnonymousFunctions(ControlFlowGraph graph) =>
        graph.Blocks.SelectMany(static block => block.Operations
                .Concat(block.BranchValue is { } branch ? [branch] : [])
                .SelectMany(static operation => operation.DescendantsAndSelf()))
            .OfType<IFlowAnonymousFunctionOperation>();

    private static HashSet<ISymbol>?[] PropagateCompletion(
        ControlFlowGraph graph,
        HashSet<ISymbol> unstable,
        Dictionary<ControlFlowRegion, HashSet<ISymbol>> regionWrites,
        CancellationToken cancellationToken)
    {
        var outputs = new HashSet<ISymbol>?[graph.Blocks.Length];
        bool changed;
        do
        {
            changed = false;
            foreach (var block in graph.Blocks)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (!block.IsReachable)
                {
                    continue;
                }

                var state = GetInput(graph, block, outputs, unstable, regionWrites);
                VisitBlock(block, state, unstable, null);
                if (outputs[block.Ordinal] is not { } previous || !previous.SetEquals(state))
                {
                    outputs[block.Ordinal] = state;
                    changed = true;
                }
            }
        }
        while (changed);

        return outputs;
    }

    private static HashSet<ISymbol> GetInput(
        ControlFlowGraph graph,
        BasicBlock block,
        HashSet<ISymbol>?[] outputs,
        HashSet<ISymbol> unstable,
        Dictionary<ControlFlowRegion, HashSet<ISymbol>> regionWrites)
    {
        // Include the implicit exceptional entry even when a handler starts with
        // a loop header that also has explicit back-edges.
        var input = GetHandlerInput(graph, block, outputs, unstable, regionWrites);
        foreach (var predecessor in block.Predecessors)
        {
            if (!predecessor.Source.IsReachable || outputs[predecessor.Source.Ordinal] is not { } output)
            {
                continue;
            }

            var edge = GetEdgeState(predecessor, output, unstable, regionWrites);
            if (input is null)
            {
                input = edge;
            }
            else
            {
                input.IntersectWith(edge);
            }
        }

        return input ?? new HashSet<ISymbol>(SymbolEqualityComparer.Default);
    }

    private static HashSet<ISymbol> GetEdgeState(
        ControlFlowBranch predecessor,
        HashSet<ISymbol> output,
        HashSet<ISymbol> unstable,
        Dictionary<ControlFlowRegion, HashSet<ISymbol>> regionWrites)
    {
        var edge = new HashSet<ISymbol>(output, SymbolEqualityComparer.Default);
        var source = predecessor.Source;
        if (source.BranchValue is { } condition && source.ConditionKind != ControlFlowConditionKind.None)
        {
            var conditionValue = predecessor.IsConditionalSuccessor
                == (source.ConditionKind == ControlFlowConditionKind.WhenTrue);
            AddGuardFacts(condition, conditionValue, edge, unstable);
        }

        // Finally bodies execute implicitly on these edges. Preserve only facts
        // about symbols they cannot write, regardless of the path through cleanup.
        foreach (var region in predecessor.FinallyRegions)
        {
            edge.ExceptWith(regionWrites[region]);
        }

        return edge;
    }

    private static Dictionary<ControlFlowRegion, HashSet<ISymbol>> GetRegionWrites(
        ControlFlowRegion region,
        ControlFlowGraph graph)
    {
        var writes = new Dictionary<ControlFlowRegion, HashSet<ISymbol>>();
        AddRegionWrites(region, graph, writes);
        return writes;
    }

    private static void AddRegionWrites(
        ControlFlowRegion region,
        ControlFlowGraph graph,
        Dictionary<ControlFlowRegion, HashSet<ISymbol>> writes)
    {
        if (region.Kind is ControlFlowRegionKind.TryAndCatch or ControlFlowRegionKind.TryAndFinally or ControlFlowRegionKind.Finally)
        {
            writes.Add(region, GetWrittenSymbols(graph.Blocks
                .Skip(region.FirstBlockOrdinal)
                .Take(region.LastBlockOrdinal - region.FirstBlockOrdinal + 1)));
        }

        foreach (var nested in region.NestedRegions)
        {
            AddRegionWrites(nested, graph, writes);
        }
    }

    private static HashSet<ISymbol> GetWrittenSymbols(IEnumerable<BasicBlock> blocks) => new(
        blocks.SelectMany(static block => block.Operations
                .Concat(block.BranchValue is { } branch ? [branch] : [])
                .SelectMany(static operation => operation.DescendantsAndSelf()))
            .OfType<IAssignmentOperation>()
            .SelectMany(static assignment => assignment.Target.DescendantsAndSelf())
            .Select(GetSymbol)
            .OfType<ISymbol>(),
        SymbolEqualityComparer.Default);

    private static HashSet<ISymbol>? GetHandlerInput(
        ControlFlowGraph graph,
        BasicBlock block,
        HashSet<ISymbol>?[] outputs,
        HashSet<ISymbol> unstable,
        Dictionary<ControlFlowRegion, HashSet<ISymbol>> regionWrites)
    {
        for (var region = block.EnclosingRegion; region is not null; region = region.EnclosingRegion)
        {
            if (block.Ordinal != region.FirstBlockOrdinal
                || region.Kind is not (ControlFlowRegionKind.Catch or ControlFlowRegionKind.Filter or ControlFlowRegionKind.Finally))
            {
                continue;
            }

            var group = region.EnclosingRegion!;
            if (group.Kind == ControlFlowRegionKind.FilterAndHandler)
            {
                group = group.EnclosingRegion!;
            }

            var protectedRegion = group.NestedRegions.First(static nested => nested.Kind == ControlFlowRegionKind.Try);
            var input = GetInput(graph, graph.Blocks[protectedRegion.FirstBlockOrdinal], outputs, unstable, regionWrites);
            // Only facts dominating the protected region can hold on every
            // exceptional entry. Any write, including in earlier filters or
            // nested cleanup, can change the task before this handler executes.
            input.ExceptWith(regionWrites[group]);
            return input;
        }

        return null;
    }

    private static void VisitBlock(
        BasicBlock block,
        HashSet<ISymbol> state,
        HashSet<ISymbol> unstable,
        ImmutableHashSet<TextSpan>.Builder? completed)
    {
        foreach (var operation in block.Operations)
        {
            Visit(operation, state, unstable, completed);
        }

        if (block.BranchValue is { } branch)
        {
            Visit(branch, state, unstable, completed);
        }
    }

    private static void Visit(
        IOperation operation,
        HashSet<ISymbol> state,
        HashSet<ISymbol> unstable,
        ImmutableHashSet<TextSpan>.Builder? completed)
    {
        // These bodies execute through their own graphs, not at declaration time.
        if (operation is IAnonymousFunctionOperation or ILocalFunctionOperation or IFlowAnonymousFunctionOperation)
        {
            return;
        }

        foreach (var child in operation.ChildOperations)
        {
            Visit(child, state, unstable, completed);
        }

        switch (operation)
        {
            case IPropertyReferenceOperation { Property.Name: "Result", Instance: { } instance } result
                when GetStableTaskSymbol(instance, unstable) is { } symbol && state.Contains(symbol):
                completed?.Add(result.Syntax.Span);
                break;
            case IAwaitOperation awaitOperation:
                var awaited = UnwrapConfiguredAwait(awaitOperation.Operation, out var maySuppressThrowing);
                AddAwaitFacts(awaited, state, unstable, !maySuppressThrowing);
                break;
            case ISimpleAssignmentOperation assignment:
                TransferAssignment(assignment, state, unstable);
                break;
            case IDeconstructionAssignmentOperation deconstruction:
                RemoveAssignedSymbols(deconstruction.Target, state);
                break;
        }
    }

    private static void TransferAssignment(ISimpleAssignmentOperation assignment, HashSet<ISymbol> state, HashSet<ISymbol> unstable)
    {
        if (GetSymbol(assignment.Target) is not { } target)
        {
            return;
        }

        var value = Unwrap(assignment.Value);
        var isComplete = (GetStableTaskSymbol(value, unstable) is { } source && state.Contains(source))
            || (value is IAwaitOperation awaited && IsWhenAny(UnwrapConfiguredAwait(awaited.Operation, out _)));
        state.Remove(target);
        if (isComplete && GetStableTaskSymbol(assignment.Target, unstable) is not null)
        {
            state.Add(target);
        }
    }

    private static void RemoveAssignedSymbols(IOperation target, HashSet<ISymbol> state)
    {
        foreach (var reference in target.DescendantsAndSelf())
        {
            if (GetSymbol(reference) is { } assigned)
            {
                state.Remove(assigned);
            }
        }
    }

    private static void AddAwaitFacts(
        IOperation awaited,
        HashSet<ISymbol> state,
        HashSet<ISymbol> unstable,
        bool completesSuccessfully = true)
    {
        // An assignment evaluates to the task now stored in its target. Nested
        // assignments can establish completion for more than one stable variable.
        awaited = Unwrap(awaited);
        while (awaited is ISimpleAssignmentOperation assignment)
        {
            if (GetStableTaskSymbol(assignment.Target, unstable) is { } target)
            {
                state.Add(target);
            }

            awaited = Unwrap(assignment.Value);
        }

        if (GetStableTaskSymbol(awaited, unstable) is { } symbol)
        {
            state.Add(symbol);
        }
        else if (awaited is IInvocationOperation invocation
                 && invocation.TargetMethod.Name == "WhenAll"
                 && invocation.TargetMethod.ContainingType.ToDisplayString() == "System.Threading.Tasks.Task")
        {
            AddWhenAllFacts(invocation, state, unstable);
        }
        else if (completesSuccessfully
                 && awaited is IInvocationOperation { TargetMethod.Name: "WaitAsync", Instance: { } instance } wait
                 && IsTask(wait.TargetMethod.ContainingType))
        {
            AddWaitAsyncFacts(wait, instance, state, unstable);
        }
    }

    private static void AddWaitAsyncFacts(
        IInvocationOperation invocation,
        IOperation instance,
        HashSet<ISymbol> state,
        HashSet<ISymbol> unstable)
    {
        // Normal continuation proves the receiver completed; timeout/cancellation
        // throws. Arguments are evaluated after the receiver and can replace it.
        var inputs = new HashSet<ISymbol>(SymbolEqualityComparer.Default);
        AddAwaitFacts(instance, inputs, unstable);
        foreach (var assignment in invocation.Arguments
                     .SelectMany(static argument => argument.Value.DescendantsAndSelf())
                     .OfType<IAssignmentOperation>())
        {
            RemoveAssignedSymbols(assignment.Target, inputs);
        }

        state.UnionWith(inputs);
    }

    private static void AddWhenAllFacts(IInvocationOperation invocation, HashSet<ISymbol> state, HashSet<ISymbol> unstable)
    {
        // Only inline task lists identify individual inputs. Unknown siblings
        // do not invalidate a stable local unless argument evaluation writes it.
        var inputs = new HashSet<ISymbol>(invocation.Arguments
            .SelectMany(static argument => GetInlineTasks(argument.Value))
            .Select(argument => GetStableTaskSymbol(argument, unstable))
            .OfType<ISymbol>(), SymbolEqualityComparer.Default);
        foreach (var assignment in invocation.DescendantsAndSelf().OfType<IAssignmentOperation>())
        {
            RemoveAssignedSymbols(assignment.Target, inputs);
        }

        state.UnionWith(inputs);
    }

    private static IEnumerable<IOperation> GetInlineTasks(IOperation operation)
    {
        operation = Unwrap(operation);
        return operation switch
        {
            IArrayCreationOperation { Initializer: { } initializer } => initializer.ElementValues,
            ICollectionExpressionOperation collection => collection.Elements,
            _ => [operation],
        };
    }

    private static void AddGuardFacts(IOperation condition, bool value, HashSet<ISymbol> state, HashSet<ISymbol> unstable)
    {
        condition = Unwrap(condition);
        if (condition is IUnaryOperation { OperatorKind: UnaryOperatorKind.Not } negated)
        {
            AddGuardFacts(negated.Operand, !value, state, unstable);
        }
        else if (value && condition is IPropertyReferenceOperation
        {
            Property.Name: "IsCompleted" or "IsCompletedSuccessfully" or "IsFaulted" or "IsCanceled",
            Property.ContainingType: { } type,
            Instance: { } instance,
        }
                 && type.ToDisplayString() == "System.Threading.Tasks.Task"
                 && GetStableTaskSymbol(instance, unstable) is { } symbol)
        {
            state.Add(symbol);
        }
    }

    private static bool IsWhenAny(IOperation operation) => operation is IInvocationOperation invocation
        && invocation.TargetMethod.Name == "WhenAny"
        && invocation.TargetMethod.ContainingType.ToDisplayString() == "System.Threading.Tasks.Task";

    private static IOperation UnwrapConfiguredAwait(IOperation operation, out bool maySuppressThrowing)
    {
        operation = Unwrap(operation);
        maySuppressThrowing = false;
        if (operation is not IInvocationOperation { TargetMethod.Name: "ConfigureAwait", Instance: { } instance } invocation
            || !IsTask(invocation.TargetMethod.ContainingType)
            || !invocation.Arguments.All(static argument => IsConfigurationValue(argument.Value)))
        {
            return operation;
        }

        // The configured task itself always completes, but SuppressThrowing can
        // hide a WaitAsync timeout/cancellation while its receiver still runs.
        maySuppressThrowing = invocation.Arguments.Any(static argument =>
            argument.Parameter?.Type.ToDisplayString() == "System.Threading.Tasks.ConfigureAwaitOptions"
            && (argument.Value.ConstantValue is not { HasValue: true, Value: int options }
                || (options & 2) != 0)); // ConfigureAwaitOptions.SuppressThrowing
        return Unwrap(instance);
    }

    private static bool IsConfigurationValue(IOperation operation) => operation.ConstantValue.HasValue
        // Reading an option variable cannot replace the task whose receiver was
        // evaluated first. An arbitrary expression can, e.g. (task = Other()).IsCompleted.
        || Unwrap(operation) is ILocalReferenceOperation or IParameterReferenceOperation;

    private static IOperation Unwrap(IOperation operation)
    {
        while (operation is IConversionOperation { OperatorMethod: null } conversion)
        {
            operation = conversion.Operand;
        }

        return operation;
    }

    private static ISymbol? GetStableTaskSymbol(IOperation operation, HashSet<ISymbol> unstable)
    {
        operation = Unwrap(operation);
        var symbol = GetSymbol(operation);
        return symbol is not null && !unstable.Contains(symbol) && IsTask(operation.Type) ? symbol : null;
    }

    private static IMethodSymbol? GetEnclosingFunction(IOperation operation)
    {
        for (var parent = operation.Parent; parent is not null; parent = parent.Parent)
        {
            if (parent is IAnonymousFunctionOperation anonymous)
            {
                return anonymous.Symbol;
            }

            if (parent is ILocalFunctionOperation local)
            {
                return local.Symbol;
            }
        }

        return null;
    }

    private static ISymbol? GetSymbol(IOperation operation) => operation switch
    {
        ILocalReferenceOperation { Local.RefKind: RefKind.None } local => local.Local,
        IParameterReferenceOperation { Parameter.RefKind: RefKind.None } parameter => parameter.Parameter,
        _ => null,
    };

    private static bool IsTask(ITypeSymbol? type)
    {
        return type?.OriginalDefinition.ToDisplayString() is
            "System.Threading.Tasks.Task" or "System.Threading.Tasks.Task<TResult>";
    }
}
