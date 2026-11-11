using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace ModularPipelines.Analyzers;

/// <summary>Tracks parameter values without changing the existing local-token analysis.</summary>
internal static class CancellationTokenParameterProvenance
{
    private static readonly ConditionalWeakTable<IOperation, Dictionary<IParameterSymbol, Dictionary<TextSpan, bool>>> Results = new();

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

        var results = Results.GetValue(root, static _ => [with(SymbolEqualityComparer.Default)]);
        Dictionary<TextSpan, bool>? references;
        lock (results)
        {
            results.TryGetValue(source, out references);
        }

        if (references is null)
        {
            // Helper analysis can visit another method. Do not hold a cache lock
            // while following it: concurrent analysis could visit methods in reverse order.
            references = Analyze(root, source);
            lock (results)
            {
                results[source] = references;
            }
        }

        return !references.TryGetValue(reference.Syntax.Span, out var value) || value;
    }

    private static Dictionary<TextSpan, bool> Analyze(IOperation root, IParameterSymbol source)
    {
        if (!root.DescendantsAndSelf().OfType<IParameterReferenceOperation>()
            .Any(candidate => SymbolEqualityComparer.Default.Equals(candidate.Parameter, source)
                && IsWritten(candidate)))
        {
            return [];
        }

        // Exception and cleanup transfers remain on the existing analysis path.
        if (root.DescendantsAndSelf().Any(static operation => operation is ITryOperation)
            || root.SemanticModel is not { } model)
        {
            return [];
        }

        try
        {
            return ControlFlowGraph.Create(root.Syntax, model) is { } graph
                ? new Analysis(graph, source, model).Run()
                : [];
        }
        catch (ArgumentException)
        {
            return [];
        }
    }

    private static bool IsWritten(IOperation operation)
    {
        while (operation.Parent is ITupleOperation or IConversionOperation)
        {
            operation = operation.Parent;
        }

        return (operation.Parent is IAssignmentOperation assignment && assignment.Target == operation)
            || operation.Parent is IArgumentOperation { Parameter.RefKind: RefKind.Ref or RefKind.Out };
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

    private sealed class Analysis(ControlFlowGraph graph, IParameterSymbol source, SemanticModel model)
    {
        private readonly Dictionary<int, State> _inputs = [];
        private readonly Queue<BasicBlock> _pending = new();
        private readonly Dictionary<TextSpan, bool> _results = [];

        public Dictionary<TextSpan, bool> Run()
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

                var constant = block.BranchValue?.ConstantValue;
                if (constant is { HasValue: true, Value: bool condition }
                    && block.ConditionKind != ControlFlowConditionKind.None)
                {
                    Follow(condition == (block.ConditionKind == ControlFlowConditionKind.WhenTrue)
                        ? block.ConditionalSuccessor
                        : block.FallThroughSuccessor, state);
                }
                else
                {
                    Follow(block.FallThroughSuccessor, state);
                    Follow(block.ConditionalSuccessor, state);
                }
            }

            return _results;
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
                    var span = parameter.Syntax.Span;
                    _results[span] = value && (!_results.TryGetValue(span, out var previous) || previous);

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
                case IArrayCreationOperation { Initializer: { } initializer }:
                    return Evaluate(initializer, state);
                case IArrayInitializerOperation initializer:
                    return EvaluateAny(initializer.ElementValues, state);
                case ICollectionExpressionOperation collection:
                    return EvaluateAny(collection.Elements, state);
                case ISpreadOperation spread:
                    return Evaluate(spread.Operand, state);
                default:
                    return EvaluateEffects(operation, state);
            }
        }

        private bool EvaluateInvocation(IInvocationOperation invocation, State state)
        {
            if (invocation.TargetMethod.Name == "CreateLinkedTokenSource"
                && invocation.TargetMethod.ContainingType.ToDisplayString() == "System.Threading.CancellationTokenSource")
            {
                return EvaluateAny(invocation.Arguments.Select(static argument => argument.Value), state);
            }

            // CFG operations have no SemanticModel. Recover the source operation
            // so the existing helper analysis can inspect its declaration.
            var arguments = new Dictionary<TextSpan, bool>();
            if (invocation.Instance is { } receiver)
            {
                arguments[receiver.Syntax.Span] = Evaluate(receiver, state);
            }

            foreach (var argument in invocation.Arguments)
            {
                arguments[argument.Value.Syntax.Span] = Evaluate(argument, state);
            }

            return model.GetOperation(invocation.Syntax) is IInvocationOperation original
                && ModuleAuthoringAnalysis.FlowsFromAssignedCancellationTokenHelper(
                    original, operation => arguments.TryGetValue(operation.Syntax.Span, out var safe) && safe);
        }

        private bool EvaluateEffects(IOperation operation, State state)
        {
            switch (operation)
            {
                case IInvocationOperation invocation:
                    return EvaluateInvocation(invocation, state);
                case IDeconstructionAssignmentOperation deconstruction:
                    AssignTuple(deconstruction.Target, deconstruction.Value, state);
                    return false;
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
                case IArgumentOperation argument:
                    return Evaluate(argument.Value, state);
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

        private bool EvaluateAny(IEnumerable<IOperation> operations, State state)
        {
            var result = false;
            foreach (var operation in operations)
            {
                result |= Evaluate(operation, state);
            }

            return result;
        }

        private void AssignTuple(IOperation target, IOperation value, State state)
        {
            while (value is IConversionOperation conversion)
            {
                value = conversion.Operand;
            }

            if (target is ITupleOperation targets && value is ITupleOperation values)
            {
                // Evaluate all right-hand values before writing any left-hand target.
                var assignments = new List<(IOperation Target, bool Safe)>();
                Collect(targets, values);
                foreach (var (assignedTarget, isSafe) in assignments)
                {
                    Assign(assignedTarget, isSafe);
                }

                void Collect(ITupleOperation left, ITupleOperation right)
                {
                    for (var i = 0; i < left.Elements.Length; i++)
                    {
                        if (left.Elements[i] is ITupleOperation nestedLeft && right.Elements[i] is ITupleOperation nestedRight)
                        {
                            Collect(nestedLeft, nestedRight);
                        }
                        else
                        {
                            assignments.Add((left.Elements[i], Evaluate(right.Elements[i], state)));
                        }
                    }
                }
            }
            else
            {
                Evaluate(value, state);
                Assign(target, false);
            }

            void Assign(IOperation operation, bool safe)
            {
                switch (operation)
                {
                    case IParameterReferenceOperation parameter:
                        Set(state.Values, parameter.Parameter, safe);
                        break;
                    case ILocalReferenceOperation local:
                        Set(state.Values, local.Local, safe);
                        break;
                    case ITupleOperation tuple:
                        foreach (var element in tuple.Elements)
                        {
                            Assign(element, false);
                        }

                        break;
                }
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
