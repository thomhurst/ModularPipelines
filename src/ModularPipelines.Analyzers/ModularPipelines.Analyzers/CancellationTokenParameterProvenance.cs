using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace ModularPipelines.Analyzers;

/// <summary>Tracks parameter values without changing the existing local-token analysis.</summary>
internal static class CancellationTokenParameterProvenance
{
    private static readonly ConditionalWeakTable<IOperation,
        ConcurrentDictionary<IParameterSymbol, Lazy<Dictionary<TextSpan, bool?>>>> Results = new();

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

        var references = Results.GetValue(root, static _ => new(SymbolEqualityComparer.Default))
            .GetOrAdd(source, token => new Lazy<Dictionary<TextSpan, bool?>>(() => Analyze(root, token))).Value;
        // Unsupported shapes keep the existing proof. This tracker adds no new helper,
        // collection, tuple, reference-storage, awaitable, loop or exception-cleanup proofs.
        return !references.TryGetValue(reference.Syntax.Span, out var value) || value != false;
    }

    private static Dictionary<TextSpan, bool?> Analyze(IOperation root, IParameterSymbol source)
    {
        var operations = root.DescendantsAndSelf().ToArray();
        if (!operations.OfType<ISimpleAssignmentOperation>().Any(assignment =>
                assignment.Target is IParameterReferenceOperation parameter
                && SymbolEqualityComparer.Default.Equals(parameter.Parameter, source))
            || operations.Any(static operation => operation is ITryOperation
                or ILoopOperation
                or IDeconstructionAssignmentOperation
                or ISimpleAssignmentOperation { IsRef: true }
                or ILocalReferenceOperation { Local.RefKind: not RefKind.None }
                or IVariableDeclaratorOperation { Symbol.RefKind: not RefKind.None }
                or IArgumentOperation { Parameter.RefKind: RefKind.Ref or RefKind.Out })
            || root.SemanticModel is not { } model)
        {
            return [];
        }

        try
        {
            return ControlFlowGraph.Create(root.Syntax, model) is { } graph
                ? new Analysis(graph, source).Run()
                : [];
        }
        catch (ArgumentException)
        {
            return [];
        }
    }

    // true means original, false means replaced on a reachable path; null marks
    // an unsupported value and leaves the pre-existing analysis responsible for it.
    private static bool? MergeSignals(bool? left, bool? right) =>
        left is null || right is null ? null : left.Value && right.Value;

    private sealed class State
    {
        public Dictionary<ISymbol, bool?> Values { get; } = [with(SymbolEqualityComparer.Default)];

        public Dictionary<CaptureId, bool?> Captures { get; } = [];

        public State Clone()
        {
            var copy = new State();
            foreach (var pair in Values)
            {
                copy.Values.Add(pair.Key, pair.Value);
            }

            foreach (var pair in Captures)
            {
                copy.Captures.Add(pair.Key, pair.Value);
            }

            return copy;
        }

        public bool Merge(State other)
        {
            return MergeValues(Values, other.Values) | MergeValues(Captures, other.Captures);
        }

        private static bool MergeValues<T>(Dictionary<T, bool?> values, Dictionary<T, bool?> incoming) where T : notnull
        {
            var changed = false;
            foreach (var key in values.Keys.Concat(incoming.Keys).Distinct().ToArray())
            {
                var left = values.TryGetValue(key, out var first) ? first : false;
                var right = incoming.TryGetValue(key, out var second) ? second : false;
                var merged = MergeSignals(left, right);
                if (!values.ContainsKey(key) || left != merged)
                {
                    values[key] = merged;
                    changed = true;
                }
            }

            return changed;
        }
    }

    private sealed class Analysis(ControlFlowGraph graph, IParameterSymbol source)
    {
        private readonly Dictionary<int, State> _inputs = [];
        private readonly Queue<BasicBlock> _pending = new();
        private readonly Dictionary<TextSpan, bool?> _results = [];
        private readonly Dictionary<CaptureId, ISymbol> _targets = [];

        public Dictionary<TextSpan, bool?> Run()
        {
            var initial = new State();
            initial.Values.Add(source, true);
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

                if (block.BranchValue?.ConstantValue is { HasValue: true, Value: bool condition }
                    && block.ConditionKind != ControlFlowConditionKind.None)
                {
                    Follow(condition == (block.ConditionKind == ControlFlowConditionKind.WhenTrue) ? block.ConditionalSuccessor : block.FallThroughSuccessor, state);
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

        private bool? Evaluate(IOperation operation, State state)
        {
            switch (operation)
            {
                case IParameterReferenceOperation parameter:
                    var value = Get(state.Values, parameter.Parameter);
                    var span = parameter.Syntax.Span;
                    _results[span] = _results.TryGetValue(span, out var previous)
                        ? MergeSignals(previous, value)
                        : value;
                    return value;
                case IDeclarationPatternOperation { DeclaredSymbol: { } declared }:
                    // Pattern bindings are outside the scalar assignment proofs. Keep
                    // their values unknown rather than treating an untracked local as replaced.
                    Set(state.Values, declared, null);
                    return null;
                case IRecursivePatternOperation { DeclaredSymbol: { } recursiveDeclared }:
                    Set(state.Values, recursiveDeclared, null);
                    foreach (var child in operation.ChildOperations)
                    {
                        Evaluate(child, state);
                    }

                    return null;
                case ILocalReferenceOperation local:
                    return Get(state.Values, local.Local);
                case IFlowCaptureReferenceOperation capture:
                    return Get(state.Captures, capture.Id);
                case IConversionOperation conversion:
                    var converted = Evaluate(conversion.Operand, state);
                    return conversion.Conversion.IsIdentity || conversion.Operand is IDefaultValueOperation ? converted : null;
                case IPropertyReferenceOperation { Property.Name: "Token", Instance: { } instance } property
                    when property.Property.ContainingType.ToDisplayString() == "System.Threading.CancellationTokenSource":
                    return Evaluate(instance, state);
                case IFlowCaptureOperation capture:
                    if (GetTarget(capture.Value) is { } target)
                    {
                        _targets[capture.Id] = target;
                    }

                    Set(state.Captures, capture.Id, Evaluate(capture.Value, state));
                    return state.Captures[capture.Id];
                case ISimpleAssignmentOperation assignment:
                    var assigned = Evaluate(assignment.Value, state);
                    if (GetTarget(assignment.Target) is { } assignedTarget)
                    {
                        Set(state.Values, assignedTarget, assigned);
                    }

                    return assigned;
                case IArgumentOperation argument:
                    return Evaluate(argument.Value, state);
                case IAnonymousFunctionOperation or IFlowAnonymousFunctionOperation or ILocalFunctionOperation:
                    return false;
                default:
                    foreach (var child in operation.ChildOperations)
                    {
                        Evaluate(child, state);
                    }

                    return operation is IDefaultValueOperation or IObjectCreationOperation || operation.ConstantValue.HasValue || operation is IPropertyReferenceOperation { Property.Name: "None", Property.ContainingType.Name: "CancellationToken" } ? false : null;
            }
        }

        private ISymbol? GetTarget(IOperation operation) => operation switch
        {
            IParameterReferenceOperation parameter => parameter.Parameter,
            ILocalReferenceOperation local => local.Local,
            IFlowCaptureReferenceOperation capture => _targets.TryGetValue(capture.Id, out var target) ? target : null,
            _ => null,
        };

        private static bool? Get<T>(Dictionary<T, bool?> values, T key) where T : notnull =>
            values.TryGetValue(key, out var value) ? value : false;

        private static void Set<T>(Dictionary<T, bool?> values, T key, bool? present) where T : notnull =>
            values[key] = present;
    }
}
