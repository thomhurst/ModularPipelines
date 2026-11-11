using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace ModularPipelines.Analyzers;

/// <summary>Tracks parameter values without changing the existing local-token analysis.</summary>
internal static class CancellationTokenParameterProvenance
{
    public static bool FlowsFromOriginal(IParameterReferenceOperation reference, IParameterSymbol source)
    {
        IOperation root = reference;
        while (root.Parent is { } parent)
        {
            // Captured parameters retain the separate mutable-capture guard.
            if (parent is IAnonymousFunctionOperation or ILocalFunctionOperation)
            {
                return true;
            }

            root = parent;
        }

        if (!root.DescendantsAndSelf().OfType<IParameterReferenceOperation>()
            .Any(candidate => SymbolEqualityComparer.Default.Equals(candidate.Parameter, source)
                && ((candidate.Parent is ISimpleAssignmentOperation assignment && assignment.Target == candidate)
                    || candidate.Parent is IArgumentOperation { Parameter.RefKind: RefKind.Ref or RefKind.Out })))
        {
            return true;
        }

        if (root.SemanticModel is not { } model)
        {
            return false;
        }

        ControlFlowGraph? graph;
        try
        {
            graph = ControlFlowGraph.Create(root.Syntax, model);
        }
        catch (ArgumentException)
        {
            return false;
        }

        if (graph is null)
        {
            return false;
        }

        // Preserve the existing exception/cleanup behavior here. The separate
        // shared control-flow analysis owns exceptional transfers; normal CFG
        // successors alone cannot prove which overwrites reach a handler.
        if (root.DescendantsAndSelf().Any(static operation => operation is ITryOperation))
        {
            return true;
        }

        return new Analysis(graph, source, reference.Syntax.Span).Run();
    }

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

    private sealed class Analysis(ControlFlowGraph graph, IParameterSymbol source, TextSpan target)
    {
        private readonly Dictionary<int, State> _inputs = [];
        private readonly Queue<BasicBlock> _pending = new();
        private bool _result = true;

        public bool Run()
        {
            var initial = new State();
            initial.Values.Add(source);
            Enqueue(graph.Blocks[0], initial);
            while (_pending.Count > 0)
            {
                var block = _pending.Dequeue();
                var state = _inputs[block.Ordinal].Clone();
                foreach (var operation in block.Operations)
                {
                    Evaluate(operation, state);
                }

                if (block.BranchValue is { } branchValue)
                {
                    Evaluate(branchValue, state);
                }

                Follow(block.FallThroughSuccessor, state);
                Follow(block.ConditionalSuccessor, state);
            }

            return _result;
        }

        private void Follow(ControlFlowBranch? branch, State state)
        {
            if (branch?.Destination is { IsReachable: true } destination)
            {
                Enqueue(destination, state);
            }
        }

        private void Enqueue(BasicBlock block, State state)
        {
            if (!_inputs.TryGetValue(block.Ordinal, out var input))
            {
                _inputs.Add(block.Ordinal, state.Clone());
                _pending.Enqueue(block);
            }
            else if (input.Merge(state))
            {
                _pending.Enqueue(block);
            }
        }

        private bool Evaluate(IOperation operation, State state)
        {
            switch (operation)
            {
                case IParameterReferenceOperation parameter:
                    var value = state.Values.Contains(parameter.Parameter);
                    if (parameter.Syntax.Span == target)
                    {
                        _result &= value;
                    }

                    return value;
                case ILocalReferenceOperation local:
                    return state.Values.Contains(local.Local);
                case IFlowCaptureReferenceOperation capture:
                    return state.Captures.Contains(capture.Id);
                case IConversionOperation conversion:
                    return Evaluate(conversion.Operand, state);
                case IPropertyReferenceOperation { Property.Name: "Token", Instance: { } instance } property
                    when property.Property.ContainingType.ToDisplayString() == "System.Threading.CancellationTokenSource":
                    return Evaluate(instance, state);
                case IInvocationOperation invocation
                    when invocation.TargetMethod.Name == "CreateLinkedTokenSource"
                        && invocation.TargetMethod.ContainingType.ToDisplayString() == "System.Threading.CancellationTokenSource":
                    var linked = false;
                    foreach (var argument in invocation.Arguments)
                    {
                        linked |= Evaluate(argument.Value, state);
                    }

                    return linked;
                case IFlowCaptureOperation capture:
                    Set(state.Captures, capture.Id, Evaluate(capture.Value, state));
                    return state.Captures.Contains(capture.Id);
                case ISimpleAssignmentOperation assignment:
                    var assigned = Evaluate(assignment.Value, state);
                    switch (assignment.Target)
                    {
                        case IParameterReferenceOperation parameter:
                            Set(state.Values, parameter.Parameter, assigned);
                            break;
                        case ILocalReferenceOperation local:
                            Set(state.Values, local.Local, assigned);
                            break;
                    }

                    return assigned;
                case IArgumentOperation argument when argument.Parameter?.RefKind is RefKind.Ref or RefKind.Out:
                    Evaluate(argument.Value, state);
                    if (argument.Value is IParameterReferenceOperation parameterArgument)
                    {
                        state.Values.Remove(parameterArgument.Parameter);
                    }
                    else if (argument.Value is ILocalReferenceOperation localArgument)
                    {
                        state.Values.Remove(localArgument.Local);
                    }

                    return false;
                case IAnonymousFunctionOperation or IFlowAnonymousFunctionOperation or ILocalFunctionOperation:
                    return false;
                default:
                    foreach (var child in operation.ChildOperations)
                    {
                        Evaluate(child, state);
                    }

                    return false;
            }
        }

        private static void Set<T>(HashSet<T> values, T key, bool present)
        {
            if (present)
            {
                values.Add(key);
            }
            else
            {
                values.Remove(key);
            }
        }
    }
}
