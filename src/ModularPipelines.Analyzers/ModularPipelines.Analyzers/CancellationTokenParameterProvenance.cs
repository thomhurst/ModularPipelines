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

    private sealed class Value(bool safe = false, Value[]? elements = null, HashSet<TextSpan>? arrays = null, bool unknownArray = false)
    {
        public static readonly Value None = new();

        public static readonly Value Original = new(safe: true);

        public bool Safe { get; } = safe;

        public Value[]? Elements { get; } = elements;

        public HashSet<TextSpan>? Arrays { get; } = arrays;

        public bool UnknownArray { get; } = unknownArray;

        public Value Merge(Value other)
        {
            if (Elements is { } left && other.Elements is { } right && left.Length == right.Length)
            {
                return new Value(elements: [.. left.Select((element, index) => element.Merge(right[index]))]);
            }

            if (Elements is { } tuple)
            {
                return new Value(elements: [.. tuple.Select(static element => element.Merge(None))]);
            }

            if (other.Elements is not null)
            {
                return other.Merge(this);
            }

            if (Arrays is { } leftArrays && other.Arrays is { } rightArrays)
            {
                return new Value(arrays: [.. leftArrays, .. rightArrays], unknownArray: UnknownArray || other.UnknownArray);
            }

            if (Arrays is { } knownArrays)
            {
                return new Value(arrays: knownArrays, unknownArray: true);
            }

            if (other.Arrays is not null)
            {
                return other.Merge(this);
            }

            return Safe && other.Safe ? Original : None;
        }

        public bool SameAs(Value other) => Safe == other.Safe && UnknownArray == other.UnknownArray
            && ((Elements is null && other.Elements is null)
                || (Elements is { } left && other.Elements is { } right && left.Length == right.Length
                    && left.Select((element, index) => element.SameAs(right[index])).All(static same => same)))
            && ((Arrays is null && other.Arrays is null)
                || (Arrays is { } leftArrays && other.Arrays is { } rightArrays && leftArrays.SetEquals(rightArrays)));
    }

    private sealed class State
    {
        public Dictionary<ISymbol, Value> Values { get; } = [with(SymbolEqualityComparer.Default)];

        public Dictionary<CaptureId, Value> Captures { get; } = [];

        public Dictionary<TextSpan, Value[]> Arrays { get; } = [];

        public HashSet<TextSpan> SafeArrays { get; } = [];

        public void SetArraySafety(TextSpan identity, bool safe)
        {
            if (safe)
            {
                SafeArrays.Add(identity);
            }
            else
            {
                SafeArrays.Remove(identity);
            }
        }

        public State Clone()
        {
            var copy = new State();
            foreach (var entry in Values)
            {
                copy.Values.Add(entry.Key, entry.Value);
            }

            foreach (var entry in Captures)
            {
                copy.Captures.Add(entry.Key, entry.Value);
            }

            foreach (var entry in Arrays)
            {
                copy.Arrays.Add(entry.Key, (Value[]) entry.Value.Clone());
            }

            copy.SafeArrays.UnionWith(SafeArrays);

            return copy;
        }

        public bool Merge(State other)
        {
            var changed = MergeValues(Values, other.Values) | MergeValues(Captures, other.Captures);
            foreach (var entry in other.Arrays)
            {
                var span = entry.Key;
                var elements = entry.Value;
                if (!Arrays.TryGetValue(span, out var current))
                {
                    Arrays.Add(span, (Value[]) elements.Clone());
                    SetArraySafety(span, other.SafeArrays.Contains(span));
                    changed = true;
                    continue;
                }

                var safe = SafeArrays.Contains(span) && other.SafeArrays.Contains(span);
                changed |= safe != SafeArrays.Contains(span);
                SetArraySafety(span, safe);

                for (var i = 0; i < current.Length; i++)
                {
                    var merged = current[i].Merge(i < elements.Length ? elements[i] : Value.None);
                    changed |= !merged.SameAs(current[i]);
                    current[i] = merged;
                }
            }

            return changed;
        }

        private static bool MergeValues<TKey>(Dictionary<TKey, Value> values, Dictionary<TKey, Value> other)
            where TKey : notnull
        {
            var changed = false;
            foreach (var key in values.Keys.ToArray())
            {
                var merged = values[key].Merge(other.TryGetValue(key, out var value) ? value : Value.None);
                changed |= !merged.SameAs(values[key]);
                values[key] = merged;
            }

            return changed;
        }
    }

    private sealed class Analysis(ControlFlowGraph graph, IParameterSymbol source, SemanticModel model)
    {
        private readonly Dictionary<int, State> _inputs = [];
        private readonly Queue<BasicBlock> _pending = new();
        private readonly Dictionary<TextSpan, bool> _results = [];
        private readonly HashSet<TextSpan> _summarizedArrays = [];

        public Dictionary<TextSpan, bool> Run()
        {
            var initial = new State();
            initial.Values.Add(source, Value.Original);
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

        private Value Evaluate(IOperation operation, State state)
        {
            switch (operation)
            {
                case IParameterReferenceOperation parameter:
                    var value = Get(state.Values, parameter.Parameter);
                    var safe = IsSafe(value, state);
                    var span = parameter.Syntax.Span;
                    _results[span] = safe && (!_results.TryGetValue(span, out var previous) || previous);

                    return value;
                case ILocalReferenceOperation local:
                    return Get(state.Values, local.Local);
                case IFlowCaptureReferenceOperation capture:
                    return Get(state.Captures, capture.Id);
                case IConversionOperation conversion:
                    var converted = Evaluate(conversion.Operand, state);
                    if (converted.Elements is null || conversion.Type is INamedTypeSymbol { IsTupleType: true })
                    {
                        return converted;
                    }

                    InvalidateArrays(converted, state);
                    return Value.None;
                case IPropertyReferenceOperation { Property.Name: "Token", Instance: { } instance } property
                    when property.Property.ContainingType.ToDisplayString() == "System.Threading.CancellationTokenSource":
                    return Evaluate(instance, state);
                case ITupleOperation tuple:
                    return new Value(elements: [.. tuple.Elements.Select(element => Evaluate(element, state))]);
                case IFieldReferenceOperation field when TupleElementIndex(field) >= 0:
                    return Element(Evaluate(field.Instance!, state), TupleElementIndex(field));
                case IArrayCreationOperation { Initializer: { } initializer } array:
                    var arrayElements = initializer.ElementValues.Select(element => Evaluate(element, state)).ToArray();
                    if (IsTokenCollection(array.Type))
                    {
                        return CreateArray(array.Syntax.Span, arrayElements, state);
                    }

                    foreach (var arrayElement in arrayElements)
                    {
                        InvalidateArrays(arrayElement, state);
                    }

                    return Value.None;
                case ICollectionExpressionOperation collection:
                    var elements = new List<Value>();
                    foreach (var element in collection.Elements)
                    {
                        if (element is ISpreadOperation spread)
                        {
                            // One fact per source element bounds loops that append a spread.
                            _summarizedArrays.Add(collection.Syntax.Span);
                            elements.Add(IsSafe(Evaluate(spread.Operand, state), state) ? Value.Original : Value.None);
                        }
                        else
                        {
                            elements.Add(Evaluate(element, state));
                        }
                    }

                    if (IsTokenCollection(collection.Type))
                    {
                        return CreateArray(collection.Syntax.Span, elements, state);
                    }

                    foreach (var element in elements)
                    {
                        InvalidateArrays(element, state);
                    }

                    return Value.None;
                case IArrayElementReferenceOperation element:
                    var arrayValue = Evaluate(element.ArrayReference, state);
                    foreach (var index in element.Indices)
                    {
                        Evaluate(index, state);
                    }

                    if (element.Indices.Length == 1 && element.Indices[0].ConstantValue is { HasValue: true, Value: int arrayIndex }
                        && arrayValue.Arrays is { } arrayIdentities && !arrayIdentities.Any(_summarizedArrays.Contains))
                    {
                        return ArrayElement(arrayValue, arrayIndex, state);
                    }

                    return Value.None;
                default:
                    return EvaluateEffects(operation, state);
            }
        }

        private Value EvaluateInvocation(IInvocationOperation invocation, State state)
        {
            if (invocation.TargetMethod.Name == "CreateLinkedTokenSource"
                && invocation.TargetMethod.ContainingType.ToDisplayString() == "System.Threading.CancellationTokenSource")
            {
                var linked = false;
                foreach (var argument in invocation.Arguments)
                {
                    linked |= IsSafe(Evaluate(argument.Value, state), state);
                }

                return linked ? Value.Original : Value.None;
            }

            // CFG operations have no SemanticModel. Recover the source operation
            // so the existing helper analysis can inspect its declaration.
            var arguments = new Dictionary<TextSpan, bool>();
            if (invocation.Instance is { } receiver)
            {
                var value = Evaluate(receiver, state);
                arguments[receiver.Syntax.Span] = IsSafe(value, state);
                InvalidateArrays(value, state);
            }

            foreach (var argument in invocation.Arguments)
            {
                var value = Evaluate(argument, state);
                arguments[argument.Value.Syntax.Span] = IsSafe(value, state);
                InvalidateArrays(value, state);
            }

            if (model.GetOperation(invocation.Syntax) is IInvocationOperation original
                && ModuleAuthoringAnalysis.FlowsFromAssignedCancellationTokenHelper(
                    original, operation => arguments.TryGetValue(operation.Syntax.Span, out var safe) && safe))
            {
                return Value.Original;
            }

            return Value.None;
        }

        private Value EvaluateEffects(IOperation operation, State state)
        {
            switch (operation)
            {
                case IInvocationOperation invocation:
                    return EvaluateInvocation(invocation, state);
                case IDeconstructionAssignmentOperation deconstruction:
                    var tuple = Evaluate(deconstruction.Value, state);
                    Assign(deconstruction.Target, tuple, state);
                    return tuple;
                case IFlowCaptureOperation capture:
                    var captured = Evaluate(capture.Value, state);
                    state.Captures[capture.Id] = captured;
                    return captured;
                case ISimpleAssignmentOperation assignment:
                    var assigned = Evaluate(assignment.Value, state);
                    Assign(assignment.Target, assigned, state);
                    return assigned;
                case IArgumentOperation argument when argument.Parameter?.RefKind is RefKind.Ref or RefKind.Out:
                    InvalidateArrays(Evaluate(argument.Value, state), state);
                    Assign(argument.Value, Value.None, state);
                    return Value.None;
                case IArgumentOperation argument:
                    return Evaluate(argument.Value, state);
                case IAnonymousFunctionOperation or IFlowAnonymousFunctionOperation or ILocalFunctionOperation:
                    return Value.None;
                default:
                    foreach (var child in operation.ChildOperations)
                    {
                        Evaluate(child, state);
                    }

                    return Value.None;
            }
        }

        private static Value Get<TKey>(Dictionary<TKey, Value> values, TKey key)
            where TKey : notnull => values.TryGetValue(key, out var value) ? value : Value.None;

        private static bool IsSafe(Value value, State state) => value.Safe
            || (!value.UnknownArray && value.Arrays is { Count: > 0 } arrays
                && arrays.All(state.SafeArrays.Contains));

        private static bool IsTokenCollection(ITypeSymbol? type) => type switch
        {
            IArrayTypeSymbol { Rank: 1 } array => array.ElementType.ToDisplayString() == "System.Threading.CancellationToken",
            INamedTypeSymbol { TypeArguments.Length: 1 } named => named.TypeArguments[0].ToDisplayString() == "System.Threading.CancellationToken"
                && named.OriginalDefinition.ToDisplayString() is "System.Span<T>" or "System.ReadOnlySpan<T>",
            _ => false,
        };

        private static Value Element(Value value, int index) => value.Elements is { } elements && index >= 0 && index < elements.Length ? elements[index] : Value.None;

        private static int TupleElementIndex(IFieldReferenceOperation field)
        {
            if (field.Instance?.Type is not INamedTypeSymbol { IsTupleType: true } tuple)
            {
                return -1;
            }

            for (var i = 0; i < tuple.TupleElements.Length; i++)
            {
                if (SymbolEqualityComparer.Default.Equals(tuple.TupleElements[i], field.Field)
                    || SymbolEqualityComparer.Default.Equals(tuple.TupleElements[i].CorrespondingTupleField, field.Field.CorrespondingTupleField))
                {
                    return i;
                }
            }

            return -1;
        }

        private static Value CreateArray(TextSpan identity, IEnumerable<Value> elements, State state)
        {
            // Allocation sites are finite, even when a loop revisits this operation.
            var values = elements.ToArray();
            var safe = values.Any(static value => value.Safe);
            if (state.Arrays.TryGetValue(identity, out var previous))
            {
                // Aliases can still refer to an earlier allocation at this site.
                safe &= state.SafeArrays.Contains(identity);
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] = values[i].Merge(i < previous.Length ? previous[i] : Value.None);
                }
            }

            state.Arrays[identity] = values;
            state.SetArraySafety(identity, safe);
            return new Value(arrays: [identity]);
        }

        private static Value ArrayElement(Value value, int index, State state)
        {
            Value? result = null;
            if (value.UnknownArray || value.Arrays is not { Count: > 0 } arrays)
            {
                return Value.None;
            }

            foreach (var span in arrays)
            {
                if (!state.Arrays.TryGetValue(span, out var elements)
                    || index < 0 || index >= elements.Length)
                {
                    return Value.None;
                }

                result = result is null ? elements[index] : result.Merge(elements[index]);
            }

            return result ?? Value.None;
        }

        private static void InvalidateArrays(Value value, State state)
        {
            if (value.Arrays is { } arrays)
            {
                foreach (var span in arrays)
                {
                    if (state.Arrays.TryGetValue(span, out var elements))
                    {
                        state.Arrays[span] = [.. elements.Select(static _ => Value.None)];
                        state.SafeArrays.Remove(span);
                    }
                }
            }

            if (value.Elements is { } tuple)
            {
                foreach (var element in tuple)
                {
                    InvalidateArrays(element, state);
                }
            }
        }

        private void Assign(IOperation target, Value value, State state)
        {
            switch (target)
            {
                case IParameterReferenceOperation parameter:
                    state.Values[parameter.Parameter] = value;
                    break;
                case ILocalReferenceOperation local:
                    state.Values[local.Local] = value;
                    break;
                case IFlowCaptureReferenceOperation capture:
                    state.Captures[capture.Id] = value;
                    break;
                case IConversionOperation conversion:
                    Assign(conversion.Operand, value, state);
                    break;
                case IDeclarationExpressionOperation declaration:
                    Assign(declaration.Expression, value, state);
                    break;
                case ITupleOperation tuple:
                    // The complete right-hand value was evaluated before any write.
                    for (var i = 0; i < tuple.Elements.Length; i++)
                    {
                        Assign(tuple.Elements[i], Element(value, i), state);
                    }

                    break;
                case IFieldReferenceOperation field when TupleElementIndex(field) >= 0:
                    var parent = Evaluate(field.Instance!, state);
                    if (parent.Elements is { } elements)
                    {
                        var updated = (Value[]) elements.Clone();
                        updated[TupleElementIndex(field)] = value;
                        Assign(field.Instance!, new Value(elements: updated), state);
                    }

                    break;
                case IArrayElementReferenceOperation element:
                    var array = Evaluate(element.ArrayReference, state);
                    foreach (var index in element.Indices)
                    {
                        Evaluate(index, state);
                    }

                    if (array.Arrays is not { } identities)
                    {
                        break;
                    }

                    if (element.Indices.Length != 1
                        || element.Indices[0].ConstantValue is not { HasValue: true, Value: int arrayIndex }
                        || identities.Any(_summarizedArrays.Contains))
                    {
                        InvalidateArrays(array, state);
                        break;
                    }

                    foreach (var identity in identities)
                    {
                        if (state.Arrays.TryGetValue(identity, out var items) && arrayIndex >= 0 && arrayIndex < items.Length)
                        {
                            items[arrayIndex] = identities.Count == 1 && !array.UnknownArray ? value : items[arrayIndex].Merge(value);
                            // A safe write cannot remove an existing signal. Other writes
                            // require a still-safe element; the summary does not name one.
                            state.SetArraySafety(identity, items.Any(static item => item.Safe)
                                || (value.Safe && state.SafeArrays.Contains(identity)));
                        }
                    }

                    break;
                case IPropertyReferenceOperation { Property.IsIndexer: true, Instance: { } instance }:
                    InvalidateArrays(Evaluate(instance, state), state);
                    break;
                default:
                    // A stored array can be mutated through a field or another escape.
                    InvalidateArrays(value, state);
                    break;
            }
        }
    }
}
