using System.Collections.Concurrent;
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
    private readonly SemanticModel _model;
    private readonly Dictionary<(TextSpan Span, OperationKind Kind), bool> _values = [];

    private CancellationTokenReachingValues(ControlFlowGraph graph, SemanticModel model)
    {
        _graph = graph;
        _model = model;
    }

    public static bool? TryGetValue(IOperation reference, IParameterSymbol source)
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

        if (root.SemanticModel is not { } model
            || !root.DescendantsAndSelf().Any(static operation => operation is ILoopOperation or ITryOperation))
        {
            return null;
        }

        var analysis = Analyses.GetValue(root, static _ => new(SymbolEqualityComparer.Default))
            .GetOrAdd(source, _ => new Lazy<CancellationTokenReachingValues?>(() => Create(root, model, source))).Value;
        if (analysis is null)
        {
            return null;
        }

        // No evaluated read means this reference is unreachable, including a
        // return whose cleanup and handlers all exit the callable.
        return !analysis._values.TryGetValue((reference.Syntax.Span, reference.Kind), out var value) || value;
    }

    private static CancellationTokenReachingValues? Create(IOperation root, SemanticModel model, IParameterSymbol source)
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

        var analysis = new CancellationTokenReachingValues(graph, model);
        var initial = new State();
        initial.Values.Add(source);
        analysis.Run(graph.Root, initial, graph.Blocks[0]);
        return analysis;
    }

    // A state contains facts true on every path to this point. Joining can only
    // remove facts, so even mutually dependent copies converge in finite time.
    private sealed class State
    {
        public HashSet<ISymbol> Values { get; } = [with(SymbolEqualityComparer.Default)];

        public HashSet<CaptureId> Captures { get; } = [];

        public State Clone()
        {
            var copy = new State();
            copy.Values.UnionWith(Values);
            copy.Captures.UnionWith(Captures);
            return copy;
        }

        public bool Merge(State other)
        {
            var count = Values.Count + Captures.Count;
            Values.IntersectWith(other.Values);
            Captures.IntersectWith(other.Captures);
            return count != Values.Count + Captures.Count;
        }
    }

    private State? Run(ControlFlowRegion region, State input, BasicBlock entry, Action<BasicBlock, State>? outside = null)
    {
        var states = new Dictionary<int, State>();
        var pending = new Queue<BasicBlock>();
        State? completion = null;
        Enqueue(entry, input);
        while (pending.Count > 0)
        {
            var block = pending.Dequeue();
            var state = states[block.Ordinal].Clone();
            foreach (var operation in block.Operations)
            {
                Evaluate(operation, state, block, Exceptional);
            }

            if (block.BranchValue is { } branchValue)
            {
                Evaluate(branchValue, state, block, Exceptional);
            }

            Follow(block.FallThroughSuccessor, state);
            Follow(block.ConditionalSuccessor, state);
        }

        return completion;

        void Enqueue(BasicBlock block, State state)
        {
            if (block.Ordinal < region.FirstBlockOrdinal || block.Ordinal > region.LastBlockOrdinal)
            {
                outside?.Invoke(block, state);
                return;
            }

            if (!states.TryGetValue(block.Ordinal, out var existing))
            {
                states.Add(block.Ordinal, state.Clone());
                pending.Enqueue(block);
            }
            else if (existing.Merge(state))
            {
                pending.Enqueue(block);
            }
        }

        void Follow(ControlFlowBranch? branch, State state)
        {
            if (branch is null)
            {
                return;
            }

            // Roslyn retains an unreachable successor for constant conditions.
            if (branch.Source.BranchValue?.ConstantValue is { HasValue: true, Value: bool value }
                && branch.Source.ConditionKind != ControlFlowConditionKind.None)
            {
                var conditionalTaken = branch.Source.ConditionKind == ControlFlowConditionKind.WhenTrue ? value : !value;
                if (branch.IsConditionalSuccessor != conditionalTaken)
                {
                    return;
                }
            }

            var next = state.Clone();
            foreach (var finalizer in branch.FinallyRegions)
            {
                next = Run(finalizer, next, _graph.Blocks[finalizer.FirstBlockOrdinal], Enqueue);
                if (next is null)
                {
                    return;
                }
            }

            if (branch.Semantics == ControlFlowBranchSemantics.StructuredExceptionHandling)
            {
                if (completion is null)
                {
                    completion = next.Clone();
                }
                else
                {
                    completion.Merge(next);
                }
            }
            else if (branch.Destination is { } destination)
            {
                Enqueue(destination, next);
            }
            else if (branch.Semantics is ControlFlowBranchSemantics.Throw or ControlFlowBranchSemantics.Rethrow)
            {
                DispatchException(branch.Source, next, Enqueue);
            }
        }

        // Exceptions raised by expressions use the same worklist as ordinary
        // edges. A catch is never entered merely because a return exists.
        void DispatchException(BasicBlock block, State state, Action<BasicBlock, State> enqueue)
        {
            for (var current = block.EnclosingRegion; current.EnclosingRegion is { } parent; current = parent)
            {
                if (current.Kind != ControlFlowRegionKind.Try)
                {
                    continue;
                }

                if (parent.Kind == ControlFlowRegionKind.TryAndFinally)
                {
                    var finalizer = parent.NestedRegions.First(static nested => nested.Kind == ControlFlowRegionKind.Finally);
                    var cleaned = Run(finalizer, state.Clone(), _graph.Blocks[finalizer.FirstBlockOrdinal], Enqueue);
                    if (cleaned is null)
                    {
                        return;
                    }

                    state = cleaned;
                }
                else if (parent.Kind == ControlFlowRegionKind.TryAndCatch)
                {
                    foreach (var handler in parent.NestedRegions.Where(static nested => nested.Kind is
                        ControlFlowRegionKind.Catch or ControlFlowRegionKind.FilterAndHandler))
                    {
                        enqueue(_graph.Blocks[handler.FirstBlockOrdinal], state);
                        if (handler.Kind == ControlFlowRegionKind.Catch
                            && (handler.ExceptionType is null
                                || handler.ExceptionType.SpecialType == SpecialType.System_Object
                                || handler.ExceptionType.ToDisplayString() == "System.Exception"))
                        {
                            return;
                        }
                    }
                }
            }
        }

        void Exceptional(BasicBlock block, State state) => DispatchException(block, state, Enqueue);
    }

    private bool Evaluate(IOperation operation, State state, BasicBlock block, Action<BasicBlock, State> exceptional)
    {
        bool value;
        switch (operation)
        {
            case IParameterReferenceOperation parameter:
                value = state.Values.Contains(parameter.Parameter);
                break;
            case ILocalReferenceOperation local:
                value = state.Values.Contains(local.Local);
                break;
            case IFlowCaptureReferenceOperation capture:
                value = state.Captures.Contains(capture.Id);
                break;
            case IFlowCaptureOperation capture:
                value = Evaluate(capture.Value, state, block, exceptional);
                Set(state.Captures, capture.Id, value);
                break;
            case ISimpleAssignmentOperation assignment:
                if (assignment.Target is not (ILocalReferenceOperation or IParameterReferenceOperation))
                {
                    Evaluate(assignment.Target, state, block, exceptional);
                }

                value = Evaluate(assignment.Value, state, block, exceptional);
                switch (assignment.Target)
                {
                    case ILocalReferenceOperation local:
                        Set(state.Values, local.Local, value);
                        break;
                    case IParameterReferenceOperation parameter:
                        Set(state.Values, parameter.Parameter, value);
                        break;
                    default:
                        exceptional(block, state.Clone());
                        break;
                }

                break;
            case IConversionOperation conversion:
                value = Evaluate(conversion.Operand, state, block, exceptional);
                if (conversion.OperatorMethod is not null || conversion.IsChecked
                    || (!conversion.IsImplicit && conversion.Type?.IsReferenceType == true))
                {
                    exceptional(block, state.Clone());
                }

                break;
            case IPropertyReferenceOperation property:
                var instanceValue = property.Instance is { } instance && Evaluate(instance, state, block, exceptional);
                foreach (var argument in property.Arguments)
                {
                    Evaluate(argument.Value, state, block, exceptional);
                }

                value = instanceValue && property.Property.Name == "Token"
                    && property.Property.ContainingType.ToDisplayString() == "System.Threading.CancellationTokenSource";
                exceptional(block, state.Clone());
                break;
            case IInvocationOperation invocation:
                if (invocation.Instance is { } receiver)
                {
                    Evaluate(receiver, state, block, exceptional);
                }

                var arguments = invocation.Arguments.Select(argument => Evaluate(argument.Value, state, block, exceptional)).ToArray();
                foreach (var argument in invocation.Arguments.Where(static argument => argument.Parameter?.RefKind is RefKind.Ref or RefKind.Out))
                {
                    if (argument.Value is ILocalReferenceOperation local)
                    {
                        state.Values.Remove(local.Local);
                    }
                    else if (argument.Value is IParameterReferenceOperation parameter)
                    {
                        state.Values.Remove(parameter.Parameter);
                    }
                }

                if (!IsBuiltInIteration(invocation))
                {
                    exceptional(block, state.Clone());
                }

                value = invocation.TargetMethod.Name == "CreateLinkedTokenSource"
                    && invocation.TargetMethod.ContainingType.ToDisplayString() == "System.Threading.CancellationTokenSource"
                    && arguments.Any(static argument => argument);
                break;
            case IAnonymousFunctionOperation or IFlowAnonymousFunctionOperation or ILocalFunctionOperation:
                value = false;
                break;
            default:
                var children = operation.ChildOperations.Select(child => Evaluate(child, state, block, exceptional)).ToArray();
                value = operation is IArrayInitializerOperation or IArrayCreationOperation or ICollectionExpressionOperation or ISpreadOperation
                    && children.Any(static child => child);
                if (operation is IAwaitOperation or IObjectCreationOperation or IArrayCreationOperation or IArrayElementReferenceOperation
                    or IFieldReferenceOperation { Instance: not null }
                    || operation is IBinaryOperation { OperatorMethod: not null } or IBinaryOperation { IsChecked: true }
                        or IBinaryOperation { OperatorKind: BinaryOperatorKind.Divide or BinaryOperatorKind.Remainder }
                    || operation is IUnaryOperation { OperatorMethod: not null } or IUnaryOperation { IsChecked: true })
                {
                    exceptional(block, state.Clone());
                }

                break;
        }

        if (operation is ILocalReferenceOperation or IParameterReferenceOperation)
        {
            var key = (operation.Syntax.Span, operation.Kind);
            _values[key] = (!_values.TryGetValue(key, out var existing) || existing) && value;
        }

        return value;
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
