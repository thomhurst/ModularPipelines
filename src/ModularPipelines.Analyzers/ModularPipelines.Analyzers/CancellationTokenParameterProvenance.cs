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

        public Dictionary<ISymbol, HashSet<string>> Tuples { get; } = [with(SymbolEqualityComparer.Default)];

        public Dictionary<CaptureId, HashSet<string>> TupleCaptures { get; } = [];

        public State Clone()
        {
            var copy = new State();
            copy.Values.UnionWith(Values);
            copy.Captures.UnionWith(Captures);
            foreach (var pair in Tuples)
            {
                copy.Tuples.Add(pair.Key, [.. pair.Value]);
            }

            foreach (var pair in TupleCaptures)
            {
                copy.TupleCaptures.Add(pair.Key, [.. pair.Value]);
            }

            return copy;
        }

        public bool Merge(State other)
        {
            var count = Values.Count + Captures.Count;
            Values.IntersectWith(other.Values);
            Captures.IntersectWith(other.Captures);
            var changed = count != Values.Count + Captures.Count;
            return MergeTuples(Tuples, other.Tuples) | MergeTuples(TupleCaptures, other.TupleCaptures) | changed;
        }
    }

    private static bool MergeTuples<T>(Dictionary<T, HashSet<string>> target, Dictionary<T, HashSet<string>> other) where T : notnull
    {
        var changed = false;
        foreach (var key in target.Keys.ToArray())
        {
            var paths = target[key];
            var count = paths.Count;
            if (other.TryGetValue(key, out var incoming))
            {
                paths.IntersectWith(incoming);
            }
            else
            {
                paths.Clear();
            }

            changed |= count != paths.Count;
        }

        return changed;
    }

    private sealed class Analysis(ControlFlowGraph graph, IParameterSymbol source, SemanticModel model)
    {
        private readonly Dictionary<int, State> _inputs = [];
        private readonly Queue<BasicBlock> _pending = new();
        private readonly Dictionary<TextSpan, bool> _results = [];
        private readonly HashSet<object> _mutableArrays = FindMutableArrays(graph);

        // Array values are references: a write through any alias invalidates that
        // allocation's contents. Exclude the connected alias group throughout the
        // method rather than retaining a stale initializer fact after a write.
        private static HashSet<object> FindMutableArrays(ControlFlowGraph graph)
        {
            var operations = graph.Blocks.SelectMany(static block => block.Operations)
                .SelectMany(static operation => operation.DescendantsAndSelf()).ToArray();
            var mutable = new HashSet<object>();
            var aliases = new List<(object Left, object Right)>();
            foreach (var operation in operations)
            {
                if (operation is IArrayElementReferenceOperation element && IsWritten(element)
                    && ArrayKey(element.ArrayReference) is { } array)
                {
                    mutable.Add(array);
                }

                if (operation is ISimpleAssignmentOperation assignment)
                {
                    if (assignment.Target.Type is IArrayTypeSymbol
                        && ArrayKey(assignment.Target) is { } left && ArrayKey(assignment.Value) is { } right)
                    {
                        aliases.Add((left, right));
                    }
                }
                else if (operation is IFlowCaptureOperation capture && capture.Value.Type is IArrayTypeSymbol
                    && ArrayKey(capture.Value) is { } captured)
                {
                    aliases.Add((capture.Id, captured));
                }
            }

            bool changed;
            do
            {
                changed = false;
                foreach (var (left, right) in aliases)
                {
                    if (mutable.Contains(left))
                    {
                        changed |= mutable.Add(right);
                    }

                    if (mutable.Contains(right))
                    {
                        changed |= mutable.Add(left);
                    }
                }
            }
            while (changed);
            return mutable;

            static object? ArrayKey(IOperation operation)
            {
                while (operation is IConversionOperation conversion)
                {
                    operation = conversion.Operand;
                }

                return operation switch
                {
                    ILocalReferenceOperation local => local.Local,
                    IFlowCaptureReferenceOperation capture => capture.Id,
                    _ => null,
                };
            }
        }

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
                    return !_mutableArrays.Contains(local.Local) && state.Values.Contains(local.Local);
                case IFieldReferenceOperation field when TupleField(field, out var tupleSymbol, out var tuplePath):
                    return state.Tuples.TryGetValue(tupleSymbol, out var tupleFacts) && tupleFacts.Contains(tuplePath);
                case IFlowCaptureReferenceOperation capture:
                    return !_mutableArrays.Contains(capture.Id) && state.Captures.Contains(capture.Id);
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
                case IFlowCaptureOperation capture when capture.Value.Type is INamedTypeSymbol { IsTupleType: true }:
                    state.TupleCaptures[capture.Id] = TupleFacts(capture.Value, state);
                    return false;
                case IFlowCaptureOperation capture:
                    Set(state.Captures, capture.Id, Evaluate(capture.Value, state));
                    return !_mutableArrays.Contains(capture.Id) && state.Captures.Contains(capture.Id);
                case ISimpleAssignmentOperation assignment when assignment.Value.Type is INamedTypeSymbol { IsTupleType: true }:
                    var facts = TupleFacts(assignment.Value, state);
                    if (assignment.Target is ILocalReferenceOperation tupleLocal)
                    {
                        state.Tuples[tupleLocal.Local] = facts;
                    }

                    return false;
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
                        case IFieldReferenceOperation field when TupleField(field, out var tupleSymbol, out var tuplePath):
                            if (state.Tuples.TryGetValue(tupleSymbol, out var tupleFacts))
                            {
                                Set(tupleFacts, tuplePath, assigned);
                            }

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
                        state.Tuples.Remove(localArgument.Local);
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

        private static bool TupleField(IFieldReferenceOperation field, out ISymbol symbol, out string path)
        {
            var indices = new List<int>();
            IOperation? current = field;
            while (current is IFieldReferenceOperation part && part.Field.ContainingType.IsTupleType)
            {
                var fields = part.Field.ContainingType.TupleElements;
                var index = -1;
                for (var i = 0; i < fields.Length; i++)
                {
                    if (SymbolEqualityComparer.Default.Equals(fields[i], part.Field)
                        || SymbolEqualityComparer.Default.Equals(fields[i].CorrespondingTupleField, part.Field))
                    {
                        index = i;
                    }
                }

                if (index < 0)
                {
                    break;
                }

                indices.Insert(0, index);
                current = part.Instance;
            }

            symbol = (current as ILocalReferenceOperation)?.Local!;
            path = string.Join(".", indices);
            return symbol is not null && indices.Count > 0;
        }

        private HashSet<string> TupleFacts(IOperation operation, State state)
        {
            while (operation is IConversionOperation conversion)
            {
                operation = conversion.Operand;
            }

            if (operation is ILocalReferenceOperation local && state.Tuples.TryGetValue(local.Local, out var localFacts))
            {
                return [.. localFacts];
            }

            if (operation is IFlowCaptureReferenceOperation capture && state.TupleCaptures.TryGetValue(capture.Id, out var captureFacts))
            {
                return [.. captureFacts];
            }

            var result = new HashSet<string>();
            if (operation is ITupleOperation tuple)
            {
                for (var i = 0; i < tuple.Elements.Length; i++)
                {
                    var element = tuple.Elements[i];
                    if (element.Type is INamedTypeSymbol { IsTupleType: true })
                    {
                        foreach (var path in TupleFacts(element, state))
                        {
                            result.Add(i + "." + path);
                        }
                    }
                    else if (Evaluate(element, state))
                    {
                        result.Add(i.ToString());
                    }
                }
            }
            else
            {
                Evaluate(operation, state);
            }

            return result;
        }

        private void AssignTuple(IOperation target, IOperation value, State state)
        {
            // Capture the entire value before changing any target, including when
            // the right-hand side is a saved tuple or a conditional flow capture.
            var facts = TupleFacts(value, state);
            Assign(target, string.Empty);

            void Assign(IOperation operation, string path)
            {
                switch (operation)
                {
                    case IParameterReferenceOperation parameter:
                        Set(state.Values, parameter.Parameter, facts.Contains(path));
                        break;
                    case ILocalReferenceOperation local:
                        if (local.Type is INamedTypeSymbol { IsTupleType: true })
                        {
                            state.Tuples[local.Local] = [.. facts.Where(p => p.StartsWith(path + ".", StringComparison.Ordinal)).Select(p => p.Substring(path.Length + 1))];
                        }
                        else
                        {
                            Set(state.Values, local.Local, facts.Contains(path));
                        }

                        break;
                    case ITupleOperation tuple:
                        for (var i = 0; i < tuple.Elements.Length; i++)
                        {
                            Assign(tuple.Elements[i], path.Length == 0 ? i.ToString() : path + "." + i);
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
