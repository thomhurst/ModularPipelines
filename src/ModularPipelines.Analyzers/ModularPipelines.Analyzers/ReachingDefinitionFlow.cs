using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
    IOperation? operationRoot = null)
{
    // A path without a local definition still reads the capture at the enclosing execution site.
    private static readonly TextSpan IncomingValue = new(int.MaxValue, 0);
    // An unsupported local write invalidates only queries it can reach.
    private static readonly TextSpan UnknownValue = new(int.MaxValue - 1, 0);
    private readonly IOperation _operationRoot = operationRoot ?? GetOperationRoot(invocation);
    private readonly HashSet<ILocalSymbol> _aliases = GetAliases(operationRoot ?? GetOperationRoot(invocation), local);
    private readonly HashSet<TextSpan> _reaching = [];
    private readonly Dictionary<string, HashSet<TextSpan>> _inputs = [];
    private readonly Queue<(int Block, ImmutableArray<int> Continuation, FilterContext? Filter,
        ImmutableDictionary<int, ITypeSymbol?> Exceptions)> _queue = [];
    private ImmutableDictionary<int, ITypeSymbol?> _activeExceptions = ImmutableDictionary<int, ITypeSymbol?>.Empty;

    // Query one callable only. Unknown calls can throw, but their bodies never transfer local writes.
    // A selected return contributes only after its finally blocks complete without abandoning it.
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

        var definitions = root.DescendantsAndSelf().SelectMany(candidate => GetAssignedValues(candidate, local))
            .OfType<IOperation>()
            .GroupBy(static value => value.Syntax.Span)
            .ToDictionary(static group => group.Key, static group => group.First());
        var flow = new ReachingDefinitionFlow(graph, operation, definitions, local, analyzeReceiverTypes,
            operationRoot: root);
        flow.Analyze();
        if (flow._reaching.Contains(UnknownValue))
        {
            return false;
        }

        values = [.. flow._reaching.Where(span => span != IncomingValue).Select(span => definitions[span])];
        inheritsIncomingValue = flow._reaching.Contains(IncomingValue);
        return true;
    }

    private void Analyze()
    {
        Enqueue(0, [], [IncomingValue]);
        AnalyzePendingBlocks();
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
        if (state.Count == 0)
        {
            return;
        }

        // Delegate bodies have their own graph and execute only when invoked.
        if (operation is IFlowAnonymousFunctionOperation)
        {
            return;
        }

        if (operation is IDeconstructionAssignmentOperation deconstruction)
        {
            VisitDeconstructionTarget(deconstruction.Target, state, block, continuation, filter);
            Visit(deconstruction.Value, state, block, continuation, filter);
            if (state.Count == 0)
            {
                return;
            }

            if (operation.Syntax.Span == invocation.Syntax.Span)
            {
                _reaching.UnionWith(state);
            }

            // Deconstruct calls and element conversions precede stores; later stores see earlier local writes.
            FollowOperationExceptions(deconstruction, state, block, continuation, filter);
            StoreDeconstructionTarget(deconstruction.Target, deconstruction.Value, state, block, continuation, filter);
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

        if (operation.Syntax.Span == invocation.Syntax.Span)
        {
            _reaching.UnionWith(state);
        }

        // A ref/out callee can mutate before throwing, after all arguments finish evaluating.
        if (GetByReferenceArguments(operation).Any(MayReferenceLocal))
        {
            Invalidate(state);
        }

        FollowOperationExceptions(operation, state, block, continuation, filter);
        ApplyDefinition(operation, state, addedTargets);
    }

    private void VisitDeconstructionTarget(IOperation target, HashSet<TextSpan> state, BasicBlock block,
        ImmutableArray<int> continuation, FilterContext? filter)
    {
        if (target is ITupleOperation or IDeclarationExpressionOperation)
        {
            foreach (var child in target.ChildOperations)
            {
                VisitDeconstructionTarget(child, state, block, continuation, filter);
            }
        }
        else if (target is IPropertyReferenceOperation { Property.ReturnsByRef: true })
        {
            Visit(target, state, block, continuation, filter);
        }
        else
        {
            // Receivers and indices are evaluated now; the actual store happens after the RHS.
            foreach (var child in target.ChildOperations)
            {
                Visit(child, state, block, continuation, filter);
            }
        }
    }

    private void StoreDeconstructionTarget(IOperation target, IOperation? value, HashSet<TextSpan> state,
        BasicBlock block, ImmutableArray<int> continuation, FilterContext? filter)
    {
        if (state.Count == 0)
        {
            return;
        }

        if (target is IDeclarationExpressionOperation declaration)
        {
            target = declaration.Expression;
        }

        if (target is ITupleOperation tuple)
        {
            StoreDeconstructionTuple(tuple, value, state, block, continuation, filter);
            return;
        }

        if (IsAliasTarget(target))
        {
            Invalidate(state);
        }
        else if (target is ILocalReferenceOperation reference
            && SymbolEqualityComparer.Default.Equals(reference.Local, local))
        {
            StoreLocalDefinition(value, state);
        }
        else if (target is not IPropertyReferenceOperation { Property.ReturnsByRef: true })
        {
            FollowOperationExceptions(target, state, block, continuation, filter, deconstructionStore: true, storedValue: value);
        }
    }

    private void StoreLocalDefinition(IOperation? value, HashSet<TextSpan> state) =>
        ApplyAssignedValue(value, state, addedTargets: null);

    private void StoreDeconstructionTuple(ITupleOperation tuple, IOperation? value, HashSet<TextSpan> state,
        BasicBlock block, ImmutableArray<int> continuation, FilterContext? filter)
    {
        while (value is IConversionOperation conversion)
        {
            value = conversion.Operand;
        }

        var values = value as ITupleOperation;
        for (var index = 0; index < tuple.Elements.Length; index++)
        {
            StoreDeconstructionTarget(tuple.Elements[index], values?.Elements[index], state, block,
                continuation, filter);
        }
    }

    private void FollowOperationExceptions(IOperation operation, HashSet<TextSpan> state, BasicBlock block,
        ImmutableArray<int> continuation, FilterContext? filter, bool deconstructionStore = false,
        IOperation? storedValue = null)
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
            if (CanArrayAccessMismatch(operation, array, deconstructionStore, storedValue))
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

    private bool CanArrayAccessMismatch(IOperation operation, IArrayElementReferenceOperation array,
        bool deconstructionStore, IOperation? storedValue) =>
        array.Type is not { IsValueType: true } and not INamedTypeSymbol { IsSealed: true }
        && (deconstructionStore ? storedValue is not { ConstantValue: { HasValue: true, Value: null } }
            : IsWritableArrayAccess(operation))
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

    private static bool HasKnownAssignedValue(IAssignmentOperation assignment) =>
        assignment is ISimpleAssignmentOperation { IsRef: false } or ICoalesceAssignmentOperation
        || (assignment is ICompoundAssignmentOperation compound && IsDelegateAddition(compound));

    private static IEnumerable<IOperation?> GetAssignedValues(IOperation operation, ILocalSymbol localSymbol)
    {
        if (operation is IVariableDeclaratorOperation { Initializer: { } initializer } declarator
            && SymbolEqualityComparer.Default.Equals(declarator.Symbol, localSymbol))
        {
            yield return initializer.Value;
        }
        else if (operation is IAssignmentOperation { Target: ILocalReferenceOperation target } assignment
            && SymbolEqualityComparer.Default.Equals(target.Local, localSymbol))
        {
            yield return HasKnownAssignedValue(assignment) ? assignment.Value : null;
        }
        else if (operation is IIncrementOrDecrementOperation { Target: ILocalReferenceOperation reference }
            && SymbolEqualityComparer.Default.Equals(reference.Local, localSymbol))
        {
            yield return null;
        }
        else if (operation is IDeconstructionAssignmentOperation deconstruction)
        {
            foreach (var value in GetDeconstructedValues(deconstruction.Target, deconstruction.Value, localSymbol))
            {
                yield return value;
            }
        }
        else if (SymbolEqualityComparer.Default.Equals(GetPatternLocal(operation), localSymbol))
        {
            // Pattern matching defines the local, but its extracted value has no independent expression.
            yield return null;
        }
    }

    private static ISymbol? GetPatternLocal(IOperation operation) => operation switch
    {
        IDeclarationPatternOperation pattern => pattern.DeclaredSymbol,
        IRecursivePatternOperation pattern => pattern.DeclaredSymbol,
        IListPatternOperation pattern => pattern.DeclaredSymbol,
        _ => null,
    };

    private static IEnumerable<IOperation?> GetDeconstructedValues(IOperation target, IOperation value,
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

        if (target is not ITupleOperation targets)
        {
            yield break;
        }

        if (value is not ITupleOperation values)
        {
            // The tuple source has no element expressions to return for this write.
            if (TargetsLocal(target, localSymbol))
            {
                yield return null;
            }

            yield break;
        }

        foreach (var element in GetTupleElementValues(targets, values, localSymbol))
        {
            yield return element;
        }
    }

    private static IEnumerable<IOperation?> GetTupleElementValues(ITupleOperation targets, ITupleOperation values,
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

    private static void Invalidate(HashSet<TextSpan> state)
    {
        state.Clear();
        state.Add(UnknownValue);
    }

    private bool IsAliasTarget(IOperation target) =>
        (target is IFlowCaptureReferenceOperation && MayReferenceLocal(target))
        || _aliases.Any(alias => !SymbolEqualityComparer.Default.Equals(alias, local) && TargetsLocal(target, alias));

    private bool MayReferenceLocal(IOperation value) =>
        _aliases.Any(alias => TargetsLocal(value, alias))
        // Conditional ref expressions are lowered to captures; use their original target tree.
        || (value is IFlowCaptureReferenceOperation && _operationRoot.DescendantsAndSelf().Any(candidate =>
            candidate.Syntax.Span == value.Syntax.Span && _aliases.Any(alias => TargetsLocal(candidate, alias))));

    private bool WritesThroughAlias(IOperation operation) => operation switch
    {
        ISimpleAssignmentOperation { IsRef: true } => false,
        IAssignmentOperation assignment => IsAliasTarget(assignment.Target),
        IIncrementOrDecrementOperation increment => IsAliasTarget(increment.Target),
        _ => false,
    };

    private bool TryStoreCoalescingAssignment(IOperation operation, HashSet<TextSpan> state)
    {
        if (operation is not ISimpleAssignmentOperation assignment
            || _operationRoot.SemanticModel?.GetOperation(operation.Syntax) is not ICoalesceAssignmentOperation
            { Target: ILocalReferenceOperation reference }
            || !SymbolEqualityComparer.Default.Equals(reference.Local, local))
        {
            return false;
        }

        // The CFG captures the target location, then lowers the conditional store to a simple assignment.
        StoreLocalDefinition(assignment.Value, state);
        return true;
    }

    private void ApplyDefinition(IOperation operation, HashSet<TextSpan> state, HashSet<TextSpan>? addedTargets)
    {
        if (TryStoreCoalescingAssignment(operation, state))
        {
            return;
        }

        if (WritesThroughAlias(operation))
        {
            Invalidate(state);
            return;
        }

        if (operation is not (IAssignmentOperation or IVariableDeclaratorOperation or IIncrementOrDecrementOperation or IPatternOperation))
        {
            return;
        }

        foreach (var value in GetAssignedValues(operation, local))
        {
            ApplyAssignedValue(value, state, addedTargets);
        }
    }

    private void ApplyAssignedValue(IOperation? value, HashSet<TextSpan> state, HashSet<TextSpan>? addedTargets)
    {
        if (value is not null && !definitions.ContainsKey(value.Syntax.Span))
        {
            // Lowered stores such as foreach iteration variables can have no registered source expression.
            value = null;
        }

        state.Clear();
        if (addedTargets is not null)
        {
            state.UnionWith(addedTargets);
        }

        state.Add(value?.Syntax.Span ?? UnknownValue);
    }

    private bool CanThrow(IOperation operation)
    {
        if (IsThrowingAccessOrCall(operation) || IsThrowingPropertyAccess(operation) || CanImplicitStringCallThrow(operation))
        {
            return true;
        }

        return operation switch
        {
            IRecursivePatternOperation { DeconstructSymbol: not null } => true,
            IListPatternOperation pattern => CanListPatternMemberThrow(pattern.LengthSymbol)
                || CanListPatternMemberThrow(pattern.IndexerSymbol),
            ISlicePatternOperation { SliceSymbol: not null } => true,
            // Deconstruct calls and element conversions need not appear as operation children.
            IDeconstructionAssignmentOperation deconstruction =>
                deconstruction.Syntax is not AssignmentExpressionSyntax syntax
                || _operationRoot.SemanticModel is not { } model
                || CanDeconstructionThrow(model.GetDeconstructionInfo(syntax)),
            IConversionOperation conversion => CanConversionThrow(conversion),
            _ => false,
        };
    }

    private static bool CanFormattingThrow(IInterpolationOperation interpolation) =>
        interpolation.Alignment is not null
        || (interpolation.Expression.Type?.SpecialType != SpecialType.System_String
            && interpolation.Expression is not { ConstantValue: { HasValue: true, Value: null } });

    private bool DefersInterpolationFormatting(IInterpolatedStringOperation text)
    {
        if (_operationRoot.SemanticModel is not { } model)
        {
            return false;
        }

        var destination = model.GetTypeInfo(text.Syntax).ConvertedType;
        return SymbolEqualityComparer.Default.Equals(destination, model.Compilation.GetTypeByMetadataName("System.FormattableString"))
            || SymbolEqualityComparer.Default.Equals(destination, model.Compilation.GetTypeByMetadataName("System.IFormattable"));
    }

    private bool FormatsEachInterpolation(IInterpolatedStringOperation text)
    {
        if (text.Syntax.SyntaxTree.Options is not CSharpParseOptions { LanguageVersion: >= LanguageVersion.CSharp10 }
            || _operationRoot.SemanticModel is not { } model
            || model.Compilation.GetTypeByMetadataName("System.Runtime.CompilerServices.DefaultInterpolatedStringHandler") is null)
        {
            return false;
        }

        // Await and dynamic holes require the factory lowering instead of a ref-struct handler.
        if (text.DescendantsAndSelf().Any(static part => part is IAwaitOperation || part.Type?.TypeKind == TypeKind.Dynamic))
        {
            return false;
        }

        var expressionType = model.Compilation.GetTypeByMetadataName("System.Linq.Expressions.Expression`1");
        return !text.Syntax.Ancestors().OfType<AnonymousFunctionExpressionSyntax>().Any(lambda =>
            model.GetTypeInfo(lambda).ConvertedType is INamedTypeSymbol converted
            && SymbolEqualityComparer.Default.Equals(converted.OriginalDefinition, expressionType));
    }

    // Handlers format after each hole; string.Format evaluates every argument before formatting.
    // FormattableString/IFormattable defer formatting, unlike conversions of the finished string to object/interfaces.
    private bool CanInterpolationThrow(IInterpolationOperation interpolation) =>
        interpolation.Parent is IInterpolatedStringOperation text
        && !DefersInterpolationFormatting(text) && FormatsEachInterpolation(text) && CanFormattingThrow(interpolation);

    private bool CanCompletedInterpolationThrow(IInterpolatedStringOperation text) =>
        !DefersInterpolationFormatting(text) && !FormatsEachInterpolation(text)
        && text.Parts.OfType<IInterpolationOperation>().Any(CanFormattingThrow);

    // Formatting and concatenation can call user code without an invocation child.
    private bool CanImplicitStringCallThrow(IOperation operation) => operation switch
    {
        IInterpolationOperation interpolation => CanInterpolationThrow(interpolation),
        IInterpolatedStringOperation text => CanCompletedInterpolationThrow(text),
        IBinaryOperation { OperatorKind: BinaryOperatorKind.Add, Type.SpecialType: SpecialType.System_String } binary =>
            CanConcatenationOperandThrow(binary.LeftOperand) || CanConcatenationOperandThrow(binary.RightOperand),
        ICompoundAssignmentOperation { OperatorKind: BinaryOperatorKind.Add } assignment
            when assignment.Target.Type?.SpecialType == SpecialType.System_String
                || assignment.Value.Type?.SpecialType == SpecialType.System_String =>
            CanConcatenationOperandThrow(assignment.Target) || CanConcatenationOperandThrow(assignment.Value),
        _ => false,
    };

    private static bool CanConcatenationOperandThrow(IOperation operand) =>
        operand.Type?.SpecialType != SpecialType.System_String && !operand.ConstantValue.HasValue;

    // List patterns guard null and bounds before intrinsic array/string member accesses.
    private static bool CanListPatternMemberThrow(ISymbol? member) => member is not null
        && member.ContainingType.SpecialType is not (SpecialType.System_Array or SpecialType.System_String);

    private static bool CanDeconstructionThrow(DeconstructionInfo info) =>
        info.Method is not null || info.Conversion is { IsUserDefined: true } or { IsDynamic: true }
        || (!info.Nested.IsDefaultOrEmpty && info.Nested.Any(CanDeconstructionThrow));

    private static bool IsThrowingAccessOrCall(IOperation operation) => operation is
        IInvocationOperation or IEventAssignmentOperation or IObjectCreationOperation or IDynamicObjectCreationOperation
        or ITypeParameterObjectCreationOperation
        or IArrayCreationOperation or IAwaitOperation or IDynamicInvocationOperation;

    private static bool IsThrowingPropertyAccess(IOperation operation) => operation switch
    {
        // Setters run after the assigned value; ref-return getters run while evaluating the target.
        IAssignmentOperation { Target: IDynamicMemberReferenceOperation or IDynamicIndexerAccessOperation } => true,
        IIncrementOrDecrementOperation { Target: IDynamicMemberReferenceOperation or IDynamicIndexerAccessOperation } => true,
        IDynamicMemberReferenceOperation or IDynamicIndexerAccessOperation => !IsSimpleAssignmentTarget(operation),
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

    private static IEnumerable<IOperation> GetByReferenceArguments(IOperation operation)
    {
        foreach (var argument in operation.ChildOperations.OfType<IArgumentOperation>())
        {
            if (argument.Parameter?.RefKind is RefKind.Ref or RefKind.Out)
            {
                yield return argument.Value;
            }
        }

        var dynamicArguments = operation switch
        {
            IDynamicInvocationOperation invocation => invocation.Arguments,
            IDynamicObjectCreationOperation creation => creation.Arguments,
            _ => [],
        };
        for (var index = 0; index < dynamicArguments.Length; index++)
        {
            var refKind = operation switch
            {
                IDynamicInvocationOperation invocation => invocation.GetArgumentRefKind(index),
                IDynamicObjectCreationOperation creation => creation.GetArgumentRefKind(index),
                _ => null,
            };
            if (refKind is RefKind.Ref or RefKind.Out)
            {
                yield return dynamicArguments[index];
            }
        }
    }

    // Bindings are a conservative callable-wide union; only traversed writes invalidate flow.
    private static HashSet<ILocalSymbol> GetAliases(IOperation root, ILocalSymbol local)
    {
        HashSet<ILocalSymbol> aliases = [with(SymbolEqualityComparer.Default), local];
        var sources = root.DescendantsAndSelf().SelectMany(GetAliasSources).ToArray();
        bool changed;
        do
        {
            changed = false;
            foreach (var (alias, source) in sources)
            {
                if (!aliases.Contains(alias) && aliases.Any(candidate => TargetsLocal(source, candidate)))
                {
                    changed |= aliases.Add(alias);
                }
            }
        }
        while (changed);

        return aliases;
    }

    private static IEnumerable<(ILocalSymbol Alias, IOperation Source)> GetAliasSources(IOperation operation)
    {
        if (operation is IVariableDeclaratorOperation { Symbol.RefKind: not RefKind.None, Initializer: { } initializer } declarator)
        {
            yield return (declarator.Symbol, initializer.Value);
        }
        else if (operation is ISimpleAssignmentOperation { IsRef: true, Target: ILocalReferenceOperation reference } assignment)
        {
            yield return (reference.Local, assignment.Value);
        }
    }

    private static bool TargetsLocal(IOperation target, ILocalSymbol local) => target switch
    {
        ILocalReferenceOperation reference => SymbolEqualityComparer.Default.Equals(reference.Local, local),
        IDeclarationExpressionOperation declaration => TargetsLocal(declaration.Expression, local),
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
            if (completingReturn)
            {
                // A handler returning another value abandons the selected return.
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
            local, analyzeReceiverTypes, completingReturn: true, operationRoot: _operationRoot)
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
                filter.ExceptionType);
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
                    ref rejectedHandler))
            {
                return;
            }
        }

        FollowEscapingCleanup(state, cleanup.ToImmutable());
    }

    private bool SearchHandlers(BasicBlock source, ITypeSymbol? exceptionType, ControlFlowRegion group,
        ImmutableArray<int> cleanup, ImmutableArray<int> continuation, HashSet<TextSpan> state, ref ControlFlowRegion? rejectedHandler)
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

            if (TryHandler(source, exceptionType, handler, cleanup, continuation, state))
            {
                return true;
            }
        }

        return false;
    }

    private void FollowEscapingCleanup(HashSet<TextSpan> state,
        ImmutableArray<int> cleanup)
    {
        if (!cleanup.IsEmpty)
        {
            Enqueue(cleanup[0], cleanup.RemoveAt(0), state);
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

        var exceptions = _activeExceptions.SetItem(catchRegion.FirstBlockOrdinal, exceptionType);
        Enqueue(path[0], path.RemoveAt(0), state, exceptions: exceptions);
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
