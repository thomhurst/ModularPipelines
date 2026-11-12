using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace ModularPipelines.Analyzers;

/// <summary>Tracks bounded caller-owned collections, tuple storage and reference escapes.</summary>
internal static class CancellationTokenCollectionProvenance
{
    public static bool RequiresTracking(IOperation operation) => operation is ITupleOperation
        or IDeconstructionAssignmentOperation
        or ISimpleAssignmentOperation { IsRef: true }
        or IArgumentOperation { Parameter.RefKind: RefKind.Ref or RefKind.Out }
        or ILocalReferenceOperation { Local.RefKind: not RefKind.None }
        || operation.Type is INamedTypeSymbol { IsTupleType: true }
        || IsTokenCollection(operation.Type);

    private static bool IsTokenCollection(ITypeSymbol? type) => type switch
    {
        IArrayTypeSymbol { Rank: 1 } array => array.ElementType.ToDisplayString() == "System.Threading.CancellationToken",
        INamedTypeSymbol { TypeArguments.Length: 1 } named => named.TypeArguments[0].ToDisplayString() == "System.Threading.CancellationToken"
            && named.OriginalDefinition.ToDisplayString() is "System.Span<T>" or "System.ReadOnlySpan<T>",
        _ => false,
    };

    public static Dictionary<TextSpan, bool?> Analyze(ControlFlowGraph graph, IParameterSymbol source) =>
        new Analysis(graph, source).Run();

    // Facts are bounded by symbols, CFG captures, syntax allocation sites, literal
    // indices and statically typed tuple paths. Loops/exception cleanup stay on the
    // existing analysis path; no operation can add heap-wide alias identities.
    // Values carry a scalar signal, tuple elements or array allocation identities.
    // Escaping collections lose their signal; joins retain only guaranteed signals.
    private sealed class Value(bool safe = false, Value[]? elements = null, HashSet<TextSpan>? arrays = null, bool unknownArray = false, bool sliced = false, bool unsupported = false)
    {
        public static readonly Value None = new();

        public static readonly Value Original = new(safe: true);

        public static readonly Value Unknown = new(unsupported: true);

        public bool Unsupported { get; } = unsupported;

        public bool Safe { get; } = safe;

        public Value[]? Elements { get; } = elements;

        public HashSet<TextSpan>? Arrays { get; } = arrays;

        public bool UnknownArray { get; } = unknownArray;

        public bool Sliced { get; } = sliced;

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

            return MergeScalarOrArray(other);
        }

        private Value MergeScalarOrArray(Value other)
        {
            if (Arrays is { } leftArrays && other.Arrays is { } rightArrays)
            {
                return new Value(arrays: [.. leftArrays, .. rightArrays], unknownArray: UnknownArray || other.UnknownArray, sliced: Sliced || other.Sliced);
            }

            if (Arrays is { } knownArrays)
            {
                return new Value(arrays: knownArrays, unknownArray: true, sliced: Sliced);
            }

            if (other.Arrays is not null)
            {
                return other.Merge(this);
            }

            if (Unsupported || other.Unsupported)
            {
                return Unknown;
            }

            if (Safe && other.Safe)
            {
                return Original;
            }

            return None;
        }

        public bool SameAs(Value other) => Unsupported == other.Unsupported && Safe == other.Safe && UnknownArray == other.UnknownArray && Sliced == other.Sliced
            && ((Elements is null && other.Elements is null)
                || (Elements is { } left && other.Elements is { } right && left.Length == right.Length
                    && left.Select((element, index) => element.SameAs(right[index])).All(static same => same)))
            && ((Arrays is null && other.Arrays is null)
                || (Arrays is { } leftArrays && other.Arrays is { } rightArrays && leftArrays.SetEquals(rightArrays)));
    }

    private readonly struct StorageReference(ISymbol? symbol, string path) : IEquatable<StorageReference>
    {
        public ISymbol? Symbol { get; } = symbol;

        public string Path { get; } = path;

        public bool Equals(StorageReference other) => SymbolEqualityComparer.Default.Equals(Symbol, other.Symbol) && Path == other.Path;

        public override bool Equals(object? obj) => obj is StorageReference other && Equals(other);

        public override int GetHashCode() => unchecked(((Symbol is null ? 0 : SymbolEqualityComparer.Default.GetHashCode(Symbol)) * 397) ^ StringComparer.Ordinal.GetHashCode(Path));
    }

    private sealed class State
    {
        public Dictionary<ISymbol, Value> Values { get; } = [with(SymbolEqualityComparer.Default)];

        public Dictionary<CaptureId, Value> Captures { get; } = [];

        public HashSet<ISymbol> EscapedSymbols { get; } = [with(SymbolEqualityComparer.Default)];

        public Dictionary<ISymbol, HashSet<string>> EscapedFields { get; } = [with(SymbolEqualityComparer.Default)];

        public HashSet<CaptureId> EscapedCaptures { get; } = [];

        public Dictionary<CaptureId, HashSet<StorageReference>> CaptureTargets { get; } = [];

        public Dictionary<ISymbol, HashSet<StorageReference>> ReferenceTargets { get; } = [with(SymbolEqualityComparer.Default)];

        // Only syntax-named indices need facts, even for very large allocations.
        public Dictionary<TextSpan, Dictionary<int, Value>> Arrays { get; } = [];

        public HashSet<TextSpan> SafeArrays { get; } = [];

        public void SetValue(ISymbol symbol, Value value) =>
            Values[symbol] = EscapedSymbols.Contains(symbol) ? Value.None : value;

        public void SetCapture(CaptureId id, Value value) =>
            Captures[id] = EscapedCaptures.Contains(id) ? Value.None : value;

        public void SetCaptureTargets(CaptureId id, IEnumerable<StorageReference> targets)
        {
            CaptureTargets[id] = [.. targets];
            if (EscapedCaptures.Contains(id))
            {
                Escape(id);
            }
        }

        public void Escape(ISymbol symbol)
        {
            EscapedSymbols.Add(symbol);
            SetValue(symbol, Values.TryGetValue(symbol, out var value) ? value : Value.None);
        }

        public void Escape(ISymbol? symbol, string path)
        {
            if (symbol is null)
            {
                return;
            }

            if (path.Length == 0)
            {
                Escape(symbol);
                return;
            }

            if (!EscapedFields.TryGetValue(symbol, out var paths))
            {
                EscapedFields.Add(symbol, paths = [with(StringComparer.Ordinal)]);
            }

            paths.Add(path);
        }

        public bool IsEscaped(StorageReference reference) => reference.Symbol is null || EscapedSymbols.Contains(reference.Symbol)
            || (EscapedFields.TryGetValue(reference.Symbol, out var paths)
                && paths.Any(path => reference.Path == path || reference.Path.StartsWith(path + "/", StringComparison.Ordinal)));

        public void Escape(CaptureId id)
        {
            EscapedCaptures.Add(id);
            SetCapture(id, Captures.TryGetValue(id, out var value) ? value : Value.None);
            if (CaptureTargets.TryGetValue(id, out var targets))
            {
                foreach (var target in targets)
                {
                    Escape(target.Symbol, target.Path);
                }
            }
        }

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
                copy.Arrays.Add(entry.Key, [with(entry.Value)]);
            }

            copy.SafeArrays.UnionWith(SafeArrays);
            copy.EscapedSymbols.UnionWith(EscapedSymbols);
            copy.EscapedCaptures.UnionWith(EscapedCaptures);
            foreach (var entry in EscapedFields)
            {
                copy.EscapedFields.Add(entry.Key, [with(StringComparer.Ordinal), .. entry.Value]);
            }

            foreach (var entry in CaptureTargets)
            {
                copy.CaptureTargets.Add(entry.Key, [.. entry.Value]);
            }

            foreach (var entry in ReferenceTargets)
            {
                copy.ReferenceTargets.Add(entry.Key, [.. entry.Value]);
            }

            return copy;
        }

        public bool Merge(State other)
        {
            var changed = MergeCaptureTargets(other) | MergeReferenceTargets(other) | MergeEscapedFields(other);
            var escapedCount = EscapedSymbols.Count + EscapedCaptures.Count;
            EscapedSymbols.UnionWith(other.EscapedSymbols);
            EscapedCaptures.UnionWith(other.EscapedCaptures);
            ApplyCaptureEscapes();
            changed |= escapedCount != EscapedSymbols.Count + EscapedCaptures.Count;
            changed |= MergeValues(Values, other.Values, EscapedSymbols) | MergeValues(Captures, other.Captures, EscapedCaptures);
            // An allocation absent from one path has no aliases on that path.
            foreach (var entry in other.Arrays)
            {
                var span = entry.Key;
                var elements = entry.Value;
                if (!Arrays.TryGetValue(span, out var current))
                {
                    Arrays.Add(span, [with(elements)]);
                    SetArraySafety(span, other.SafeArrays.Contains(span));
                    changed = true;
                    continue;
                }

                var safe = SafeArrays.Contains(span) && other.SafeArrays.Contains(span);
                changed |= safe != SafeArrays.Contains(span);
                SetArraySafety(span, safe);

                foreach (var index in current.Keys.ToArray())
                {
                    var merged = current[index].Merge(GetElement(elements, index));
                    changed |= !merged.SameAs(current[index]);
                    current[index] = merged;
                }
            }

            return changed;
        }

        private bool MergeCaptureTargets(State other)
        {
            var changed = false;
            foreach (var entry in other.CaptureTargets)
            {
                if (!CaptureTargets.TryGetValue(entry.Key, out var targets))
                {
                    targets = [];
                    CaptureTargets.Add(entry.Key, targets);
                }

                var count = targets.Count;
                targets.UnionWith(entry.Value);
                changed |= targets.Count != count;
            }

            return changed;
        }

        private bool MergeReferenceTargets(State other)
        {
            var changed = false;
            foreach (var entry in other.ReferenceTargets)
            {
                if (!ReferenceTargets.TryGetValue(entry.Key, out var targets))
                {
                    ReferenceTargets.Add(entry.Key, targets = []);
                }

                var count = targets.Count;
                targets.UnionWith(entry.Value);
                changed |= targets.Count != count;
            }

            return changed;
        }

        private bool MergeEscapedFields(State other)
        {
            var changed = false;
            foreach (var entry in other.EscapedFields)
            {
                if (!EscapedFields.TryGetValue(entry.Key, out var paths))
                {
                    EscapedFields.Add(entry.Key, [with(StringComparer.Ordinal), .. entry.Value]);
                    changed = true;
                    continue;
                }

                var count = paths.Count;
                paths.UnionWith(entry.Value);
                changed |= paths.Count != count;
            }

            return changed;
        }

        private void ApplyCaptureEscapes()
        {
            // A join can add targets to a capture that already escaped.
            foreach (var capture in EscapedCaptures)
            {
                Escape(capture);
            }
        }

        private static bool MergeValues<TKey>(Dictionary<TKey, Value> values, Dictionary<TKey, Value> other, HashSet<TKey> escaped)
            where TKey : notnull
        {
            var changed = false;
            foreach (var key in values.Keys.ToArray())
            {
                var merged = values[key].Merge(other.TryGetValue(key, out var value) ? value : Value.None);
                if (escaped.Contains(key))
                {
                    merged = Value.None;
                }

                changed |= !merged.SameAs(values[key]);
                values[key] = merged;
            }

            return changed;
        }

        public static Value GetElement(Dictionary<int, Value> elements, int index) =>
            elements.TryGetValue(index, out var value) ? value : Value.None;
    }

    private sealed class Analysis(ControlFlowGraph graph, IParameterSymbol source)
    {
        private readonly Dictionary<int, State> _inputs = [];
        private readonly Queue<BasicBlock> _pending = new();
        private readonly Dictionary<TextSpan, bool?> _results = [];
        private readonly HashSet<TextSpan> _summarizedArrays = [];

        public Dictionary<TextSpan, bool?> Run()
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
            return operation switch
            {
                IParameterReferenceOperation parameter => EvaluateParameter(parameter, state),
                ILocalReferenceOperation local => GetSymbolValue(local.Local, state),
                IFlowCaptureReferenceOperation capture => Get(state.Captures, capture.Id),
                IConversionOperation conversion => EvaluateConversion(conversion, state),
                IPropertyReferenceOperation { Property.Name: "Token", Instance: { } instance } property
                                    when property.Property.ContainingType.ToDisplayString() == "System.Threading.CancellationTokenSource" => Evaluate(instance, state),
                ITupleOperation tuple => new Value(elements: [.. tuple.Elements.Select(element => Evaluate(element, state))]),
                IFieldReferenceOperation field when TupleElementIndex(field) >= 0 => Element(Evaluate(field.Instance!, state), TupleElementIndex(field)),
                _ => EvaluateCollectionOrEffects(operation, state),
            };
        }

        private Value EvaluateCollectionOrEffects(IOperation operation, State state)
        {
            return operation switch
            {
                IArrayCreationOperation array => EvaluateArray(array, state),
                IObjectCreationOperation creation => EvaluateCreation(creation, state),
                ICollectionExpressionOperation collection => EvaluateCollection(collection, state),
                IArrayElementReferenceOperation element => EvaluateArrayElement(element, state),
                IPropertyReferenceOperation { Property.IsIndexer: true, Instance: { } receiver } indexer
                    when IsTokenCollection(receiver.Type) => EvaluateCollectionElement(receiver, [.. indexer.Arguments.Select(argument => argument.Value)], state),
                _ => EvaluateEffects(operation, state),
            };
        }

        private Value EvaluateParameter(IParameterReferenceOperation parameter, State state)
        {
            var value = GetSymbolValue(parameter.Parameter, state);
            bool? safe = value.Unsupported ? null : IsSafe(value, state);
            var span = parameter.Syntax.Span;
            _results[span] = _results.TryGetValue(span, out var previous)
                ? safe is null || previous is null ? null : safe.Value && previous.Value
                : safe;

            return value;
        }

        private Value EvaluateConversion(IConversionOperation conversion, State state)
        {
            var converted = Evaluate(conversion.Operand, state);
            if (conversion.Operand.Type is IArrayTypeSymbol && IsTokenCollection(conversion.Operand.Type)
                && conversion.Type is INamedTypeSymbol && IsTokenCollection(conversion.Type)
                && conversion.OperatorMethod?.ContainingType.OriginalDefinition.ToDisplayString() is "System.Span<T>" or "System.ReadOnlySpan<T>")
            {
                return converted;
            }

            if (conversion.OperatorMethod is not null)
            {
                InvalidateArrays(converted, state);
                return Value.None;
            }

            if (converted.Elements is null || conversion.Type is INamedTypeSymbol { IsTupleType: true })
            {
                return converted;
            }

            InvalidateArrays(converted, state);
            return Value.None;
        }

        private Value EvaluateCreation(IObjectCreationOperation creation, State state)
        {
            var arguments = creation.Arguments.Select(argument => (Argument: argument, Value: Evaluate(argument.Value, state))).ToArray();
            if (IsTokenCollection(creation.Type) && creation.Arguments.Length is 1 or 3
                && creation.Arguments[0].Parameter?.Type is IArrayTypeSymbol)
            {
                // Slice construction does not mutate the backing array. Its offsets
                // are not tracked, so only full-array views share element facts.
                return arguments.Length == 1 ? arguments[0].Value : new Value(arrays: arguments[0].Value.Arrays, unknownArray: true, sliced: true);
            }

            // Constructors cannot mutate arguments until all values are evaluated.
            foreach (var (argument, value) in arguments)
            {
                InvalidateArrays(value, state);
                if (argument.Parameter?.RefKind is RefKind.Ref or RefKind.Out)
                {
                    if (creation.Type?.IsRefLikeType == true)
                    {
                        InvalidateReferencedStorage(argument.Value, state);
                    }
                    Assign(argument.Value, Value.None, state);
                }
            }

            if (creation.Initializer is { } initializer)
            {
                Evaluate(initializer, state);
            }

            return Value.None;
        }

        private Value EvaluateArray(IArrayCreationOperation array, State state)
        {
            foreach (var dimension in array.DimensionSizes)
            {
                Evaluate(dimension, state);
            }

            var arrayElements = array.Initializer is { } initializer
                ? initializer.ElementValues.Select(element => Evaluate(element, state)).ToArray()
                : [];
            if (IsTokenCollection(array.Type))
            {
                return CreateArray(array.Syntax.Span, arrayElements, state);
            }

            foreach (var arrayElement in arrayElements)
            {
                InvalidateArrays(arrayElement, state);
            }

            return Value.None;
        }

        private Value EvaluateCollection(ICollectionExpressionOperation collection, State state)
        {
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
        }

        private Value EvaluateArrayElement(IArrayElementReferenceOperation element, State state)
            => EvaluateCollectionElement(element.ArrayReference, element.Indices, state);

        private Value EvaluateCollectionElement(IOperation receiver, IReadOnlyList<IOperation> indices, State state)
        {
            var arrayValue = Evaluate(receiver, state);
            foreach (var index in indices)
            {
                Evaluate(index, state);
            }

            if (indices.Count == 1 && indices[0].ConstantValue is { HasValue: true, Value: int arrayIndex }
                && arrayValue.Arrays is { } arrayIdentities && !arrayIdentities.Any(_summarizedArrays.Contains))
            {
                return ArrayElement(arrayValue, arrayIndex, state);
            }

            return Value.None;
        }

        private Value EvaluateLinkedSource(IInvocationOperation invocation, State state)
        {
            var linked = false;
            foreach (var argument in invocation.Arguments)
            {
                linked |= IsSafe(Evaluate(argument.Value, state), state);
            }

            return linked ? Value.Original : Value.None;
        }

        private Value EvaluateInvocation(IInvocationOperation invocation, State state)
        {
            if (invocation.TargetMethod.Name == "CreateLinkedTokenSource"
                && invocation.TargetMethod.ContainingType.ToDisplayString() == "System.Threading.CancellationTokenSource")
            {
                return EvaluateLinkedSource(invocation, state);
            }

            var values = EvaluateInvocationArguments(invocation, state);
            if (invocation.TargetMethod is { Name: "AsSpan", IsStatic: true } asSpan
                && asSpan.ContainingType.ToDisplayString() == "System.MemoryExtensions"
                && invocation.Arguments.Length > 0
                && invocation.Arguments[0].Parameter?.Type is IArrayTypeSymbol
                && IsTokenCollection(invocation.Type))
            {
                var backing = Get(values, invocation.Arguments[0].Value.Syntax.Span);
                // Full-array views retain identity and index facts. Slices retain
                // aliases for invalidation, but cannot prove offsets or contents.
                return invocation.Arguments.Length == 1
                    ? backing
                    : new Value(arrays: backing.Arrays, unknownArray: true, sliced: true);
            }

            if (invocation.TargetMethod is { Name: "KeepAlive", IsStatic: true, Parameters.Length: 1 } keepAlive
                && keepAlive.ContainingType.ToDisplayString() == "System.GC")
            {
                return Value.None;
            }

            foreach (var value in values.Values)
            {
                InvalidateArrays(value, state);
            }

            // Source-helper and awaitable payload proofs remain with the existing
            // analysis. Unknown scalar results must not become replacement proofs.
            return IsUnsupportedScalar(invocation) ? Value.Unknown : Value.None;
        }

        private Dictionary<TextSpan, Value> EvaluateInvocationArguments(IInvocationOperation invocation, State state)
        {
            var values = new Dictionary<TextSpan, Value>();
            var references = new List<(IOperation Target, Value Value)>();
            if (invocation.Instance is { } receiver)
            {
                var value = Evaluate(receiver, state);
                values[receiver.Syntax.Span] = value;
            }

            foreach (var argument in invocation.Arguments)
            {
                var value = Evaluate(argument.Value, state);
                if (argument.Parameter?.RefKind is RefKind.Ref or RefKind.Out)
                {
                    references.Add((argument.Value, value));
                    value = Value.None;
                }

                values[argument.Value.Syntax.Span] = value;
            }

            // A call cannot write ref/out targets until every argument is evaluated.
            foreach (var (target, value) in references)
            {
                InvalidateArrays(value, state);
                if (invocation.TargetMethod.ReturnsByRef || invocation.TargetMethod.ReturnsByRefReadonly
                    || invocation.Type?.IsRefLikeType == true)
                {
                    InvalidateReferencedStorage(target, state);
                }
                Assign(target, Value.None, state);
            }

            return values;
        }

        private Value EvaluateEffects(IOperation operation, State state)
        {
            switch (operation)
            {
                case IDeclarationPatternOperation { DeclaredSymbol: { } symbol }:
                    state.SetValue(symbol, Value.Unknown);
                    return Value.Unknown;
                case IRecursivePatternOperation { DeclaredSymbol: { } symbol }:
                    state.SetValue(symbol, Value.Unknown);
                    foreach (var child in operation.ChildOperations)
                    {
                        Evaluate(child, state);
                    }

                    return Value.Unknown;
                case IInvocationOperation invocation:
                    return EvaluateInvocation(invocation, state);
                case IDeconstructionAssignmentOperation deconstruction:
                    var tuple = Evaluate(deconstruction.Value, state);
                    Assign(deconstruction.Target, tuple, state);
                    return tuple;
                case IFlowCaptureOperation capture:
                    var captured = Evaluate(capture.Value, state);
                    state.SetCapture(capture.Id, captured);
                    state.SetCaptureTargets(capture.Id, ReferencedStorage(capture.Value, state));
                    return captured;
                case ISimpleAssignmentOperation assignment:
                    return EvaluateAssignment(assignment, state);
                case IArgumentOperation argument when argument.Parameter?.RefKind is RefKind.Ref or RefKind.Out:
                    InvalidateArrays(Evaluate(argument.Value, state), state);
                    if (argument.Parent is IInvocationOperation referenceCall
                        && (referenceCall.TargetMethod.ReturnsByRef || referenceCall.TargetMethod.ReturnsByRefReadonly
                            || referenceCall.Type?.IsRefLikeType == true))
                    {
                        InvalidateReferencedStorage(argument.Value, state);
                    }
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

                    return IsUnsupportedScalar(operation) ? Value.Unknown : Value.None;
            }
        }

        private static bool IsUnsupportedScalar(IOperation operation) =>
            operation.Type?.ToDisplayString() is "System.Threading.CancellationToken" or "System.Threading.CancellationTokenSource"
            && operation is not IDefaultValueOperation
                and not IObjectCreationOperation
                and not IPropertyReferenceOperation { Property.Name: "None", Property.ContainingType.Name: "CancellationToken" };

        private Value EvaluateAssignment(ISimpleAssignmentOperation assignment, State state)
        {
            // Snapshot a collection receiver before the RHS can replace its local.
            var assign = PrepareAssignment(assignment.Target, state);
            var assigned = Evaluate(assignment.Value, state);
            if (assignment.IsRef)
            {
                // Even a readonly alias can observe later writes to its source.
                if (assignment.Target is ILocalReferenceOperation referenceLocal)
                {
                    state.ReferenceTargets[referenceLocal.Local] = [.. ReferencedStorage(assignment.Value, state)];
                    state.Escape(referenceLocal.Local);
                }
                else
                {
                    InvalidateReferencedStorage(assignment.Target, state);
                }

                if (assignment.Target is not ILocalReferenceOperation local || local.Local.RefKind == RefKind.Ref)
                {
                    InvalidateReferenceTarget(assignment.Value, state);
                }
            }

            assign(assigned);
            return assigned;
        }

        private Action<Value> PrepareAssignment(IOperation target, State state)
        {
            var (receiver, indices) = target switch
            {
                IArrayElementReferenceOperation element => (element.ArrayReference, element.Indices.ToArray()),
                IPropertyReferenceOperation { Property.IsIndexer: true, Instance: { } instance } indexer
                    when IsTokenCollection(instance.Type) => (instance, indexer.Arguments.Select(argument => argument.Value).ToArray()),
                _ => ((IOperation?) null, []),
            };
            if (receiver is null)
            {
                if (target is IPropertyReferenceOperation property)
                {
                    var instance = property.Instance is null ? Value.None : Evaluate(property.Instance, state);
                    foreach (var argument in property.Arguments)
                    {
                        Evaluate(argument.Value, state);
                    }

                    return value =>
                    {
                        InvalidateArrays(instance, state);
                        InvalidateArrays(value, state);
                    };
                }

                return value => Assign(target, value, state);
            }

            var array = Evaluate(receiver, state);
            foreach (var index in indices)
            {
                Evaluate(index, state);
            }

            return value => WriteCollectionElement(receiver, indices, array, value, state);
        }

        private void InvalidateReferenceTarget(IOperation target, State state)
        {
            // A writable reference can outlive this operation and mutate storage
            // through a local alias that scalar provenance does not track.
            switch (target)
            {
                case ILocalReferenceOperation local:
                    InvalidateArrays(Get(state.Values, local.Local), state);
                    break;
                case IParameterReferenceOperation parameter:
                    InvalidateArrays(Get(state.Values, parameter.Parameter), state);
                    break;
                case IFlowCaptureReferenceOperation capture:
                    InvalidateArrays(Get(state.Captures, capture.Id), state);
                    break;
                case IFieldReferenceOperation field when TupleElementIndex(field) >= 0:
                    InvalidateArrays(Evaluate(field, state), state);
                    break;
                case IArrayElementReferenceOperation element:
                    InvalidateArrays(Evaluate(element.ArrayReference, state), state);
                    InvalidateUnknownReferenceArrays(element.ArrayReference, state);
                    break;
                case IPropertyReferenceOperation { Property.IsIndexer: true, Instance: { } receiver }
                    when IsTokenCollection(receiver.Type):
                    InvalidateArrays(Evaluate(receiver, state), state);
                    InvalidateUnknownReferenceArrays(receiver, state);
                    break;
            }

            InvalidateReferencedStorage(target, state);
        }

        private static void InvalidateReferencedStorage(IOperation target, State state)
        {
            foreach (var reference in ReferencedStorage(target, state))
            {
                state.Escape(reference.Symbol, reference.Path);
            }

            if (target is IFlowCaptureReferenceOperation capture)
            {
                state.Escape(capture.Id);
            }
        }

        private static IEnumerable<StorageReference> ReferencedStorage(IOperation target, State state) => target switch
        {
            ILocalReferenceOperation local when state.ReferenceTargets.TryGetValue(local.Local, out var targets) => targets,
            ILocalReferenceOperation { Local.RefKind: not RefKind.None } => [new StorageReference(null, string.Empty)],
            ILocalReferenceOperation local => [new StorageReference(local.Local, string.Empty)],
            IParameterReferenceOperation parameter => [new StorageReference(parameter.Parameter, string.Empty)],
            IFlowCaptureReferenceOperation capture when state.CaptureTargets.TryGetValue(capture.Id, out var targets) => targets,
            IFieldReferenceOperation field when TupleElementIndex(field) >= 0 => ReferencedStorage(field.Instance!, state)
                .Select(reference => new StorageReference(reference.Symbol, reference.Path + "/" + TupleElementIndex(field))),
            IConversionOperation conversion => ReferencedStorage(conversion.Operand, state),
            IDeclarationExpressionOperation declaration => ReferencedStorage(declaration.Expression, state),
            // An unknown byref return must remain a target alternative at joins.
            // An empty set would let another branch's known targets erase it.
            _ => [new StorageReference(null, string.Empty)],
        };

        private static void InvalidateUnknownReferenceArrays(IOperation receiver, State state)
        {
            var references = ReferencedStorage(receiver, state).ToArray();
            var escaped = references.Any(state.IsEscaped)
                || (receiver is ILocalReferenceOperation local && state.EscapedSymbols.Contains(local.Local))
                || (receiver is IFlowCaptureReferenceOperation capture && state.EscapedCaptures.Contains(capture.Id));
            if (!escaped)
            {
                return;
            }

            // Read current storage, since an alias can outlive array replacement.
            var possible = references.Select(reference => GetStorageValue(reference, state)).ToArray();
            if (references.Length > 0 && !references.Any(state.IsEscaped)
                && possible.All(static value => value.Arrays is not null))
            {
                foreach (var value in possible)
                {
                    InvalidateArrays(value, state);
                }
            }
            else
            {
                InvalidateAllArrays(state);
            }
        }

        private static void InvalidateAllArrays(State state)
        {
            // Unknown storage can alias any tracked allocation, including storage
            // assigned after a ref was created. No heap-wide alias proof is made.
            foreach (var identity in state.Arrays.Keys.ToArray())
            {
                state.Arrays[identity] = [];
            }

            state.SafeArrays.Clear();
        }

        private static Value GetStorageValue(StorageReference reference, State state)
        {
            if (reference.Symbol is null)
            {
                return Value.None;
            }

            var value = Get(state.Values, reference.Symbol);
            foreach (var index in reference.Path.Split(['/'], StringSplitOptions.RemoveEmptyEntries))
            {
                value = Element(value, int.Parse(index, System.Globalization.CultureInfo.InvariantCulture));
            }

            return value;
        }

        private static Value GetSymbolValue(ISymbol symbol, State state)
        {
            if (symbol is ILocalSymbol { RefKind: RefKind.RefReadOnly }
                && state.ReferenceTargets.TryGetValue(symbol, out var targets))
            {
                Value? referenced = null;
                foreach (var target in targets)
                {
                    var current = state.IsEscaped(target) ? Value.None : GetStorageValue(target, state);
                    if (target.Symbol is not null && state.EscapedFields.TryGetValue(target.Symbol, out var escapedPaths))
                    {
                        current = MaskEscapedFields(current, escapedPaths, target.Path);
                    }

                    referenced = referenced is null ? current : referenced.Merge(current);
                }

                return referenced ?? Value.Unknown;
            }

            if (state.EscapedSymbols.Contains(symbol))
            {
                return Value.None;
            }

            var value = Get(state.Values, symbol);
            return state.EscapedFields.TryGetValue(symbol, out var paths)
                ? MaskEscapedFields(value, paths, string.Empty)
                : value;
        }

        private static Value MaskEscapedFields(Value value, HashSet<string> paths, string path)
        {
            if (paths.Contains(path))
            {
                return Value.None;
            }

            return value.Elements is { } elements
                ? new Value(elements: [.. elements.Select((element, index) => MaskEscapedFields(element, paths, path + "/" + index))])
                : value;
        }

        private static Value Get<TKey>(Dictionary<TKey, Value> values, TKey key)
            where TKey : notnull => values.TryGetValue(key, out var value) ? value : Value.None;

        private static bool IsSafe(Value value, State state) => value.Safe
            || (!value.UnknownArray && value.Arrays is { Count: > 0 } arrays
                && arrays.All(state.SafeArrays.Contains));

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
            var values = elements.Select((value, index) => (value, index))
                .ToDictionary(static element => element.index, static element => element.value);
            var safe = values.Values.Any(static value => value.Safe);
            if (state.Arrays.TryGetValue(identity, out var previous))
            {
                // Aliases can still refer to an earlier allocation at this site.
                safe &= state.SafeArrays.Contains(identity);
                foreach (var index in values.Keys.ToArray())
                {
                    values[index] = values[index].Merge(State.GetElement(previous, index));
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
                if (!state.Arrays.TryGetValue(span, out var elements) || index < 0)
                {
                    return Value.None;
                }

                var element = State.GetElement(elements, index);
                result = result is null ? element : result.Merge(element);
            }

            return result ?? Value.None;
        }

        private static void InvalidateArrays(Value value, State state)
        {
            if (value.Arrays is { } arrays)
            {
                foreach (var span in arrays)
                {
                    if (state.Arrays.ContainsKey(span))
                    {
                        state.Arrays[span] = [];
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

        private void AssignArrayElement(IArrayElementReferenceOperation element, Value value, State state)
            => AssignCollectionElement(element.ArrayReference, element.Indices, value, state);

        private void AssignCollectionElement(IOperation receiver, IReadOnlyList<IOperation> indices, Value value, State state)
        {
            var array = Evaluate(receiver, state);
            foreach (var index in indices)
            {
                Evaluate(index, state);
            }

            WriteCollectionElement(receiver, indices, array, value, state);
        }

        private void WriteCollectionElement(IOperation receiver, IReadOnlyList<IOperation> indices, Value array, Value value, State state)
        {
            if (array.UnknownArray)
            {
                InvalidateUnknownReferenceArrays(receiver, state);
            }

            if (array.Arrays is not { } identities)
            {
                InvalidateUnknownReferenceArrays(receiver, state);
                return;
            }

            if (array.Sliced || indices.Count != 1
                || indices[0].ConstantValue is not { HasValue: true, Value: int arrayIndex }
                || identities.Any(_summarizedArrays.Contains))
            {
                InvalidateArrays(array, state);
                return;
            }

            foreach (var identity in identities)
            {
                if (state.Arrays.TryGetValue(identity, out var items) && arrayIndex >= 0)
                {
                    UpdateArrayElement(array, identities.Count, identity, items, arrayIndex, value, state);
                }
            }
        }

        private static void UpdateArrayElement(
            Value array, int aliasCount, TextSpan identity, Dictionary<int, Value> items, int arrayIndex, Value value, State state)
        {
            items[arrayIndex] = aliasCount == 1 && !array.UnknownArray ? value : State.GetElement(items, arrayIndex).Merge(value);
            // A safe write cannot remove an existing signal. Other writes
            // require a still-safe element; the summary does not name one.
            state.SetArraySafety(identity, items.Values.Any(static item => item.Safe)
                || (value.Safe && state.SafeArrays.Contains(identity)));
        }

        private void Assign(IOperation target, Value value, State state)
        {
            switch (target)
            {
                case IPropertyReferenceOperation { Property.IsIndexer: true, Instance: { } receiver } indexer
                    when IsTokenCollection(receiver.Type):
                    AssignCollectionElement(receiver, [.. indexer.Arguments.Select(argument => argument.Value)], value, state);
                    break;
                case IParameterReferenceOperation parameter:
                    state.SetValue(parameter.Parameter, value);
                    break;
                case ILocalReferenceOperation local:
                    state.SetValue(local.Local, value);
                    break;
                case IFlowCaptureReferenceOperation capture:
                    state.SetCapture(capture.Id, value);
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
                    // Default tuple values have no element facts until a field is written.
                    var updated = parent.Elements is { } elements
                        ? (Value[]) elements.Clone()
                        : [.. Enumerable.Repeat(Value.None, ((INamedTypeSymbol) field.Instance!.Type!).TupleElements.Length)];
                    updated[TupleElementIndex(field)] = value;
                    Assign(field.Instance!, new Value(elements: updated), state);

                    break;
                case IArrayElementReferenceOperation element:
                    AssignArrayElement(element, value, state);
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
