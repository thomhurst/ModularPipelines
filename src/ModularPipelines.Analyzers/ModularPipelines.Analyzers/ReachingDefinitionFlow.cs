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
    bool completingReturn = false)
{
    // A path without a local definition still reads the capture at the enclosing execution site.
    private static readonly TextSpan IncomingValue = new(int.MaxValue, 0);
    private readonly HashSet<TextSpan> _reaching = [];
    private readonly Dictionary<string, HashSet<TextSpan>> _inputs = [];
    private readonly Queue<(int Block, ImmutableArray<int> Continuation, FilterContext? Filter,
        ImmutableDictionary<int, ITypeSymbol?> Exceptions)> _queue = [];
    private ImmutableDictionary<int, ITypeSymbol?> _activeExceptions = ImmutableDictionary<int, ITypeSymbol?>.Empty;

    public static bool TryGetReachingLocalValues(IOperation root, IOperation operation, ILocalSymbol local,
        out ImmutableArray<IOperation> values, out bool inheritsIncomingValue, bool analyzeReceiverTypes = true)
    {
        values = [];
        inheritsIncomingValue = false;
        if (CreateGraph(root) is not { } rootGraph
            || FindGraph(rootGraph, (operation is IReturnOperation { ReturnedValue: { } returnedValue }
                ? returnedValue : operation).Syntax.Span) is not { } graph)
        {
            return false;
        }

        var definitions = root.DescendantsAndSelf().Select(candidate => candidate switch
        {
            IVariableDeclaratorOperation { Initializer: { } initializer } declarator
                when SymbolEqualityComparer.Default.Equals(declarator.Symbol, local) => initializer.Value,
            ISimpleAssignmentOperation { Target: ILocalReferenceOperation target } assignment
                when SymbolEqualityComparer.Default.Equals(target.Local, local) => assignment.Value,
            ICompoundAssignmentOperation { Target: ILocalReferenceOperation target } assignment
                when IsDelegateAddition(assignment)
                    && SymbolEqualityComparer.Default.Equals(target.Local, local) => assignment.Value,
            _ => null,
        }).OfType<IOperation>().GroupBy(static value => value.Syntax.Span)
            .ToDictionary(static group => group.Key, static group => group.First());
        var flow = new ReachingDefinitionFlow(graph, operation, definitions, local, analyzeReceiverTypes);
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

        if (operation.Syntax.Span == invocation.Syntax.Span)
        {
            _reaching.UnionWith(state);
        }

        FollowOperationExceptions(operation, state, block, continuation, filter);
        ApplyDefinition(operation, state, addedTargets);
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
            invocation.SemanticModel?.Compilation.GetTypeByMetadataName(exceptionName), state, continuation, filter);
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

        var root = invocation;
        while (root.Parent is { } parent)
        {
            root = parent;
        }

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

    private void ApplyDefinition(IOperation operation, HashSet<TextSpan> state, HashSet<TextSpan>? addedTargets)
    {
        if (operation is not IAssignmentOperation { Target: ILocalReferenceOperation target } assignment
            || !SymbolEqualityComparer.Default.Equals(target.Local, local)
            || !definitions.ContainsKey(assignment.Value.Syntax.Span))
        {
            return;
        }

        state.Clear();
        if (addedTargets is not null)
        {
            state.UnionWith(addedTargets);
        }

        state.Add(assignment.Value.Syntax.Span);
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

        var root = invocation;
        while (root.Parent is { } parent)
        {
            root = parent;
        }

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
        if (branch is null || !IsFeasible(branch))
        {
            return;
        }

        if (branch.Semantics == ControlFlowBranchSemantics.Return)
        {
            if (completingReturn)
            {
                // A handler returning another value abandons the selected factory return.
                return;
            }

            CompleteSelectedReturn(branch, state, continuation, filter);
        }

        if (branch.Semantics is ControlFlowBranchSemantics.Throw or ControlFlowBranchSemantics.Rethrow)
        {
            var exceptionType = branch.Semantics == ControlFlowBranchSemantics.Rethrow
                ? GetActiveExceptionType(branch.Source) : GetThrownType(branch.Source.BranchValue);
            FollowException(branch.Source, exceptionType, state, continuation, filter);
            return;
        }

        if (branch.Semantics == ControlFlowBranchSemantics.StructuredExceptionHandling)
        {
            FollowStructuredExceptionHandling(state, continuation, filter);
            return;
        }

        if (branch.Destination is not { } destination)
        {
            return;
        }

        FollowDestination(branch, destination, state, continuation, filter);
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
            local, analyzeReceiverTypes, completingReturn: true)
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
            EnqueueHandler(filter.Source, destination.Ordinal, filter.Cleanup, filter.Continuation, state, filter.ExceptionType);
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
        ImmutableArray<int> continuation, FilterContext? filter = null)
    {
        // An exception in a filter rejects that filter; it does not replace the original exception.
        if (filter is not null)
        {
            RejectFilter(filter, state);
            return;
        }

        SearchException(source, exceptionType, state, continuation);
    }

    private void RejectFilter(FilterContext filter, HashSet<TextSpan> state) =>
        SearchException(filter.Source, filter.ExceptionType, state, filter.Continuation, filter.Handler);

    private void SearchException(BasicBlock source, ITypeSymbol? exceptionType, HashSet<TextSpan> state,
        ImmutableArray<int> continuation, ControlFlowRegion? rejectedHandler = null)
    {
        var cleanup = ImmutableArray.CreateBuilder<int>();
        var skipping = rejectedHandler is not null;
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

            foreach (var handler in GetHandlers(group))
            {
                if (skipping)
                {
                    skipping = handler != rejectedHandler;
                    continue;
                }

                if (TryHandler(source, exceptionType, handler, cleanup.ToImmutable(), continuation, state))
                {
                    return;
                }
            }
        }

        if (cleanup.Count > 0)
        {
            var path = cleanup.ToImmutable();
            Enqueue(path[0], path.RemoveAt(0), state);
        }
    }

    private bool TryHandler(BasicBlock source, ITypeSymbol? exceptionType, ControlFlowRegion handler,
        ImmutableArray<int> cleanup, ImmutableArray<int> continuation, HashSet<TextSpan> state)
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
            var filter = new FilterContext(source, exceptionType, handler, cleanup, continuation);
            Enqueue(handler.FirstBlockOrdinal, [], state, filter);
        }
        else
        {
            EnqueueHandler(source, handler.FirstBlockOrdinal, cleanup, continuation, state, exceptionType);
        }

        // An unknown exception may skip a typed handler, but never an unfiltered catch-all.
        // A matching filter continues the search only from its rejected branch.
        return exceptionType is not null || caught is null
            || caught.SpecialType == SpecialType.System_Object || caught.ToDisplayString() == "System.Exception";
    }

    private void EnqueueHandler(BasicBlock source, int handler, ImmutableArray<int> cleanup,
        ImmutableArray<int> continuation, HashSet<TextSpan> state, ITypeSymbol? exceptionType)
    {
        var remaining = GetHandlerContinuation(source, graph.Blocks[handler], continuation);
        var path = cleanup.Add(handler).AddRange(remaining);
        // Keep each catch's exception separately: a nested handler must not replace an outer rethrow.
        var catchRegion = graph.Blocks[handler].EnclosingRegion;
        while (catchRegion.Kind != ControlFlowRegionKind.Catch && catchRegion.EnclosingRegion is { } parent)
        {
            catchRegion = parent;
        }

        Enqueue(path[0], path.RemoveAt(0), state, exceptions:
            _activeExceptions.SetItem(catchRegion.FirstBlockOrdinal, exceptionType));
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

    private sealed class FilterContext(BasicBlock source, ITypeSymbol? exceptionType, ControlFlowRegion handler,
        ImmutableArray<int> cleanup, ImmutableArray<int> continuation)
    {
        public BasicBlock Source { get; } = source;

        public ITypeSymbol? ExceptionType { get; } = exceptionType;

        public ControlFlowRegion Handler { get; } = handler;

        public ImmutableArray<int> Cleanup { get; } = cleanup;

        public ImmutableArray<int> Continuation { get; } = continuation;

        public string Key { get; } = source.Ordinal + ":" + handler.FirstBlockOrdinal + ":"
            + exceptionType?.ToDisplayString() + ":" + string.Join(",", continuation);
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
