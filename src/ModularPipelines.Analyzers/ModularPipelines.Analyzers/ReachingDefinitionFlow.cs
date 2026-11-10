using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace ModularPipelines.Analyzers;

internal sealed class ReachingDefinitionFlow(
    ControlFlowGraph graph,
    IOperation invocation,
    Dictionary<TextSpan, IOperation> definitions,
    ILocalSymbol local,
    bool analyzeReceiverTypes = true,
    bool completingReturn = false,
    bool analyzeCapturedWrites = true,
    ImmutableDictionary<IMethodSymbol, IInvocationOperation>? callSites = null,
    Action<ITypeSymbol?, HashSet<TextSpan>, ReachingDefinitionFlow.PendingCleanup?>? escapingException = null,
    ImmutableHashSet<TextSpan>? receiverQueries = null,
    Action<HashSet<TextSpan>>? completedCleanup = null,
    IOperation? operationRoot = null,
    Action<HashSet<TextSpan>>? asyncSuspension = null)
{
    // A path without a local definition still reads the capture at the enclosing execution site.
    private static readonly TextSpan IncomingValue = new(int.MaxValue, 0);
    private readonly IOperation _operationRoot = operationRoot ?? GetOperationRoot(invocation);
    private readonly HashSet<TextSpan> _reaching = [];
    private readonly Dictionary<string, HashSet<TextSpan>> _inputs = [];
    private readonly Queue<(int Block, ImmutableArray<int> Continuation, FilterContext? Filter,
        ImmutableDictionary<int, ITypeSymbol?> Exceptions)> _queue = [];
    private ImmutableDictionary<int, ITypeSymbol?> _activeExceptions = ImmutableDictionary<int, ITypeSymbol?>.Empty;
    private AwaitFault? _awaitFault;
    private bool _resumingFaultedCall;

    public static bool TryGetReachingLocalValues(IOperation root, IOperation operation, ILocalSymbol local,
        out ImmutableArray<IOperation> values, out bool inheritsIncomingValue, bool analyzeReceiverTypes = true,
        bool analyzeCapturedWrites = true, ImmutableHashSet<TextSpan>? receiverQueries = null)
    {
        values = [];
        inheritsIncomingValue = false;
        if (CreateGraph(root) is not { } rootGraph
            || FindGraph(rootGraph, (operation is IReturnOperation { ReturnedValue: { } returnedValue }
                ? returnedValue : operation).Syntax.Span) is not { } graph)
        {
            return false;
        }

        var definitions = root.DescendantsAndSelf().SelectMany(candidate => GetAssignedValues(candidate, local))
            .GroupBy(static value => value.Syntax.Span)
            .ToDictionary(static group => group.Key, static group => group.First());
        var flow = new ReachingDefinitionFlow(graph, operation, definitions, local, analyzeReceiverTypes,
            analyzeCapturedWrites: analyzeCapturedWrites, receiverQueries: receiverQueries, operationRoot: root);
        values = [.. flow.Analyze()];
        inheritsIncomingValue = flow._reaching.Contains(IncomingValue);
        return true;
    }

    private IEnumerable<IOperation> Analyze()
    {
        Enqueue(0, [], [IncomingValue]);
        AnalyzePendingBlocks();
        return _reaching.Where(span => span != IncomingValue).Select(span => definitions[span]);
    }

    private void AnalyzePendingBlocks()
    {
        while (_queue.Count > 0)
        {
            var (ordinal, continuation, filter, exceptions) = _queue.Dequeue();
            _activeExceptions = exceptions;
            // Exception search has selected the destination before this cleanup resumes it.
            if (ordinal == -1)
            {
                completedCleanup?.Invoke(_inputs[Key(ordinal, continuation, filter, exceptions)]);
                continue;
            }

            var block = graph.Blocks[ordinal];
            var state = new HashSet<TextSpan>(_inputs[Key(ordinal, continuation, filter, _activeExceptions)]);
            if (completingReturn && block.Kind == BasicBlockKind.Exit)
            {
                _reaching.UnionWith(state);
            }

            foreach (var operation in block.Operations)
            {
                Visit(operation, state, block, continuation, filter);
            }

            if (block.BranchValue is { } branchValue)
            {
                Visit(branchValue, state, block, continuation, filter);
            }

            Follow(block.FallThroughSuccessor, state, continuation, filter);
            Follow(block.ConditionalSuccessor, state, continuation, filter);
        }
    }

    private void Enqueue(int ordinal, ImmutableArray<int> continuation, HashSet<TextSpan> state,
        FilterContext? filter = null, ImmutableDictionary<int, ITypeSymbol?>? exceptions = null)
    {
        exceptions ??= _activeExceptions;
        var key = Key(ordinal, continuation, filter, exceptions);
        if (!_inputs.TryGetValue(key, out var previous))
        {
            _inputs.Add(key, [.. state]);
        }
        else
        {
            var count = previous.Count;
            previous.UnionWith(state);
            if (count == previous.Count)
            {
                return;
            }
        }

        _queue.Enqueue((ordinal, continuation, filter, exceptions));
    }

    private void Visit(IOperation operation, HashSet<TextSpan> state, BasicBlock block,
        ImmutableArray<int> continuation, FilterContext? filter)
    {
        if (state.Count == 0 || SkipBeforeFaultedCall(operation))
        {
            return;
        }

        // Delegate bodies have their own graph and execute only when invoked.
        if (operation is IFlowAnonymousFunctionOperation)
        {
            return;
        }

        // Compound assignment reads its previous targets before evaluating the right-hand side.
        var addedTargets = operation is ICompoundAssignmentOperation { Target: ILocalReferenceOperation target } assignment
            && SymbolEqualityComparer.Default.Equals(target.Local, local) && IsDelegateAddition(assignment)
            ? new HashSet<TextSpan>(state) : null;
        foreach (var child in operation.ChildOperations)
        {
            Visit(child, state, block, continuation, filter);
        }

        if (state.Count == 0)
        {
            return;
        }

        if (ObserveAwaitFault(operation, state, block, continuation, filter))
        {
            return;
        }

        if (operation.Syntax.Span == invocation.Syntax.Span)
        {
            _reaching.UnionWith(state);
        }

        if (operation is IAwaitOperation awaitOperation && CanSuspend(awaitOperation))
        {
            asyncSuspension?.Invoke(state);
        }

        if (!ApplyCapturedWrites(operation, state, block, continuation, filter))
        {
            FollowOperationExceptions(operation, state, block, continuation, filter);
        }
        ApplyDefinition(operation, state, addedTargets);
    }

    private bool SkipBeforeFaultedCall(IOperation operation)
    {
        if (!_resumingFaultedCall)
        {
            return false;
        }

        if (operation is IInvocationOperation && operation.Syntax.Span == _awaitFault!.Call)
        {
            _resumingFaultedCall = false;
            return true;
        }

        return !operation.Syntax.Span.Contains(_awaitFault!.Call);
    }

    private bool ObserveAwaitFault(IOperation operation, HashSet<TextSpan> state, BasicBlock block,
        ImmutableArray<int> continuation, FilterContext? filter)
    {
        if (operation is not IAwaitOperation || _awaitFault is not { } fault || operation.Syntax.Span != fault.Await)
        {
            return false;
        }

        fault.Caller.FollowException(block, fault.ExceptionType, state, continuation, filter);
        state.Clear();
        return true;
    }

    private void FollowOperationExceptions(IOperation operation, HashSet<TextSpan> state, BasicBlock block,
        ImmutableArray<int> continuation, FilterContext? filter)
    {
        var arithmeticExceptions = GetArithmeticExceptions(operation);
        var receiverException = GetReceiverExceptionName(operation);
        if (receiverException is not null)
        {
            FollowKnownException(receiverException);
        }
        else if (GetArrayAccess(operation) is { } array)
        {
            FollowKnownException("System.NullReferenceException");
            FollowKnownException("System.IndexOutOfRangeException");
            if (CanArrayAccessMismatch(operation, array))
            {
                FollowKnownException("System.ArrayTypeMismatchException");
            }
        }
        else if (arithmeticExceptions == ArithmeticExceptions.Unknown || CanThrow(operation))
        {
            FollowException(block, null, state, continuation, filter);
        }
        else
        {
            if ((arithmeticExceptions & ArithmeticExceptions.DivideByZero) != 0)
            {
                FollowKnownException("System.DivideByZeroException");
            }

            if ((arithmeticExceptions & ArithmeticExceptions.Overflow) != 0)
            {
                FollowKnownException("System.OverflowException");
            }
        }

        void FollowKnownException(string exceptionName) => FollowException(block,
            _operationRoot.SemanticModel?.Compilation.GetTypeByMetadataName(exceptionName), state, continuation, filter);
    }

    private string? GetReceiverExceptionName(IOperation operation)
    {
        var field = GetFieldAccess(operation);
        if (field is { Field: { IsStatic: true, IsConst: false } })
        {
            return "System.TypeInitializationException";
        }

        return (field is not null && HasNullableReceiver(field.Instance)) || IsThrowingMethodReference(operation)
            ? "System.NullReferenceException"
            : null;
    }

    private static bool IsSimpleAssignmentTarget(IOperation operation) =>
        operation.Parent is ISimpleAssignmentOperation assignment && ReferenceEquals(assignment.Target, operation);

    private static IArrayElementReferenceOperation? GetArrayAccess(IOperation operation) => operation switch
    {
        // A store checks the array after evaluating its assigned value.
        ISimpleAssignmentOperation { Target: IArrayElementReferenceOperation array } => array,
        IArrayElementReferenceOperation array when !IsSimpleAssignmentTarget(array) => array,
        _ => null,
    };

    private bool CanArrayAccessMismatch(IOperation operation, IArrayElementReferenceOperation array) =>
        array.Type is not { IsValueType: true } and not INamedTypeSymbol { IsSealed: true }
        && IsWritableArrayAccess(operation)
        && (!analyzeReceiverTypes || !HasExactArrayType(array.ArrayReference, array.ArrayReference.Type,
            [with(SymbolEqualityComparer.Default)]));

    private static bool IsWritableArrayAccess(IOperation operation) => operation switch
    {
        ISimpleAssignmentOperation assignment =>
            !(assignment.Value.ConstantValue.HasValue && assignment.Value.ConstantValue.Value is null),
        IArrayElementReferenceOperation
        {
            Parent: ISimpleAssignmentOperation
            { IsRef: true, Target: ILocalReferenceOperation { Local.RefKind: RefKind.Ref } }
        } => true,
        IArrayElementReferenceOperation
        {
            Parent: IArgumentOperation { Parameter.RefKind: RefKind.Ref or RefKind.Out }
        } => true,
        _ => false,
    };

    private bool HasExactArrayType(IOperation value, ITypeSymbol? arrayType, HashSet<ILocalSymbol> visited)
    {
        while (value is IConversionOperation { OperatorMethod: null } conversion)
        {
            value = conversion.Operand;
        }

        if (value is IArrayCreationOperation)
        {
            return SymbolEqualityComparer.Default.Equals(value.Type, arrayType);
        }

        if (value is not ILocalReferenceOperation reference || !visited.Add(reference.Local))
        {
            return false;
        }

        var root = _operationRoot;

        // Disable receiver proofs in the nested flow to avoid recursive array/field analysis.
        var known = !HasUntrackedWrites(root, reference.Local, value)
            && TryGetReachingLocalValues(root, value, reference.Local, out var values, out var incoming,
                analyzeReceiverTypes: false)
            && !incoming && !values.IsEmpty
            && values.All(candidate => HasExactArrayType(candidate, arrayType, visited));
        visited.Remove(reference.Local);
        return known;
    }

    private static bool IsDelegateAddition(ICompoundAssignmentOperation assignment) =>
        assignment.OperatorKind == BinaryOperatorKind.Add && assignment.Target.Type?.TypeKind == TypeKind.Delegate;

    private static IEnumerable<IOperation> GetAssignedValues(IOperation operation, ILocalSymbol localSymbol)
    {
        if (operation is IVariableDeclaratorOperation { Initializer: { } initializer } declarator
            && SymbolEqualityComparer.Default.Equals(declarator.Symbol, localSymbol))
        {
            yield return initializer.Value;
        }
        else if (operation is IAssignmentOperation { Target: ILocalReferenceOperation target } assignment
            && (assignment is ISimpleAssignmentOperation
                || (assignment is ICompoundAssignmentOperation compound && IsDelegateAddition(compound)))
            && SymbolEqualityComparer.Default.Equals(target.Local, localSymbol))
        {
            yield return assignment.Value;
        }
        else if (operation is IDeconstructionAssignmentOperation deconstruction)
        {
            foreach (var value in GetDeconstructedValues(deconstruction.Target, deconstruction.Value, localSymbol))
            {
                yield return value;
            }
        }
    }

    private static IEnumerable<IOperation> GetDeconstructedValues(IOperation target, IOperation value,
        ILocalSymbol localSymbol)
    {
        if (target is IDeclarationExpressionOperation declaration)
        {
            target = declaration.Expression;
        }

        if (target is ILocalReferenceOperation reference
            && SymbolEqualityComparer.Default.Equals(reference.Local, localSymbol))
        {
            yield return value;
            yield break;
        }

        while (value is IConversionOperation conversion)
        {
            value = conversion.Operand;
        }

        if (target is not ITupleOperation targets || value is not ITupleOperation values)
        {
            yield break;
        }

        foreach (var element in GetTupleElementValues(targets, values, localSymbol))
        {
            yield return element;
        }
    }

    private static IEnumerable<IOperation> GetTupleElementValues(ITupleOperation targets, ITupleOperation values,
        ILocalSymbol localSymbol)
    {
        for (var index = 0; index < targets.Elements.Length && index < values.Elements.Length; index++)
        {
            foreach (var element in GetDeconstructedValues(targets.Elements[index], values.Elements[index], localSymbol))
            {
                yield return element;
            }
        }
    }

    private void ApplyDefinition(IOperation operation, HashSet<TextSpan> state, HashSet<TextSpan>? addedTargets)
    {
        if (operation is not (IAssignmentOperation or IVariableDeclaratorOperation))
        {
            return;
        }

        foreach (var value in GetAssignedValues(operation, local))
        {
            if (!definitions.ContainsKey(value.Syntax.Span))
            {
                continue;
            }

            state.Clear();
            if (addedTargets is not null)
            {
                state.UnionWith(addedTargets);
            }

            state.Add(value.Syntax.Span);
        }
    }

    private bool ApplyCapturedWrites(IOperation operation, HashSet<TextSpan> state, BasicBlock block,
        ImmutableArray<int> continuation, FilterContext? filter)
    {
        if (!analyzeCapturedWrites || operation is not IInvocationOperation
            { TargetMethod.MethodKind: MethodKind.LocalFunction or MethodKind.DelegateInvoke } call)
        {
            return false;
        }

        var sites = callSites ?? ImmutableDictionary.Create<IMethodSymbol, IInvocationOperation>(SymbolEqualityComparer.Default);
        if (!TryGetInvokedGraphs(call, sites, out var bodies))
        {
            return false;
        }

        var output = new HashSet<TextSpan>();
        foreach (var path in bodies)
        {
            if (path.IsEmpty)
            {
                FollowException(block, _operationRoot.SemanticModel?.Compilation
                    .GetTypeByMetadataName("System.NullReferenceException"), state, continuation, filter);
                continue;
            }

            var current = new HashSet<TextSpan>(state);
            for (var index = 0; index < path.Length; index++)
            {
                var (method, bodyGraph) = path[index];
                var awaited = method.IsAsync && index == path.Length - 1 && GetAwaitedOperation(call) is not null;
                current = TransferCapturedWrites(method, bodyGraph, call, sites, current, awaited,
                    block, continuation, filter);
            }

            output.UnionWith(current);
        }

        state.Clear();
        state.UnionWith(output);
        return true;
    }

    private HashSet<TextSpan> TransferCapturedWrites(IMethodSymbol method, ControlFlowGraph bodyGraph,
        IInvocationOperation call, ImmutableDictionary<IMethodSymbol, IInvocationOperation> sites,
        HashSet<TextSpan> state, bool awaited, BasicBlock block, ImmutableArray<int> continuation, FilterContext? filter)
    {
        var output = new HashSet<TextSpan>();
        Action<HashSet<TextSpan>>? suspension = null;
        if (method.IsAsync)
        {
            suspension = awaited ? asyncSuspension : output.UnionWith;
        }

        // Transfer normal exits separately from exceptions so discarded writes cannot reach the caller.
        var body = new ReachingDefinitionFlow(bodyGraph, invocation, definitions, local, analyzeReceiverTypes,
            completingReturn: true, analyzeCapturedWrites, sites.Add(method.OriginalDefinition, call),
            (exceptionType, escaped, cleanup) =>
            {
                if (method.IsAsync)
                {
                    FollowAsyncException(exceptionType, escaped, cleanup, awaited, call, output, block, continuation, filter);
                }
                else
                {
                    FollowException(block, exceptionType, escaped, continuation, filter, cleanup);
                }
            },
            receiverQueries, operationRoot: _operationRoot,
            asyncSuspension: suspension);
        body.Enqueue(0, [], state);
        body.AnalyzePendingBlocks();
        output.UnionWith(body._reaching);
        return output;
    }

    private void FollowAsyncException(ITypeSymbol? exceptionType, HashSet<TextSpan> escaped, PendingCleanup? cleanup,
        bool awaited, IInvocationOperation call, HashSet<TextSpan> output, BasicBlock block,
        ImmutableArray<int> continuation, FilterContext? filter)
    {
        // Await observes the fault after callee cleanup; an unawaited task lets its caller continue.
        ApplyCleanup(cleanup, escaped, completed =>
        {
            if (awaited)
            {
                ResumeAwaitFault(call, exceptionType, completed, block, continuation, filter);
            }
            else
            {
                output.UnionWith(completed);
            }
        });
    }

    private void ResumeAwaitFault(IInvocationOperation call, ITypeSymbol? exceptionType, HashSet<TextSpan> state,
        BasicBlock block, ImmutableArray<int> continuation, FilterContext? filter)
    {
        // Resume with the faulted task value. Caller arguments and branches run before await observes the fault.
        var resumed = new ReachingDefinitionFlow(graph, invocation, definitions, local, analyzeReceiverTypes,
            completingReturn, analyzeCapturedWrites, callSites, escapingException, receiverQueries,
            completedCleanup, _operationRoot, asyncSuspension)
        {
            _activeExceptions = _activeExceptions,
            _awaitFault = new AwaitFault(this, call.Syntax.Span, GetAwaitedOperation(call)!.Syntax.Span, exceptionType),
            _resumingFaultedCall = true,
        };
        resumed.Enqueue(block.Ordinal, continuation, state, filter);
        resumed.AnalyzePendingBlocks();
        _reaching.UnionWith(resumed._reaching);
    }

    private IAwaitOperation? GetAwaitedOperation(IInvocationOperation call)
    {
        // CFG lowering can separate the receiver from branching arguments with flow captures.
        var value = _operationRoot.SemanticModel?.GetOperation(call.Syntax) ?? call;
        while (value.Parent is IConversionOperation
               || (value.Parent is IInvocationOperation { TargetMethod.Name: "ConfigureAwait" } configured
                   && ReferenceEquals(configured.Instance, value)))
        {
            value = value.Parent;
        }

        return value.Parent as IAwaitOperation;
    }

    private sealed class AwaitFault(ReachingDefinitionFlow caller, TextSpan call, TextSpan awaitSpan,
        ITypeSymbol? exceptionType)
    {
        public ReachingDefinitionFlow Caller { get; } = caller;
        public TextSpan Call { get; } = call;
        public TextSpan Await { get; } = awaitSpan;
        public ITypeSymbol? ExceptionType { get; } = exceptionType;
    }

    private bool TryGetInvokedGraphs(IInvocationOperation call,
        ImmutableDictionary<IMethodSymbol, IInvocationOperation> sites,
        out ImmutableArray<ImmutableArray<(IMethodSymbol Method, ControlFlowGraph Graph)>> bodies)
    {
        bodies = [];
        if (CreateGraph(_operationRoot) is not { } rootGraph)
        {
            return false;
        }

        var result = ImmutableArray.CreateBuilder<ImmutableArray<(IMethodSymbol, ControlFlowGraph)>>();
        foreach (var path in GetInvokedMethods(_operationRoot, call, sites, receiverQueries ?? []))
        {
            var graphs = ImmutableArray.CreateBuilder<(IMethodSymbol, ControlFlowGraph)>();
            foreach (var method in path)
            {
                if (sites.ContainsKey(method.OriginalDefinition) || FindCallableGraph(rootGraph, method) is not { } bodyGraph)
                {
                    return false;
                }

                graphs.Add((method, bodyGraph));
            }

            result.Add(graphs.ToImmutable());
        }

        bodies = result.ToImmutable();
        return !bodies.IsEmpty;
    }

    private static ControlFlowGraph? FindCallableGraph(ControlFlowGraph root, IMethodSymbol method)
    {
        foreach (var localFunction in root.LocalFunctions)
        {
            var nested = root.GetLocalFunctionControlFlowGraph(localFunction);
            if (SymbolEqualityComparer.Default.Equals(localFunction.OriginalDefinition, method.OriginalDefinition))
            {
                return nested;
            }

            if (FindCallableGraph(nested, method) is { } found)
            {
                return found;
            }
        }

        foreach (var anonymous in root.Blocks.SelectMany(static block => block.Operations
                     .Concat(block.BranchValue is { } value ? [value] : []))
                     .SelectMany(static operation => operation.DescendantsAndSelf()).OfType<IFlowAnonymousFunctionOperation>())
        {
            var nested = root.GetAnonymousFunctionControlFlowGraph(anonymous);
            if (SymbolEqualityComparer.Default.Equals(anonymous.Symbol.OriginalDefinition, method.OriginalDefinition))
            {
                return nested;
            }

            if (FindCallableGraph(nested, method) is { } found)
            {
                return found;
            }
        }

        return null;
    }

    private static IEnumerable<ImmutableArray<IMethodSymbol>> GetInvokedMethods(IOperation root, IInvocationOperation call,
        ImmutableDictionary<IMethodSymbol, IInvocationOperation> sites, ImmutableHashSet<TextSpan> queries) =>
        call.TargetMethod.MethodKind switch
        {
            MethodKind.DelegateInvoke when call.Instance is { } receiver =>
                GetDelegateMethods(root, receiver, [], sites, queries),
            MethodKind.LocalFunction => [[call.TargetMethod]],
            _ => [],
        };

    private static IEnumerable<ImmutableArray<IMethodSymbol>> GetDelegateMethods(IOperation root, IOperation value,
        HashSet<IOperation> visited, ImmutableDictionary<IMethodSymbol, IInvocationOperation> sites,
        ImmutableHashSet<TextSpan> queries)
    {
        return value switch
        {
            { ConstantValue: { HasValue: true, Value: null } } => [[]],
            IAnonymousFunctionOperation anonymous => [[anonymous.Symbol]],
            IMethodReferenceOperation method => [[method.Method]],
            IDelegateCreationOperation creation => GetDelegateMethods(root, creation.Target, visited, sites, queries),
            IConversionOperation conversion => GetDelegateMethods(root, conversion.Operand, visited, sites, queries),
            IBinaryOperation { OperatorKind: BinaryOperatorKind.Add, Type.TypeKind: TypeKind.Delegate } combined =>
                GetCombinedDelegateMethods(root, combined, visited, sites, queries),
            IConditionalOperation or ICoalesceOperation or ISwitchExpressionOperation =>
                ModuleAuthoringAnalysis.GetReachableValueLeaves(value)
                    .SelectMany(leaf => GetDelegateMethods(root, leaf, [.. visited], sites, queries)),
            ILocalReferenceOperation reference when visited.Add(value) =>
                GetDelegateLocalMethods(root, reference, value, visited, sites, queries),
            _ => [],
        };
    }

    private static IEnumerable<ImmutableArray<IMethodSymbol>> GetCombinedDelegateMethods(IOperation root,
        IBinaryOperation combined, HashSet<IOperation> visited,
        ImmutableDictionary<IMethodSymbol, IInvocationOperation> sites, ImmutableHashSet<TextSpan> queries)
    {
        foreach (var left in GetDelegateMethods(root, combined.LeftOperand, [.. visited], sites, queries))
        {
            foreach (var right in GetDelegateMethods(root, combined.RightOperand, [.. visited], sites, queries))
            {
                yield return left.AddRange(right);
            }
        }
    }

    private static IEnumerable<ImmutableArray<IMethodSymbol>> GetDelegateLocalMethods(IOperation root, ILocalReferenceOperation reference,
        IOperation site, HashSet<IOperation> visited, ImmutableDictionary<IMethodSymbol, IInvocationOperation> sites,
        ImmutableHashSet<TextSpan> queries)
    {
        // A receiver lookup may follow earlier calls, but cannot recursively query the same read site.
        if (queries.Contains(site.Syntax.Span)
            || !TryGetReachingLocalValues(root, site, reference.Local, out var values, out var incoming,
                analyzeReceiverTypes: false, receiverQueries: queries.Add(site.Syntax.Span)))
        {
            yield break;
        }

        foreach (var method in values.SelectMany(candidate => GetDelegateMethods(root, candidate,
                     [.. visited], sites, queries)))
        {
            yield return method;
        }

        // Captures inherit their receiver at the enclosing callable's execution site, not its declaration.
        var enclosing = root.SemanticModel?.GetEnclosingSymbol(site.Syntax.SpanStart) as IMethodSymbol;
        if (incoming && enclosing is not null && sites.TryGetValue(enclosing.OriginalDefinition, out var outer)
            && visited.Add(outer))
        {
            foreach (var method in GetDelegateLocalMethods(root, reference, outer, visited, sites, queries))
            {
                yield return method;
            }
        }
    }

    private static bool CanThrow(IOperation operation)
    {
        if (IsThrowingAccessOrCall(operation) || IsThrowingPropertyAccess(operation))
        {
            return true;
        }

        return operation switch
        {
            IConversionOperation conversion => CanConversionThrow(conversion),
            _ => false,
        };
    }

    private static bool CanSuspend(IAwaitOperation operation) =>
        operation.Operation is not IPropertyReferenceOperation { Property: { IsStatic: true, Name: "CompletedTask" } } property
        || property.Property.ContainingType.ToDisplayString() != "System.Threading.Tasks.Task";

    private static bool IsThrowingAccessOrCall(IOperation operation) => operation is
        IInvocationOperation or IEventAssignmentOperation or IObjectCreationOperation or IDynamicObjectCreationOperation
        or ITypeParameterObjectCreationOperation
        or IArrayCreationOperation or IAwaitOperation or IDynamicInvocationOperation
        or IDynamicIndexerAccessOperation or IDynamicMemberReferenceOperation;

    private static bool IsThrowingPropertyAccess(IOperation operation) => operation switch
    {
        // Setters run after the assigned value; ref-return getters run while evaluating the target.
        IAssignmentOperation { Target: IPropertyReferenceOperation property } =>
            !property.Property.ReturnsByRef && !property.Property.ReturnsByRefReadonly,
        IIncrementOrDecrementOperation { Target: IPropertyReferenceOperation property } =>
            !property.Property.ReturnsByRef && !property.Property.ReturnsByRefReadonly,
        IPropertyReferenceOperation property => !IsSimpleAssignmentTarget(property)
            || property.Property.ReturnsByRef || property.Property.ReturnsByRefReadonly,
        _ => false,
    };

    private static IFieldReferenceOperation? GetFieldAccess(IOperation operation) => operation switch
    {
        // A simple field write checks the receiver after evaluating the assigned value.
        ISimpleAssignmentOperation { Target: IFieldReferenceOperation field } => field,
        IFieldReferenceOperation field when !IsSimpleAssignmentTarget(field) => field,
        _ => null,
    };

    private bool IsThrowingMethodReference(IOperation operation) =>
        operation is IMethodReferenceOperation { Method: { IsStatic: false, ReducedFrom: null } } method
        && HasNullableReceiver(method.Instance);

    private bool HasNullableReceiver(IOperation? receiver) =>
        receiver is { Type.IsReferenceType: true } and not IInstanceReferenceOperation
        && (!analyzeReceiverTypes || !IsKnownNonNull(receiver, [with(SymbolEqualityComparer.Default)]));

    private bool IsKnownNonNull(IOperation value, HashSet<ILocalSymbol> visited)
    {
        while (value is IConversionOperation conversion && PreservesNonNull(conversion))
        {
            value = conversion.Operand;
        }

        if (value is IObjectCreationOperation or IInstanceReferenceOperation || IsNonNullBoxing(value))
        {
            return true;
        }

        if (value is not ILocalReferenceOperation reference || !visited.Add(reference.Local))
        {
            return false;
        }

        var root = _operationRoot;

        // Reuse reaching definitions without recursively proving receivers in that analysis.
        var known = HasOnlyNonNullDefinitions(root, value, reference.Local, visited);
        visited.Remove(reference.Local);
        return known;
    }

    private static bool PreservesNonNull(IConversionOperation conversion) =>
        conversion.Conversion is { IsIdentity: true } or { IsReference: true }
            && (!conversion.IsTryCast || conversion.Conversion.IsImplicit);

    private static bool IsNonNullableValueType(ITypeSymbol? type) =>
        type is { IsValueType: true }
            and not INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T };

    private static bool IsNonNullBoxing(IOperation value) =>
        value is IConversionOperation { Conversion.IsUserDefined: false, Type.IsReferenceType: true } conversion
            && (!conversion.IsTryCast || conversion.Conversion.IsImplicit)
            && IsNonNullableValueType(conversion.Operand.Type);

    private bool HasOnlyNonNullDefinitions(IOperation root, IOperation value, ILocalSymbol localSymbol,
        HashSet<ILocalSymbol> visited) =>
        !HasUntrackedWrites(root, localSymbol, value)
            && TryGetReachingLocalValues(root, value, localSymbol, out var values, out var incoming,
                analyzeReceiverTypes: false)
            && !incoming && !values.IsEmpty && values.All(candidate => IsKnownNonNull(candidate, visited));

    private static bool HasUntrackedWrites(IOperation root, ILocalSymbol local, IOperation use) =>
        root.DescendantsAndSelf().Any(operation => operation switch
        {
            IArgumentOperation { Parameter.RefKind: RefKind.Ref or RefKind.Out } argument =>
                TargetsLocal(argument.Value, local),
            IVariableDeclaratorOperation { Symbol.RefKind: not RefKind.None, Initializer: { } initializer } =>
                TargetsLocal(initializer.Value, local),
            ISimpleAssignmentOperation { IsRef: true } assignment => TargetsLocal(assignment.Value, local),
            IIncrementOrDecrementOperation increment => TargetsLocal(increment.Target, local),
            IAssignmentOperation assignment when TargetsLocal(assignment.Target, local) =>
                assignment is not ISimpleAssignmentOperation { Target: ILocalReferenceOperation }
                || GetEnclosingCallback(assignment) != GetEnclosingCallback(use),
            _ => false,
        });

    private static TextSpan? GetEnclosingCallback(IOperation operation) =>
        operation.Syntax.Ancestors().FirstOrDefault(static syntax => syntax is
            AnonymousFunctionExpressionSyntax or LocalFunctionStatementSyntax)?.Span;

    private static bool TargetsLocal(IOperation target, ILocalSymbol local) => target switch
    {
        ILocalReferenceOperation reference => SymbolEqualityComparer.Default.Equals(reference.Local, local),
        ITupleOperation tuple => tuple.Elements.Any(element => TargetsLocal(element, local)),
        IConditionalOperation conditional => TargetsLocal(conditional.WhenTrue, local)
            || (conditional.WhenFalse is { } whenFalse && TargetsLocal(whenFalse, local)),
        IConversionOperation conversion => TargetsLocal(conversion.Operand, local),
        IParenthesizedOperation parenthesized => TargetsLocal(parenthesized.Operand, local),
        _ => false,
    };

    private static bool CanConversionThrow(IConversionOperation conversion) =>
        conversion.IsChecked || conversion.Conversion.IsUserDefined
        || (!conversion.IsTryCast && !conversion.Conversion.IsIdentity
            && (conversion.Operand.Type?.TypeKind == TypeKind.Dynamic
                || (!conversion.Conversion.IsImplicit && !conversion.Conversion.IsNumeric)));

    private static ArithmeticExceptions GetArithmeticExceptions(IOperation operation) => operation switch
    {
        IBinaryOperation { LeftOperand.Type.TypeKind: TypeKind.Dynamic }
            or IBinaryOperation { RightOperand.Type.TypeKind: TypeKind.Dynamic }
            or ICompoundAssignmentOperation { Target.Type.TypeKind: TypeKind.Dynamic }
            or ICompoundAssignmentOperation { Value.Type.TypeKind: TypeKind.Dynamic }
            or IIncrementOrDecrementOperation { Target.Type.TypeKind: TypeKind.Dynamic }
            or IUnaryOperation { Operand.Type.TypeKind: TypeKind.Dynamic } => ArithmeticExceptions.Unknown,
        IBinaryOperation binary => GetArithmeticExceptions(binary.OperatorKind, binary.IsChecked,
            binary.OperatorMethod, binary.LeftOperand.Type),
        ICompoundAssignmentOperation assignment => assignment.InConversion.IsUserDefined
            || assignment.OutConversion.IsUserDefined
                ? ArithmeticExceptions.Unknown
                : GetArithmeticExceptions(assignment.OperatorKind, assignment.IsChecked,
                    assignment.OperatorMethod, assignment.Target.Type),
        IIncrementOrDecrementOperation increment => GetArithmeticExceptions(BinaryOperatorKind.Add,
            increment.IsChecked, increment.OperatorMethod, increment.Target.Type),
        IUnaryOperation { OperatorMethod: not null } => ArithmeticExceptions.Unknown,
        IUnaryOperation { IsChecked: true } => ArithmeticExceptions.Overflow,
        _ => ArithmeticExceptions.None,
    };

    private static ArithmeticExceptions GetArithmeticExceptions(BinaryOperatorKind operatorKind, bool isChecked,
        IMethodSymbol? operatorMethod, ITypeSymbol? operandType)
    {
        if (operatorMethod is not null)
        {
            return ArithmeticExceptions.Unknown;
        }

        var type = GetArithmeticOperandType(operandType);
        if (type is SpecialType.System_Single or SpecialType.System_Double)
        {
            return ArithmeticExceptions.None;
        }

        return operatorKind switch
        {
            BinaryOperatorKind.Divide or BinaryOperatorKind.Remainder => GetDivisionExceptions(operatorKind, type, isChecked),
            BinaryOperatorKind.Add or BinaryOperatorKind.Subtract or BinaryOperatorKind.Multiply
                when isChecked || type == SpecialType.System_Decimal => ArithmeticExceptions.Overflow,
            _ => ArithmeticExceptions.None,
        };
    }

    private static ArithmeticExceptions GetDivisionExceptions(BinaryOperatorKind operatorKind, SpecialType? type, bool isChecked)
    {
        var canOverflow = type is SpecialType.System_Int32 or SpecialType.System_Int64 or SpecialType.System_IntPtr
            || (operatorKind == BinaryOperatorKind.Divide && CanNarrowOrDecimalDivisionOverflow(type, isChecked));
        return ArithmeticExceptions.DivideByZero
            | (canOverflow ? ArithmeticExceptions.Overflow : ArithmeticExceptions.None);
    }

    private static bool CanNarrowOrDecimalDivisionOverflow(SpecialType? type, bool isChecked) =>
        type == SpecialType.System_Decimal || (isChecked && type is SpecialType.System_SByte or SpecialType.System_Int16);

    private static SpecialType? GetArithmeticOperandType(ITypeSymbol? operandType)
    {
        if (operandType is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } nullable)
        {
            operandType = nullable.TypeArguments[0];
        }

        return operandType is INamedTypeSymbol { EnumUnderlyingType: { } underlyingType }
            ? underlyingType.SpecialType : operandType?.SpecialType;
    }

    [Flags]
    private enum ArithmeticExceptions
    {
        None = 0,
        Unknown = 1,
        DivideByZero = 2,
        Overflow = 4,
    }

    private void Follow(ControlFlowBranch? branch, HashSet<TextSpan> state, ImmutableArray<int> continuation,
        FilterContext? filter)
    {
        if (state.Count == 0 || branch is null || !IsFeasible(branch))
        {
            return;
        }

        if (branch.Semantics == ControlFlowBranchSemantics.Return)
        {
            if (completingReturn && escapingException is null)
            {
                // A handler returning another value abandons the selected factory return.
                return;
            }

            CompleteSelectedReturn(branch, state, continuation, filter);
        }

        if (FollowExceptionalBranch(branch, state, continuation, filter))
        {
            return;
        }

        if (branch.Destination is { } destination)
        {
            FollowDestination(branch, destination, state, continuation, filter);
        }
    }

    private bool FollowExceptionalBranch(ControlFlowBranch branch, HashSet<TextSpan> state,
        ImmutableArray<int> continuation, FilterContext? filter)
    {
        if (branch.Semantics is ControlFlowBranchSemantics.Throw or ControlFlowBranchSemantics.Rethrow)
        {
            var exceptionType = branch.Semantics == ControlFlowBranchSemantics.Rethrow
                ? GetActiveExceptionType(branch.Source) : GetThrownType(branch.Source.BranchValue);
            FollowException(branch.Source, exceptionType, state, continuation, filter);
            return true;
        }

        if (branch.Semantics == ControlFlowBranchSemantics.StructuredExceptionHandling)
        {
            FollowStructuredExceptionHandling(state, continuation, filter);
            return true;
        }

        return false;
    }

    private void CompleteSelectedReturn(ControlFlowBranch branch, HashSet<TextSpan> state,
        ImmutableArray<int> continuation, FilterContext? filter)
    {
        if (invocation is not IReturnOperation { ReturnedValue: { } returnedValue }
            || branch.Source.BranchValue?.Syntax.Span != returnedValue.Syntax.Span
            || branch.Destination is not { } destination)
        {
            return;
        }

        var completion = new ReachingDefinitionFlow(graph, invocation, definitions,
            local, analyzeReceiverTypes, completingReturn: true, analyzeCapturedWrites, callSites, escapingException,
            receiverQueries, completedCleanup, _operationRoot, asyncSuspension)
        {
            _activeExceptions = _activeExceptions,
        };
        completion.FollowDestination(branch, destination, state, continuation, filter);
        completion.AnalyzePendingBlocks();
        _reaching.UnionWith(completion._reaching);
    }

    private void FollowStructuredExceptionHandling(HashSet<TextSpan> state, ImmutableArray<int> continuation,
        FilterContext? filter)
    {
        if (filter is not null)
        {
            RejectFilter(filter, state);
        }
        else if (!continuation.IsEmpty)
        {
            Enqueue(continuation[0], continuation.RemoveAt(0), state);
        }
    }

    private static ITypeSymbol? GetThrownType(IOperation? thrown)
    {
        while (thrown is IConversionOperation conversion)
        {
            thrown = conversion.Operand;
        }

        return thrown is IObjectCreationOperation ? thrown.Type : null;
    }

    private void FollowDestination(
        ControlFlowBranch branch,
        BasicBlock destination,
        HashSet<TextSpan> state,
        ImmutableArray<int> continuation,
        FilterContext? filter)
    {
        var remaining = branch.Semantics == ControlFlowBranchSemantics.Return ? [] : continuation;
        if (filter is not null && IsInRegion(destination, ControlFlowRegionKind.Catch))
        {
            // Filters run during exception search, before intervening finally blocks unwind.
            EnqueueHandler(filter.Source, destination.Ordinal, filter.Cleanup, filter.Continuation, state,
                filter.ExceptionType, filter.PendingCleanup);
            return;
        }

        if (branch.FinallyRegions.IsEmpty)
        {
            Enqueue(destination.Ordinal, remaining, state, filter);
            return;
        }

        var cleanup = branch.FinallyRegions.Select(static region => region.FirstBlockOrdinal)
            .Append(destination.Ordinal).Concat(remaining).ToImmutableArray();
        Enqueue(cleanup[0], cleanup.RemoveAt(0), state, filter);
    }

    private void FollowException(BasicBlock source, ITypeSymbol? exceptionType, HashSet<TextSpan> state,
        ImmutableArray<int> continuation, FilterContext? filter = null, PendingCleanup? pendingCleanup = null)
    {
        if (_awaitFault is { } fault)
        {
            fault.Caller.FollowException(source, exceptionType, state, continuation, filter, pendingCleanup);
            return;
        }

        // An exception in a filter rejects that filter; it does not replace the original exception.
        if (filter is not null)
        {
            ApplyCleanup(pendingCleanup, state, rejected => RejectFilter(filter, rejected));
            return;
        }

        SearchException(source, exceptionType, state, continuation, pendingCleanup: pendingCleanup);
    }

    private void RejectFilter(FilterContext filter, HashSet<TextSpan> state) =>
        SearchException(filter.Source, filter.ExceptionType, state, filter.Continuation, filter.Handler, filter.PendingCleanup);

    private void SearchException(BasicBlock source, ITypeSymbol? exceptionType, HashSet<TextSpan> state,
        ImmutableArray<int> continuation, ControlFlowRegion? rejectedHandler = null, PendingCleanup? pendingCleanup = null)
    {
        var cleanup = ImmutableArray.CreateBuilder<int>();
        for (var region = source.EnclosingRegion; region is not null && region != graph.Root.EnclosingRegion;
             region = region.EnclosingRegion)
        {
            if (region.Kind != ControlFlowRegionKind.Try || region.EnclosingRegion is not { } group)
            {
                continue;
            }

            if (group.Kind == ControlFlowRegionKind.TryAndFinally)
            {
                cleanup.Add(group.NestedRegions.First(static nested => nested.Kind == ControlFlowRegionKind.Finally).FirstBlockOrdinal);
                continue;
            }

            if (SearchHandlers(source, exceptionType, group, cleanup.ToImmutable(), continuation, state,
                    pendingCleanup, ref rejectedHandler))
            {
                return;
            }
        }

        PropagateException(exceptionType, state, cleanup.ToImmutable(), pendingCleanup);
    }

    private bool SearchHandlers(BasicBlock source, ITypeSymbol? exceptionType, ControlFlowRegion group,
        ImmutableArray<int> cleanup, ImmutableArray<int> continuation, HashSet<TextSpan> state,
        PendingCleanup? pendingCleanup, ref ControlFlowRegion? rejectedHandler)
    {
        foreach (var handler in GetHandlers(group))
        {
            if (rejectedHandler is not null)
            {
                if (handler == rejectedHandler)
                {
                    rejectedHandler = null;
                }

                continue;
            }

            if (TryHandler(source, exceptionType, handler, cleanup, continuation, state, pendingCleanup))
            {
                return true;
            }
        }

        return false;
    }

    private void PropagateException(ITypeSymbol? exceptionType, HashSet<TextSpan> state,
        ImmutableArray<int> cleanup, PendingCleanup? pendingCleanup)
    {
        if (escapingException is not null)
        {
            // Caller filters participate in the search before any callee cleanup executes.
            escapingException(exceptionType, state, DeferCleanup(pendingCleanup, cleanup));
        }
        else
        {
            ApplyCleanup(pendingCleanup, state, unwound =>
            {
                if (!cleanup.IsEmpty)
                {
                    Enqueue(cleanup[0], cleanup.RemoveAt(0), unwound);
                }
            });
        }
    }

    private PendingCleanup? DeferCleanup(PendingCleanup? pending, ImmutableArray<int> cleanup)
    {
        if (cleanup.IsEmpty)
        {
            return pending;
        }

        var exceptions = _activeExceptions;
        var key = graph.OriginalOperation.Syntax.Span + ":" + Key(-1, cleanup, null, exceptions) + ":" + pending?.Key;
        return new PendingCleanup(key, (state, completed) => ApplyCleanup(pending, state, unwound =>
        {
            var flow = new ReachingDefinitionFlow(graph, invocation, definitions, local, analyzeReceiverTypes,
                analyzeCapturedWrites: analyzeCapturedWrites, callSites: callSites, escapingException: escapingException,
                receiverQueries: receiverQueries, completedCleanup: completed, operationRoot: _operationRoot,
                asyncSuspension: asyncSuspension)
            {
                _activeExceptions = exceptions,
            };
            var path = cleanup.Add(-1);
            flow.Enqueue(path[0], path.RemoveAt(0), unwound);
            flow.AnalyzePendingBlocks();
        }));
    }

    private static void ApplyCleanup(PendingCleanup? cleanup, HashSet<TextSpan> state, Action<HashSet<TextSpan>> completed)
    {
        if (cleanup is null)
        {
            completed(state);
        }
        else
        {
            cleanup.Apply(state, completed);
        }
    }

    private bool TryHandler(BasicBlock source, ITypeSymbol? exceptionType, ControlFlowRegion handler,
        ImmutableArray<int> cleanup, ImmutableArray<int> continuation, HashSet<TextSpan> state, PendingCleanup? pendingCleanup)
    {
        var caught = handler.Kind == ControlFlowRegionKind.FilterAndHandler
            ? handler.NestedRegions.First(static nested => nested.Kind == ControlFlowRegionKind.Catch).ExceptionType
            : handler.ExceptionType;
        if (!CanCatch(caught, exceptionType))
        {
            return false;
        }

        if (handler.Kind == ControlFlowRegionKind.FilterAndHandler)
        {
            var filter = new FilterContext(source, exceptionType, handler, cleanup, continuation, pendingCleanup);
            Enqueue(handler.FirstBlockOrdinal, [], state, filter);
        }
        else
        {
            EnqueueHandler(source, handler.FirstBlockOrdinal, cleanup, continuation, state, exceptionType, pendingCleanup);
        }

        // An unknown exception may skip a typed handler, but never an unfiltered catch-all.
        // A matching filter continues the search only from its rejected branch.
        return exceptionType is not null || caught is null
            || caught.SpecialType == SpecialType.System_Object || caught.ToDisplayString() == "System.Exception";
    }

    private void EnqueueHandler(BasicBlock source, int handler, ImmutableArray<int> cleanup,
        ImmutableArray<int> continuation, HashSet<TextSpan> state, ITypeSymbol? exceptionType, PendingCleanup? pendingCleanup)
    {
        var remaining = GetHandlerContinuation(source, graph.Blocks[handler], continuation);
        var path = cleanup.Add(handler).AddRange(remaining);
        // Keep each catch's exception separately: a nested handler must not replace an outer rethrow.
        var catchRegion = graph.Blocks[handler].EnclosingRegion;
        while (catchRegion.Kind != ControlFlowRegionKind.Catch && catchRegion.EnclosingRegion is { } parent)
        {
            catchRegion = parent;
        }

        var exceptions = _activeExceptions.SetItem(catchRegion.FirstBlockOrdinal, exceptionType);
        ApplyCleanup(pendingCleanup, state, unwound => Enqueue(path[0], path.RemoveAt(0), unwound, exceptions: exceptions));
    }

    private ITypeSymbol? GetActiveExceptionType(BasicBlock source)
    {
        for (var region = source.EnclosingRegion; region is not null; region = region.EnclosingRegion)
        {
            if (region.Kind == ControlFlowRegionKind.Catch)
            {
                return _activeExceptions.TryGetValue(region.FirstBlockOrdinal, out var type) ? type : null;
            }
        }

        return null;
    }

    private ImmutableArray<int> GetHandlerContinuation(BasicBlock source, BasicBlock handler,
        ImmutableArray<int> continuation)
    {
        var handlerFinally = GetEnclosingFinally(handler);
        if (handlerFinally is null)
        {
            return [];
        }

        if (GetEnclosingFinally(source) == handlerFinally)
        {
            return continuation;
        }

        // A handler in an outer finally abandons destinations inside that finally,
        // but the outer finally must still resume its own pending destination.
        return [.. continuation.SkipWhile(ordinal => ordinal >= handlerFinally.FirstBlockOrdinal
            && ordinal <= handlerFinally.LastBlockOrdinal)];
    }

    private ControlFlowRegion? GetEnclosingFinally(BasicBlock block)
    {
        for (var region = block.EnclosingRegion; region is not null && region != graph.Root.EnclosingRegion;
             region = region.EnclosingRegion)
        {
            if (region.Kind == ControlFlowRegionKind.Finally)
            {
                return region;
            }
        }

        return null;
    }

    private static IEnumerable<ControlFlowRegion> GetHandlers(ControlFlowRegion group) =>
        group.NestedRegions.Where(static nested => nested.Kind is
            ControlFlowRegionKind.Catch or ControlFlowRegionKind.FilterAndHandler);

    private static string Key(int block, ImmutableArray<int> continuation, FilterContext? filter,
        ImmutableDictionary<int, ITypeSymbol?> exceptions) =>
        block + ":" + string.Join(",", continuation) + ":" + filter?.Key + ":"
            + string.Join(";", exceptions.OrderBy(static pair => pair.Key)
                .Select(static pair => pair.Key + "=" + pair.Value?.ToDisplayString()));

    internal sealed class PendingCleanup(string key, Action<HashSet<TextSpan>, Action<HashSet<TextSpan>>> apply)
    {
        public string Key { get; } = key;

        public void Apply(HashSet<TextSpan> state, Action<HashSet<TextSpan>> completed) => apply(state, completed);
    }

    private sealed class FilterContext(BasicBlock source, ITypeSymbol? exceptionType, ControlFlowRegion handler,
        ImmutableArray<int> cleanup, ImmutableArray<int> continuation, PendingCleanup? pendingCleanup)
    {
        public BasicBlock Source { get; } = source;

        public ITypeSymbol? ExceptionType { get; } = exceptionType;

        public ControlFlowRegion Handler { get; } = handler;

        public ImmutableArray<int> Cleanup { get; } = cleanup;

        public ImmutableArray<int> Continuation { get; } = continuation;

        public PendingCleanup? PendingCleanup { get; } = pendingCleanup;

        public string Key { get; } = source.Ordinal + ":" + handler.FirstBlockOrdinal + ":"
            + exceptionType?.ToDisplayString() + ":" + string.Join(",", continuation) + ":" + pendingCleanup?.Key;
    }

    private bool IsInRegion(BasicBlock block, ControlFlowRegionKind kind)
    {
        // A nested handler takes precedence over any outer handler's region.
        for (var region = block.EnclosingRegion; region is not null && region != graph.Root.EnclosingRegion;
             region = region.EnclosingRegion)
        {
            if (region.Kind is ControlFlowRegionKind.Filter or ControlFlowRegionKind.Catch or ControlFlowRegionKind.Finally)
            {
                return region.Kind == kind;
            }
        }

        return false;
    }

    private static bool CanCatch(ITypeSymbol? caught, ITypeSymbol? thrown)
    {
        if (caught is null || thrown is null)
        {
            return true;
        }

        for (var type = thrown; type is not null; type = type.BaseType)
        {
            if (SymbolEqualityComparer.Default.Equals(type, caught))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsFeasible(ControlFlowBranch branch)
    {
        var source = branch.Source;
        return source.ConditionKind == ControlFlowConditionKind.None
            || source.BranchValue?.ConstantValue is not { HasValue: true, Value: bool value }
            || branch.IsConditionalSuccessor == (value == (source.ConditionKind == ControlFlowConditionKind.WhenTrue));
    }

    private static IOperation GetOperationRoot(IOperation operation)
    {
        while (operation.Parent is { } parent)
        {
            operation = parent;
        }

        return operation;
    }

    private static ControlFlowGraph? CreateGraph(IOperation root) => root switch
    {
        IMethodBodyOperation method => ControlFlowGraph.Create(method),
        IConstructorBodyOperation constructor => ControlFlowGraph.Create(constructor),
        IBlockOperation block => ControlFlowGraph.Create(block),
        IFieldInitializerOperation field => ControlFlowGraph.Create(field),
        IPropertyInitializerOperation property => ControlFlowGraph.Create(property),
        _ => null,
    };

    private static ControlFlowGraph? FindGraph(ControlFlowGraph graph, TextSpan invocation)
    {
        var operations = graph.Blocks.SelectMany(static block => block.Operations
                .Concat(block.BranchValue is { } value ? [value] : []))
            .SelectMany(static operation => operation.DescendantsAndSelf()).ToArray();
        if (operations.Any(operation => operation.Syntax.Span == invocation))
        {
            return graph;
        }

        foreach (var local in graph.LocalFunctions)
        {
            if (FindGraph(graph.GetLocalFunctionControlFlowGraph(local), invocation) is { } nested)
            {
                return nested;
            }
        }

        foreach (var anonymous in operations.OfType<IFlowAnonymousFunctionOperation>())
        {
            if (FindGraph(graph.GetAnonymousFunctionControlFlowGraph(anonymous), invocation) is { } nested)
            {
                return nested;
            }
        }

        return null;
    }
}
