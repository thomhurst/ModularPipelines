using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace ModularPipelines.Analyzers;

/// <summary>
/// The only token tracking behind MP0016. One forward pass over the control-flow graph of each
/// method, lambda and local function finds the variables that hold a lost token (one that cannot
/// carry module cancellation) on every path.
/// <para>
/// Scope is fixed by "What MP0016 reports" in docs/docs/how-to/analyzers.md. Any value not listed
/// there counts as not lost and never reports: ref/out/in arguments and ref locals, arrays, tuples
/// and collections, fields, properties, awaited values, and the results and bodies of helper
/// methods. Do not add tracking for those shapes.
/// </para>
/// </summary>
internal static class CancellationTokenFlow
{
    private const string CancellationTokenName = "System.Threading.CancellationToken";
    private const string CancellationTokenSourceName = "System.Threading.CancellationTokenSource";

    // A graph that has not settled after this many rounds is abandoned, which reports nothing.
    private const int MaximumRounds = 64;

    private static readonly ConditionalWeakTable<IOperation, HashSet<TextSpan>> LostInvocations = new();

    /// <summary>
    /// Returns whether the invocation is explicitly given a token that is not lost on every path.
    /// </summary>
    public static bool ReceivesToken(IInvocationOperation invocation, CancellationToken cancellationToken)
    {
        if (!GetTokenArguments(invocation).Any())
        {
            return false;
        }

        IOperation root = invocation;
        while (root.Parent is not null)
        {
            root = root.Parent;
        }

        return !LostInvocations
            .GetValue(root, operation => new Analysis(operation).Run(cancellationToken))
            .Contains(invocation.Syntax.Span);
    }

    private static IEnumerable<IArgumentOperation> GetTokenArguments(IInvocationOperation invocation) =>
        invocation.Arguments.Where(static argument =>
            argument.ArgumentKind == ArgumentKind.Explicit
            && IsType(argument.Parameter?.Type, CancellationTokenName));

    private static bool IsType(ITypeSymbol? type, string name) =>
        type is INamedTypeSymbol && type.ToDisplayString() == name;

    private static ISymbol? GetVariable(IOperation? operation) => operation switch
    {
        ILocalReferenceOperation local => local.Local,
        IParameterReferenceOperation parameter => parameter.Parameter,
        _ => null,
    };

    private static bool IsNestedCallable(ISymbol? symbol) =>
        symbol is IMethodSymbol { MethodKind: MethodKind.AnonymousFunction or MethodKind.LocalFunction };

    private sealed class Analysis(IOperation root)
    {
        // Variables whose value this analysis cannot follow; see the type remarks.
        private readonly HashSet<ISymbol> _untracked = [with(SymbolEqualityComparer.Default)];
        private readonly HashSet<ISymbol> _reassigned = [with(SymbolEqualityComparer.Default)];
        private readonly Dictionary<ISymbol, IOperation> _initializers = [with(SymbolEqualityComparer.Default)];
        private readonly HashSet<ISymbol> _visiting = [with(SymbolEqualityComparer.Default)];
        private readonly HashSet<TextSpan> _lost = [];
        private readonly HashSet<IArgumentOperation> _lostArguments = [];
        private readonly Dictionary<ISymbol, bool> _capturedResults = [with(SymbolEqualityComparer.Default)];

        public HashSet<TextSpan> Run(CancellationToken cancellationToken)
        {
            var graph = root switch
            {
                IMethodBodyOperation body => ControlFlowGraph.Create(body, cancellationToken),
                IConstructorBodyOperation body => ControlFlowGraph.Create(body, cancellationToken),
                IBlockOperation body => ControlFlowGraph.Create(body, cancellationToken),
                IFieldInitializerOperation initializer => ControlFlowGraph.Create(initializer, cancellationToken),
                IPropertyInitializerOperation initializer => ControlFlowGraph.Create(initializer, cancellationToken),
                IParameterInitializerOperation initializer => ControlFlowGraph.Create(initializer, cancellationToken),
                _ => null,
            };
            if (graph is null)
            {
                return _lost;
            }

            CollectVariableUses();
            AnalyzeGraph(graph, owner: null, cancellationToken);
            return _lost;
        }

        private void CollectVariableUses()
        {
            foreach (var operation in root.DescendantsAndSelf())
            {
                switch (operation)
                {
                    case IVariableDeclaratorOperation { Symbol.RefKind: not RefKind.None } declarator:
                        _untracked.Add(declarator.Symbol);
                        Untrack(declarator.GetVariableInitializer());
                        break;
                    case IVariableDeclaratorOperation declarator
                        when declarator.GetVariableInitializer() is { } initializer:
                        _initializers[declarator.Symbol] = initializer.Value;
                        break;
                    case ILocalReferenceOperation or IParameterReferenceOperation
                        when IsType(operation.Type, CancellationTokenSourceName)
                             && (!IsLocalSourceUse(operation)
                                 || (GetEnclosingCallable(operation) is { } capturer
                                     && !SymbolEqualityComparer.Default.Equals(
                                         GetVariable(operation)!.ContainingSymbol,
                                         capturer))):
                        // Any other use lets the source be cancelled from elsewhere, such as a
                        // module token registration.
                        _untracked.Add(GetVariable(operation)!);
                        break;
                    case ISimpleAssignmentOperation { IsRef: true }:
                    case IAddressOfOperation:
                    case IArgumentOperation { Parameter.RefKind: not RefKind.None }:
                        Untrack(operation);
                        break;
                    case ICompoundAssignmentOperation assignment:
                        Untrack(assignment.Target);
                        break;
                    case ICoalesceAssignmentOperation assignment:
                        Untrack(assignment.Target);
                        break;
                    case IDeconstructionAssignmentOperation assignment:
                        Untrack(assignment.Target);
                        break;
                    case IIncrementOrDecrementOperation increment:
                        Untrack(increment.Target);
                        break;
                    case ISimpleAssignmentOperation assignment when GetVariable(assignment.Target) is { } variable:
                        _reassigned.Add(variable);
                        if (GetEnclosingCallable(assignment) is { } callable
                            && !SymbolEqualityComparer.Default.Equals(variable.ContainingSymbol, callable))
                        {
                            // Written by a lambda or local function that does not own it.
                            _untracked.Add(variable);
                        }

                        break;
                }
            }
        }

        /// <summary>A source stays tracked only while it is assigned to or has a member read or called on it.</summary>
        private static bool IsLocalSourceUse(IOperation reference) => reference.Parent switch
        {
            IPropertyReferenceOperation property => property.Instance == reference,
            IInvocationOperation invocation => invocation.Instance == reference,
            ISimpleAssignmentOperation assignment => assignment.Target == reference,
            _ => false,
        };

        private void Untrack(IOperation? operation)
        {
            foreach (var variable in operation?.DescendantsAndSelf().Select(GetVariable) ?? [])
            {
                if (variable is not null)
                {
                    _untracked.Add(variable);
                }
            }
        }

        private static IMethodSymbol? GetEnclosingCallable(IOperation operation)
        {
            for (var current = operation.Parent; current is not null; current = current.Parent)
            {
                switch (current)
                {
                    case IAnonymousFunctionOperation anonymous:
                        return anonymous.Symbol;
                    case ILocalFunctionOperation local:
                        return local.Symbol;
                }
            }

            return null;
        }

        private void AnalyzeGraph(ControlFlowGraph graph, IMethodSymbol? owner, CancellationToken cancellationToken)
        {
            var blocks = graph.Blocks;
            var inputs = new HashSet<object>?[blocks.Length];
            var outputs = new HashSet<object>?[blocks.Length];
            var handlers = GetHandlerEntries(graph.Root);
            var settled = false;

            for (var round = 0; round < MaximumRounds && !settled; round++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                settled = true;
                foreach (var block in blocks)
                {
                    if (!block.IsReachable || GetInput(block, blocks, inputs, outputs, handlers) is not { } input)
                    {
                        continue;
                    }

                    inputs[block.Ordinal] = input;
                    var output = Transfer(block, input, owner, record: false);
                    if (outputs[block.Ordinal]?.SetEquals(output) != true)
                    {
                        outputs[block.Ordinal] = output;
                        settled = false;
                    }
                }
            }

            if (settled)
            {
                foreach (var block in blocks)
                {
                    if (inputs[block.Ordinal] is { } input)
                    {
                        Transfer(block, input, owner, record: true);
                    }
                }
            }

            foreach (var local in graph.LocalFunctions)
            {
                AnalyzeGraph(graph.GetLocalFunctionControlFlowGraph(local, cancellationToken), local, cancellationToken);
            }

            foreach (var anonymous in blocks
                         .SelectMany(static block => block.Operations.Concat(
                             block.BranchValue is { } branch ? [branch] : []))
                         .SelectMany(static operation => operation.DescendantsAndSelf())
                         .OfType<IFlowAnonymousFunctionOperation>())
            {
                AnalyzeGraph(
                    graph.GetAnonymousFunctionControlFlowGraph(anonymous, cancellationToken),
                    anonymous.Symbol,
                    cancellationToken);
            }
        }

        /// <summary>Maps the first block of each catch, filter and finally region to its try region.</summary>
        private static Dictionary<int, ControlFlowRegion> GetHandlerEntries(ControlFlowRegion region)
        {
            var handlers = new Dictionary<int, ControlFlowRegion>();
            var pending = new Stack<ControlFlowRegion>([region]);
            while (pending.Count > 0)
            {
                var current = pending.Pop();
                if (current.Kind is ControlFlowRegionKind.TryAndCatch or ControlFlowRegionKind.TryAndFinally)
                {
                    foreach (var handler in current.NestedRegions.Skip(1))
                    {
                        handlers[handler.FirstBlockOrdinal] = current.NestedRegions[0];
                    }
                }

                foreach (var nested in current.NestedRegions)
                {
                    pending.Push(nested);
                }
            }

            return handlers;
        }

        private static HashSet<object>? GetInput(
            BasicBlock block,
            ImmutableArray<BasicBlock> blocks,
            HashSet<object>?[] inputs,
            HashSet<object>?[] outputs,
            Dictionary<int, ControlFlowRegion> handlers)
        {
            HashSet<object>? input = block.Kind == BasicBlockKind.Entry ? [] : null;

            // An exception can leave a try block after any of its writes, so a handler starts from
            // the try's entry state without any variable the try writes.
            if (handlers.TryGetValue(block.Ordinal, out var tryRegion)
                && inputs[tryRegion.FirstBlockOrdinal] is { } tryInput)
            {
                input = [with(tryInput)];
                input.ExceptWith(GetWrittenVariables(blocks, tryRegion));
            }

            foreach (var branch in block.Predecessors)
            {
                if (outputs[branch.Source.Ordinal] is not { } output
                    || ThroughFinally(branch, blocks, output, outputs) is not { } incoming)
                {
                    continue;
                }

                if (input is null)
                {
                    input = [with(incoming)];
                }
                else
                {
                    // Lost only when lost along every incoming path.
                    input.IntersectWith(incoming);
                }
            }

            return input;
        }

        /// <summary>
        /// A branch that leaves a try block runs its finally regions first. Their blocks are not
        /// predecessors in the graph, so apply what each finally writes on top of the branch state.
        /// </summary>
        private static HashSet<object>? ThroughFinally(
            ControlFlowBranch branch,
            ImmutableArray<BasicBlock> blocks,
            HashSet<object> state,
            HashSet<object>?[] outputs)
        {
            if (branch.FinallyRegions.IsEmpty)
            {
                return state;
            }

            var result = new HashSet<object>(state);
            foreach (var region in branch.FinallyRegions)
            {
                if (outputs[region.LastBlockOrdinal] is not { } exit)
                {
                    return null;
                }

                foreach (var variable in GetWrittenVariables(blocks, region))
                {
                    Set(result, variable, exit.Contains(variable));
                }
            }

            return result;
        }

        private static IEnumerable<ISymbol> GetWrittenVariables(
            ImmutableArray<BasicBlock> blocks,
            ControlFlowRegion region) =>
            blocks
                .Skip(region.FirstBlockOrdinal)
                .Take(region.LastBlockOrdinal - region.FirstBlockOrdinal + 1)
                .SelectMany(static block => block.BranchValue is { } branch ? block.Operations.Add(branch) : block.Operations)
                .SelectMany(static operation => operation.DescendantsAndSelf())
                .OfType<ISimpleAssignmentOperation>()
                .Select(static assignment => GetVariable(assignment.Target))
                .Where(static variable => variable is not null)!;

        private static void Set(HashSet<object> state, object key, bool isLost)
        {
            if (isLost)
            {
                state.Add(key);
            }
            else
            {
                state.Remove(key);
            }
        }

        private HashSet<object> Transfer(
            BasicBlock block,
            HashSet<object> input,
            IMethodSymbol? owner,
            bool record)
        {
            var state = new HashSet<object>(input);
            foreach (var operation in block.Operations)
            {
                Visit(operation, state, owner, record);
            }

            if (block.BranchValue is { } branchValue)
            {
                Visit(branchValue, state, owner, record);
            }

            return state;
        }

        private void Visit(IOperation operation, HashSet<object> state, IMethodSymbol? owner, bool record)
        {
            foreach (var child in operation.ChildOperations)
            {
                Visit(child, state, owner, record);
            }

            switch (operation)
            {
                case ISimpleAssignmentOperation { IsRef: false } assignment
                    when GetVariable(assignment.Target) is { } variable:
                    Set(state, variable, IsLost(assignment.Value, state, owner));
                    break;
                case IFlowCaptureOperation capture:
                    Set(state, capture.Id, IsLost(capture.Value, state, owner));
                    break;
                case IArgumentOperation argument when record && IsLost(argument.Value, state, owner):
                    // Classified here, before later arguments of the same call can write the variable.
                    _lostArguments.Add(argument);
                    break;
                case IInvocationOperation invocation when record:
                    var arguments = GetTokenArguments(invocation).ToArray();
                    if (arguments.Length > 0 && arguments.All(_lostArguments.Contains))
                    {
                        _lost.Add(invocation.Syntax.Span);
                    }

                    break;
            }
        }

        /// <param name="operation">The value to classify.</param>
        /// <param name="state">The lost variables, or null when evaluating a captured variable's initializer.</param>
        /// <param name="owner">The lambda or local function being analysed, or null for the outer body.</param>
        private bool IsLost(IOperation? operation, HashSet<object>? state, IMethodSymbol? owner)
        {
            return operation switch
            {
                IParenthesizedOperation parenthesized => IsLost(parenthesized.Operand, state, owner),
                IConversionOperation { OperatorMethod: null } conversion => IsLost(conversion.Operand, state, owner),
                ISimpleAssignmentOperation assignment => IsLost(assignment.Value, state, owner),
                IDefaultValueOperation => IsType(operation.Type, CancellationTokenName),
                IObjectCreationOperation creation => IsType(creation.Type, CancellationTokenName)
                                        || IsType(creation.Type, CancellationTokenSourceName),
                IPropertyReferenceOperation { Property.Name: "None" } property => IsType(property.Property.ContainingType, CancellationTokenName),
                IPropertyReferenceOperation { Property.Name: "Token" } property
                                    when IsType(property.Property.ContainingType, CancellationTokenSourceName) => IsLost(property.Instance, state, owner),
                ICoalesceOperation coalesce => AreAllLost([coalesce.Value, coalesce.WhenNull], state, owner),
                IConditionalOperation { WhenFalse: { } whenFalse } conditional => AreAllLost([conditional.WhenTrue, whenFalse], state, owner),
                ISwitchExpressionOperation switchExpression => AreAllLost(switchExpression.Arms.Select(static arm => arm.Value), state, owner),
                IFlowCaptureReferenceOperation capture => state?.Contains(capture.Id) == true,
                ILocalReferenceOperation or IParameterReferenceOperation => IsVariableLost(GetVariable(operation)!, state, owner),
                _ => false,
            };
        }

        private bool AreAllLost(IEnumerable<IOperation> values, HashSet<object>? state, IMethodSymbol? owner)
        {
            // A branch that throws passes no token at all, so it does not count either way.
            var reachable = values.Where(static value => Unwrap(value) is not IThrowOperation).ToArray();
            return reachable.Length > 0 && reachable.All(value => IsLost(value, state, owner));
        }

        private static IOperation Unwrap(IOperation value)
        {
            while (value is IConversionOperation { OperatorMethod: null } conversion)
            {
                value = conversion.Operand;
            }

            return value;
        }

        private bool IsVariableLost(ISymbol variable, HashSet<object>? state, IMethodSymbol? owner)
        {
            if (_untracked.Contains(variable))
            {
                return false;
            }

            var isOwned = owner is null
                ? !IsNestedCallable(variable.ContainingSymbol)
                : SymbolEqualityComparer.Default.Equals(variable.ContainingSymbol, owner);
            if (state is not null && isOwned)
            {
                return state.Contains(variable);
            }

            // A captured variable is followed only when it is never assigned after its declaration.
            // Its initializer does not depend on flow state, so each one is evaluated once.
            if (_capturedResults.TryGetValue(variable, out var isLost))
            {
                return isLost;
            }

            if (_reassigned.Contains(variable)
                || !_initializers.TryGetValue(variable, out var initializer)
                || !_visiting.Add(variable))
            {
                return false;
            }

            isLost = IsLost(initializer, state: null, owner);
            _visiting.Remove(variable);
            _capturedResults[variable] = isLost;
            return isLost;
        }
    }
}
