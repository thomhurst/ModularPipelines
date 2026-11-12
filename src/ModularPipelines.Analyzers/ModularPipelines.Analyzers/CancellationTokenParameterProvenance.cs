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
            var callable = GetCallableSymbol(parent);
            if (callable is not null)
            {
                if (!SymbolEqualityComparer.Default.Equals(callable, source.ContainingSymbol))
                {
                    return true;
                }

                root = parent;
                break;
            }

            root = parent;
        }

        var references = Results.GetValue(root, static _ => new(SymbolEqualityComparer.Default))
            .GetOrAdd(source, token => new Lazy<Dictionary<TextSpan, bool?>>(() => Analyze(root, token))).Value;
        // Unsupported shapes keep the existing proof. Helper, awaitable, loop and
        // exception-cleanup proofs remain on their existing paths.
        return !references.TryGetValue(reference.Syntax.Span, out var value) || value != false;
    }

    private static Dictionary<TextSpan, bool?> Analyze(IOperation root, IParameterSymbol source)
    {
        var operations = root.DescendantsAndSelf().ToArray();
        var collections = operations.Any(CancellationTokenCollectionProvenance.RequiresTracking);
        if ((!collections && !operations.OfType<ISimpleAssignmentOperation>().Any(assignment =>
                assignment.Target is IParameterReferenceOperation parameter
                && SymbolEqualityComparer.Default.Equals(parameter.Parameter, source)))
            || operations.Any(static operation => operation is ITryOperation or ILoopOperation)
            || operations.Any(HasCapturedWrite)
            // Nested functions retain the dedicated captured-helper analysis.
            // Their ref/deconstruction effects are not caller-owned storage.
            || (collections && operations.Any(operation =>
                operation is IAnonymousFunctionOperation or ILocalFunctionOperation
                && operation.Descendants().OfType<IParameterReferenceOperation>().Any(reference =>
                    SymbolEqualityComparer.Default.Equals(reference.Parameter, source))))
            || root.SemanticModel is not { } model)
        {
            return [];
        }

        try
        {
            return GetGraph(root, model) is { } graph
                ? collections ? CancellationTokenCollectionProvenance.Analyze(graph, source) : new Analysis(graph, source, model).Run()
                : [];
        }
        catch (ArgumentException)
        {
            return [];
        }
    }

    private static ControlFlowGraph? GetGraph(IOperation root, SemanticModel model)
    {
        if (root is not (ILocalFunctionOperation or IAnonymousFunctionOperation))
        {
            return ControlFlowGraph.Create(root.Syntax, model);
        }

        var callables = new Stack<IOperation>();
        var outer = root;
        for (var current = root; current is not null; current = current.Parent)
        {
            outer = current;
            if (current is ILocalFunctionOperation or IAnonymousFunctionOperation)
            {
                callables.Push(current);
            }
        }

        var graph = ControlFlowGraph.Create(outer.Syntax, model);
        while (graph is not null && callables.Count > 0)
        {
            graph = GetNestedGraph(graph, callables.Pop());
        }

        return graph;
    }

    private static ControlFlowGraph? GetNestedGraph(ControlFlowGraph graph, IOperation callable)
    {
        if (callable is ILocalFunctionOperation local)
        {
            return graph.GetLocalFunctionControlFlowGraph(local.Symbol);
        }

        var anonymous = graph.Blocks.SelectMany(block => block.Operations
                .Concat(block.BranchValue is { } value ? [value] : []))
            .SelectMany(operation => operation.DescendantsAndSelf())
            .OfType<IFlowAnonymousFunctionOperation>()
            .FirstOrDefault(operation => operation.Syntax.Span == callable.Syntax.Span);
        return anonymous is null ? null : graph.GetAnonymousFunctionControlFlowGraph(anonymous);
    }

    private static IMethodSymbol? GetCallableSymbol(IOperation operation) => operation switch
    {
        ILocalFunctionOperation local => local.Symbol,
        IAnonymousFunctionOperation anonymous => anonymous.Symbol,
        _ => null,
    };

    private static bool HasCapturedWrite(IOperation operation)
    {
        if (GetCallableSymbol(operation) is not { } function)
        {
            return false;
        }

        // Calls can change captured scalar values without appearing as writes in
        // the caller's CFG. Keep the existing proof until helper effects are tracked.
        return operation.Descendants().OfType<ISimpleAssignmentOperation>()
            .Any(assignment => WritesCapturedValue(assignment, function));
    }

    private static bool WritesCapturedValue(ISimpleAssignmentOperation assignment, IMethodSymbol function)
    {
        var target = assignment.Target switch
        {
            IParameterReferenceOperation parameter => (ISymbol) parameter.Parameter,
            ILocalReferenceOperation local => local.Local,
            _ => null,
        };
        var owner = assignment.Parent;
        while (owner is not null && GetCallableSymbol(owner) is null)
        {
            owner = owner.Parent;
        }

        var declaringFunction = owner is null ? function : GetCallableSymbol(owner);
        return target is not null
            && !SymbolEqualityComparer.Default.Equals(target.ContainingSymbol, declaringFunction);
    }

    // true means original, false means replaced on a reachable path; null marks
    // an unsupported value and leaves the pre-existing analysis responsible for it.
    private static bool? MergeSignals(bool? left, bool? right) =>
        left is null || right is null ? null : left.Value && right.Value;

    private sealed class State
    {
        public Dictionary<ISymbol, bool?> Values { get; } = [with(SymbolEqualityComparer.Default)];

        public Dictionary<CaptureId, bool?> Captures { get; } = [];

        public Dictionary<CaptureId, Optional<object?>> Constants { get; } = [];

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

            foreach (var pair in Constants)
            {
                copy.Constants.Add(pair.Key, pair.Value);
            }

            return copy;
        }

        public bool Merge(State other)
        {
            var changed = MergeValues(Values, other.Values) | MergeValues(Captures, other.Captures);
            foreach (var key in Constants.Keys.Concat(other.Constants.Keys).Distinct().ToArray())
            {
                Constants.TryGetValue(key, out var left);
                other.Constants.TryGetValue(key, out var right);
                // Keep a constant only when every incoming path agrees, including
                // paths where the capture is absent or its value is unsupported.
                var merged = left.HasValue && right.HasValue && Equals(left.Value, right.Value) ? left : default;
                if (!Constants.ContainsKey(key) || left.HasValue != merged.HasValue)
                {
                    Constants[key] = merged;
                    changed = true;
                }
            }

            return changed;
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

    private sealed class HelperSummaries
    {
        public Dictionary<IMethodSymbol, bool?> Values { get; } = [with(SymbolEqualityComparer.Default)];

        public HashSet<IMethodSymbol> Active { get; } = [with(SymbolEqualityComparer.Default)];
    }

    private sealed class Analysis(ControlFlowGraph graph, IParameterSymbol source, SemanticModel model, HelperSummaries? helperSummaries = null)
    {
        private readonly Dictionary<int, State> _inputs = [];
        private readonly Queue<BasicBlock> _pending = new();
        private readonly Dictionary<TextSpan, bool?> _results = [];
        private readonly Dictionary<CaptureId, ISymbol> _targets = [];

        private readonly HelperSummaries _helperSummaries = helperSummaries ?? new();

        private bool _hasReturn;
        private bool? _returnValue;

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
                    var value = Evaluate(branchValue, state);
                    if (block.FallThroughSuccessor?.Semantics == ControlFlowBranchSemantics.Return)
                    {
                        _returnValue = _hasReturn ? MergeSignals(_returnValue, value) : value;
                        _hasReturn = true;
                    }
                }

                if (GetConstant(block.BranchValue, state) is { HasValue: true, Value: bool condition }
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
                    return EvaluateParameter(parameter, state);
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
                    return GetSymbolValue(state, local.Local);
                case IFlowCaptureReferenceOperation capture:
                    return Get(state.Captures, capture.Id);
                case IConversionOperation conversion:
                    var converted = Evaluate(conversion.Operand, state);
                    return conversion.Conversion.IsIdentity || conversion.Operand is IDefaultValueOperation ? converted : null;
                case IPropertyReferenceOperation { Property.Name: "Token", Instance: { } instance } property
                    when property.Property.ContainingType.ToDisplayString() == "System.Threading.CancellationTokenSource":
                    return Evaluate(instance, state);
                case IFlowCaptureOperation capture:
                    return EvaluateCapture(capture, state);
                case ISimpleAssignmentOperation assignment:
                    return EvaluateAssignment(assignment, state);
                case IInvocationOperation invocation:
                    return EvaluateLocalHelper(invocation, state);
                case IArgumentOperation argument:
                    return Evaluate(argument.Value, state);
                case IAnonymousFunctionOperation or IFlowAnonymousFunctionOperation or ILocalFunctionOperation:
                    return false;
                default:
                    foreach (var child in operation.ChildOperations)
                    {
                        Evaluate(child, state);
                    }

                    return IsReplacementValue(operation) ? false : null;
            }
        }

        private bool? EvaluateLocalHelper(IInvocationOperation invocation, State state)
        {
            // Only summarize by-value local token helpers. Other calls retain the
            // existing proof, but their argument side effects still execute.
            if (invocation.Instance is { } instance)
            {
                Evaluate(instance, state);
            }

            var arguments = invocation.Arguments.Select(argument =>
                (argument.Parameter, Value: Evaluate(argument.Value, state))).ToArray();
            if (arguments.Length != 1
                || arguments[0].Parameter is not { RefKind: RefKind.None } parameter
                || GetTokenHelper(invocation.TargetMethod) is not { } helper)
            {
                return null;
            }

            var method = helper.Symbol;
            if (!_helperSummaries.Values.TryGetValue(method, out var summary))
            {
                if (!_helperSummaries.Active.Add(method))
                {
                    return null;
                }

                try
                {
                    summary = SummarizeHelper(helper, helper.Symbol.Parameters[parameter.Ordinal]);
                    _helperSummaries.Values.Add(method, summary);
                }
                finally
                {
                    _helperSummaries.Active.Remove(method);
                }
            }

            return summary == true ? arguments[0].Value : summary;
        }

        private ILocalFunctionOperation? GetTokenHelper(IMethodSymbol method)
        {
            if (method.MethodKind != MethodKind.LocalFunction
                || method.ReturnType.ToDisplayString() != "System.Threading.CancellationToken"
                || method.Parameters.Length != 1
                || method.Parameters[0].Type.ToDisplayString() != "System.Threading.CancellationToken"
                || method.DeclaringSyntaxReferences.FirstOrDefault() is not { } declaration)
            {
                return null;
            }

            return model.GetOperation(declaration.GetSyntax()) as ILocalFunctionOperation;
        }

        private bool? SummarizeHelper(ILocalFunctionOperation helper, IParameterSymbol parameter)
        {
            if (helper.Descendants().Any(operation => operation is ITryOperation or ILoopOperation
                    or IDeconstructionAssignmentOperation
                    or ISimpleAssignmentOperation { IsRef: true }
                    or ILocalReferenceOperation { Local.RefKind: not RefKind.None }
                    or IVariableDeclaratorOperation { Symbol.RefKind: not RefKind.None }
                    or IArgumentOperation { Parameter.RefKind: RefKind.Ref or RefKind.Out })
                || helper.DescendantsAndSelf().Any(HasCapturedWrite)
                || GetGraph(helper, model) is not { } helperGraph)
            {
                return null;
            }

            // Summaries use declaration symbols, including for constructed generic
            // calls, and share results across sibling calls in a helper chain.
            var analysis = new Analysis(helperGraph, parameter, model, _helperSummaries);
            analysis.Run();
            return analysis._hasReturn ? analysis._returnValue : null;
        }

        private bool? GetSymbolValue(State state, ISymbol symbol)
        {
            if (state.Values.TryGetValue(symbol, out var value))
            {
                return value;
            }

            // A captured input is absent from this callable's initial state.
            // Its provenance is unknown, not a definite replacement.
            return SymbolEqualityComparer.Default.Equals(symbol.ContainingSymbol, source.ContainingSymbol) ? false : null;
        }

        private bool? EvaluateParameter(IParameterReferenceOperation parameter, State state)
        {
            var value = GetSymbolValue(state, parameter.Parameter);
            var span = parameter.Syntax.Span;
            _results[span] = _results.TryGetValue(span, out var previous)
                ? MergeSignals(previous, value)
                : value;
            return value;
        }

        private bool? EvaluateCapture(IFlowCaptureOperation capture, State state)
        {
            if (GetTarget(capture.Value) is { } target)
            {
                _targets[capture.Id] = target;
            }

            Set(state.Captures, capture.Id, Evaluate(capture.Value, state));
            state.Constants[capture.Id] = GetConstant(capture.Value, state);
            return state.Captures[capture.Id];
        }

        private static Optional<object?> GetConstant(IOperation? operation, State state)
        {
            if (operation?.ConstantValue is { HasValue: true } constant)
            {
                return constant;
            }

            if (operation is IFlowCaptureReferenceOperation capture)
            {
                return state.Constants.TryGetValue(capture.Id, out var captured) ? captured : default;
            }

            if (operation is IIsPatternOperation { Pattern: IConstantPatternOperation pattern } test
                && GetConstant(test.Value, state) is { HasValue: true } value
                && GetConstant(pattern.Value, state) is { HasValue: true } expected
                && (value.Value is null || expected.Value is null || value.Value.GetType() == expected.Value.GetType()))
            {
                return new Optional<object?>(Equals(value.Value, expected.Value));
            }

            return default;
        }

        private bool? EvaluateAssignment(ISimpleAssignmentOperation assignment, State state)
        {
            // Receivers and index arguments can assign scalar values before the RHS.
            Evaluate(assignment.Target, state);
            var assigned = Evaluate(assignment.Value, state);
            if (GetTarget(assignment.Target) is { } target)
            {
                Set(state.Values, target, assigned);
            }

            return assigned;
        }

        private static bool IsReplacementValue(IOperation operation) =>
            operation is IDefaultValueOperation or IObjectCreationOperation || operation.ConstantValue.HasValue ||
            operation is IPropertyReferenceOperation { Property.Name: "None", Property.ContainingType.Name: "CancellationToken" };

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
