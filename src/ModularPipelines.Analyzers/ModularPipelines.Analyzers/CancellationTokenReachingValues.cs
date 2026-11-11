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
    private readonly OperationExceptionClassifier _exceptions;
    private readonly Dictionary<(TextSpan Span, OperationKind Kind), bool> _values = [];

    private CancellationTokenReachingValues(ControlFlowGraph graph, SemanticModel model)
    {
        _graph = graph;
        _model = model;
        _exceptions = new OperationExceptionClassifier(model);
    }

    public static bool? TryGetValue(IOperation reference, IParameterSymbol source)
    {
        var root = GetAnalysisRoot(reference);
        if (root?.SemanticModel is not { } model
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
        return new Traversal(this, region, outside).Run(input, entry);
    }

    private sealed class Traversal(
        CancellationTokenReachingValues analysis,
        ControlFlowRegion region,
        Action<BasicBlock, State>? outside)
    {
        private readonly Dictionary<int, State> _states = [];
        private readonly Queue<BasicBlock> _pending = new();
        private State? _completion;

        public State? Run(State input, BasicBlock entry)
        {
            Enqueue(entry, input);
            while (_pending.Count > 0)
            {
                var block = _pending.Dequeue();
                var state = _states[block.Ordinal].Clone();
                foreach (var operation in block.Operations)
                {
                    analysis.Evaluate(operation, state, block, Exceptional);
                }

                if (block.BranchValue is { } branchValue)
                {
                    analysis.Evaluate(branchValue, state, block, Exceptional);
                }

                Follow(block.FallThroughSuccessor, state);
                Follow(block.ConditionalSuccessor, state);
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

            if (!_states.TryGetValue(block.Ordinal, out var existing))
            {
                _states.Add(block.Ordinal, state.Clone());
                _pending.Enqueue(block);
            }
            else if (existing.Merge(state))
            {
                _pending.Enqueue(block);
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

        private void Follow(ControlFlowBranch? branch, State state)
        {
            if (branch is null || !IsBranchTaken(branch))
            {
                return;
            }

            var next = RunFinalizers(branch, state);
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
                Enqueue(destination, next);
            }
            else if (branch.Semantics is ControlFlowBranchSemantics.Throw or ControlFlowBranchSemantics.Rethrow)
            {
                Exceptional(branch.Source, next);
            }
        }

        private State? RunFinalizers(ControlFlowBranch branch, State state)
        {
            var next = state.Clone();
            foreach (var finalizer in branch.FinallyRegions)
            {
                next = analysis.Run(finalizer, next, analysis._graph.Blocks[finalizer.FirstBlockOrdinal], Enqueue);
                if (next is null)
                {
                    return null;
                }
            }

            return next;
        }

        private static bool IsBranchTaken(ControlFlowBranch branch)
        {
            // Roslyn retains an unreachable successor for constant conditions.
            if (branch.Source.BranchValue?.ConstantValue is not { HasValue: true, Value: bool value }
                || branch.Source.ConditionKind == ControlFlowConditionKind.None)
            {
                return true;
            }

            var conditionalTaken = branch.Source.ConditionKind == ControlFlowConditionKind.WhenTrue ? value : !value;
            return branch.IsConditionalSuccessor == conditionalTaken;
        }

        // Exceptions raised by expressions use the same worklist as ordinary
        // edges. A catch is never entered merely because a return exists.
        private void DispatchException(BasicBlock block, State state)
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
                    var cleaned = analysis.Run(finalizer, state.Clone(), analysis._graph.Blocks[finalizer.FirstBlockOrdinal], Enqueue);
                    if (cleaned is null)
                    {
                        return;
                    }

                    state = cleaned;
                }
                else if (parent.Kind == ControlFlowRegionKind.TryAndCatch)
                {
                    var unhandled = DispatchHandlers(parent, state);
                    if (unhandled is null)
                    {
                        return;
                    }

                    state = unhandled;
                }
            }
        }

        private State? DispatchHandlers(ControlFlowRegion handlers, State state)
        {
            foreach (var handler in handlers.NestedRegions.Where(static nested => nested.Kind is
                ControlFlowRegionKind.Catch or ControlFlowRegionKind.FilterAndHandler))
            {
                if (handler.Kind == ControlFlowRegionKind.Catch)
                {
                    Enqueue(analysis._graph.Blocks[handler.FirstBlockOrdinal], state);
                    if (IsCatchAll(handler))
                    {
                        return null;
                    }

                    continue;
                }

                var filter = handler.NestedRegions.First(static nested => nested.Kind == ControlFlowRegionKind.Filter);
                var rejected = analysis.Run(filter, state.Clone(), analysis._graph.Blocks[filter.FirstBlockOrdinal], Enqueue);
                var catchRegion = handler.NestedRegions.First(static nested => nested.Kind == ControlFlowRegionKind.Catch);
                if (IsCatchAll(catchRegion))
                {
                    if (rejected is null)
                    {
                        return null;
                    }

                    state = rejected;
                }
                else if (rejected is not null)
                {
                    // The exception type can also skip this filter entirely.
                    state = state.Clone();
                    state.Merge(rejected);
                }
            }

            return state;
        }

        private bool IsCatchAll(ControlFlowRegion handler)
        {
            return handler.ExceptionType is null
                || handler.ExceptionType.SpecialType == SpecialType.System_Object
                || SymbolEqualityComparer.Default.Equals(handler.ExceptionType, analysis._model.Compilation.GetTypeByMetadataName("System.Exception"));
        }

        private void Exceptional(BasicBlock block, State state)
        {
            if (region.Kind == ControlFlowRegionKind.Filter)
            {
                // A throwing filter is treated as false, retaining its writes.
                Complete(state);
            }
            else
            {
                DispatchException(block, state);
            }
        }
    }

    private bool Evaluate(IOperation operation, State state, BasicBlock block, Action<BasicBlock, State> exceptional)
    {
        var value = operation switch
        {
            IParameterReferenceOperation parameter => state.Values.Contains(parameter.Parameter),
            ILocalReferenceOperation local => state.Values.Contains(local.Local),
            IFlowCaptureReferenceOperation capture => state.Captures.Contains(capture.Id),
            IFlowCaptureOperation capture => EvaluateCapture(capture, state, block, exceptional),
            ISimpleAssignmentOperation assignment => EvaluateAssignment(assignment, state, block, exceptional),
            IConversionOperation conversion => EvaluateConversion(conversion, state, block, exceptional),
            IPropertyReferenceOperation property => EvaluateProperty(property, state, block, exceptional),
            IInvocationOperation invocation => EvaluateInvocation(invocation, state, block, exceptional),
            _ => EvaluateOther(operation, state, block, exceptional),
        };
        if (operation is ILocalReferenceOperation or IParameterReferenceOperation)
        {
            var key = (operation.Syntax.Span, operation.Kind);
            _values[key] = (!_values.TryGetValue(key, out var existing) || existing) && value;
        }

        return value;
    }

    private bool EvaluateCapture(IFlowCaptureOperation capture, State state, BasicBlock block, Action<BasicBlock, State> exceptional)
    {
        var value = Evaluate(capture.Value, state, block, exceptional);
        Set(state.Captures, capture.Id, value);
        return value;
    }

    private bool EvaluateAssignment(ISimpleAssignmentOperation assignment, State state, BasicBlock block, Action<BasicBlock, State> exceptional)
    {
        if (assignment.Target is not (ILocalReferenceOperation or IParameterReferenceOperation or IDiscardOperation))
        {
            Evaluate(assignment.Target, state, block, exceptional);
        }

        var value = Evaluate(assignment.Value, state, block, exceptional);
        switch (assignment.Target)
        {
            case ILocalReferenceOperation local:
                Set(state.Values, local.Local, value);
                break;
            case IParameterReferenceOperation parameter:
                Set(state.Values, parameter.Parameter, value);
                break;
            case IDiscardOperation:
                break;
            default:
                exceptional(block, state.Clone());
                break;
        }

        return value;
    }

    private bool EvaluateConversion(IConversionOperation conversion, State state, BasicBlock block, Action<BasicBlock, State> exceptional)
    {
        var value = Evaluate(conversion.Operand, state, block, exceptional);
        if (!_exceptions.Classify(conversion).IsEmpty)
        {
            exceptional(block, state.Clone());
        }

        return value;
    }

    private bool EvaluateProperty(IPropertyReferenceOperation property, State state, BasicBlock block, Action<BasicBlock, State> exceptional)
    {
        var instanceValue = property.Instance is { } instance && Evaluate(instance, state, block, exceptional);
        foreach (var argument in property.Arguments)
        {
            Evaluate(argument.Value, state, block, exceptional);
        }

        exceptional(block, state.Clone());
        return instanceValue && property.Property.Name == "Token"
            && property.Property.ContainingType.ToDisplayString() == "System.Threading.CancellationTokenSource";
    }

    private bool EvaluateInvocation(IInvocationOperation invocation, State state, BasicBlock block, Action<BasicBlock, State> exceptional)
    {
        var receiverValue = invocation.Instance is { } receiver && Evaluate(receiver, state, block, exceptional);
        var arguments = invocation.Arguments.Select(argument => Evaluate(argument.Value, state, block, exceptional)).ToArray();
        InvalidateRefArguments(invocation, state);
        if (!IsBuiltInIteration(invocation))
        {
            exceptional(block, state.Clone());
        }

        return (invocation.TargetMethod.Name == "CreateLinkedTokenSource"
                && invocation.TargetMethod.ContainingType.ToDisplayString() == "System.Threading.CancellationTokenSource"
                && arguments.Any(static argument => argument))
            || ModuleAuthoringAnalysis.FlowsFromCancellationTokenSourceHelper(invocation, _model.Compilation, arguments, receiverValue);
    }

    private static void InvalidateRefArguments(IInvocationOperation invocation, State state)
    {
        foreach (var argument in invocation.Arguments.Where(static argument => argument.Parameter?.RefKind is RefKind.Ref or RefKind.Out))
        {
            InvalidateArgument(argument.Value, state);
        }
    }

    private bool EvaluateOther(IOperation operation, State state, BasicBlock block, Action<BasicBlock, State> exceptional)
    {
        if (operation is IAnonymousFunctionOperation or IFlowAnonymousFunctionOperation or ILocalFunctionOperation)
        {
            return false;
        }

        var children = operation.ChildOperations.Select(child => Evaluate(child, state, block, exceptional)).ToArray();
        InvalidateDynamicRefArguments(operation, state);
        if (!_exceptions.Classify(operation).IsEmpty)
        {
            exceptional(block, state.Clone());
        }

        return operation is IArrayInitializerOperation or IArrayCreationOperation or ICollectionExpressionOperation or ISpreadOperation
            && children.Any(static child => child);
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
        switch (argument)
        {
            case ILocalReferenceOperation local:
                state.Values.Remove(local.Local);
                break;
            case IParameterReferenceOperation parameter:
                state.Values.Remove(parameter.Parameter);
                break;
        }
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
