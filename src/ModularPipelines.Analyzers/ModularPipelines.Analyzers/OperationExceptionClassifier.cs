using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace ModularPipelines.Analyzers;

/// <summary>Classifies exceptions without traversing control flow or discovering local definitions.</summary>
/// <param name="semanticModel">The source model, including implicit calls omitted from operation children.</param>
/// <param name="isKnownNonNull">Optional contextual proof; unknown receivers remain nullable.</param>
/// <param name="hasExactArrayType">Optional contextual proof that an array has its declared runtime type.</param>
internal sealed class OperationExceptionClassifier(
    SemanticModel? semanticModel = null,
    Func<IOperation, bool>? isKnownNonNull = null,
    Func<IOperation, bool>? hasExactArrayType = null)
{
    /// <summary>Exceptions at completion of this operation, after its children have evaluated.</summary>
    /// <remarks>A null metadata name denotes an unknown exception from user code or dynamic binding.
    /// Assignment targets must also be classified at their own evaluation point: ordinary setters,
    /// field stores and array stores check at assignment completion, ref getters at target evaluation.</remarks>
    internal ImmutableArray<string?> Classify(IOperation operation, bool deconstructionStore = false,
        IOperation? storedValue = null)
    {
        var arithmetic = GetArithmeticExceptions(operation);
        if (GetReceiverExceptionName(operation, deconstructionStore) is { } receiver)
        {
            return [receiver];
        }

        var result = ImmutableArray.CreateBuilder<string?>();
        if (GetArrayAccess(operation) is { } array)
        {
            // Range slices lower to GetSubArray, whose exceptions differ from element access.
            if (array.Indices.Length == 1 && array.Indices[0].Type is INamedTypeSymbol { Name: "Range" } rangeType
                && rangeType.ToDisplayString() == "System.Range")
            {
                return [null];
            }

            result.Add("System.NullReferenceException");
            result.Add("System.IndexOutOfRangeException");
            if (CanArrayAccessMismatch(operation, array, deconstructionStore, storedValue))
            {
                result.Add("System.ArrayTypeMismatchException");
            }
        }

        if (arithmetic == ArithmeticExceptions.Unknown || CanThrow(operation))
        {
            result.Add(null);
            return result.ToImmutable();
        }

        if ((arithmetic & ArithmeticExceptions.DivideByZero) != 0)
        {
            result.Add("System.DivideByZeroException");
        }

        if ((arithmetic & ArithmeticExceptions.Overflow) != 0)
        {
            result.Add("System.OverflowException");
        }

        return result.ToImmutable();
    }

    /// <summary>Element ranges preceded by implicit constructor, Add or spread calls.
    /// These survive CFG hoisting; callers must dispatch only on entry into each element.</summary>
    internal static ImmutableArray<TextSpan> GetEarlyCollectionCallSpans(IOperation root) =>
        [.. GetCollectionCallSpans(root)];

    private string? GetReceiverExceptionName(IOperation operation, bool deconstructionStore)
    {
        var field = GetFieldAccess(operation);
        var eventAccess = GetEventAccess(operation);
        if (field is { Field: { IsStatic: true, IsConst: false } } || eventAccess is { Event.IsStatic: true })
        {
            return "System.TypeInitializationException";
        }

        return DereferencesRefField(operation, field, deconstructionStore) || (field is not null && HasNullableReceiver(field.Instance)) || (eventAccess is not null && HasNullableReceiver(eventAccess.Instance)) || IsThrowingMethodReference(operation) ? "System.NullReferenceException" : null;
    }

    private static bool DereferencesRefField(IOperation operation, IFieldReferenceOperation? field, bool deconstructionStore)
    {
        if (field is not { Field.RefKind: not RefKind.None }
            || operation is ISimpleAssignmentOperation { IsRef: true }
            || field.Syntax.Parent is RefExpressionSyntax
            || field.Syntax.Parent is ArgumentSyntax { RefKindKeyword.RawKind: not 0 })
        {
            return false;
        }

        // CFG captures of assignment locations precede the RHS and do not read the referent.
        // Deconstruction completion explicitly classifies the target after all RHS values.
        var target = field.Syntax;
        while (target.Parent is ArgumentSyntax { Parent: TupleExpressionSyntax tuple })
        {
            target = tuple;
        }

        return deconstructionStore || operation is ISimpleAssignmentOperation
            || target.Parent is not AssignmentExpressionSyntax assignment
            || !assignment.IsKind(SyntaxKind.SimpleAssignmentExpression)
            || !ReferenceEquals(assignment.Left, target);
    }

    private static bool IsSimpleAssignmentTarget(IOperation operation) =>
        operation.Parent is ISimpleAssignmentOperation assignment && ReferenceEquals(assignment.Target, operation);

    private static IArrayElementReferenceOperation? GetArrayAccess(IOperation operation) => operation switch
    {
        // A store checks the array after evaluating its assigned value.
        IAssignmentOperation { Target: IArrayElementReferenceOperation array } => array,
        IIncrementOrDecrementOperation { Target: IArrayElementReferenceOperation array } => array,
        IArrayElementReferenceOperation array when !IsSimpleAssignmentTarget(array) => array,
        _ => null,
    };

    private bool CanArrayAccessMismatch(IOperation operation, IArrayElementReferenceOperation array,
        bool deconstructionStore, IOperation? storedValue) =>
        array.Type is not { IsValueType: true } and not INamedTypeSymbol { IsSealed: true }
        && (deconstructionStore ? storedValue is not { ConstantValue: { HasValue: true, Value: null } } : IsWritableArrayAccess(operation))
        && hasExactArrayType?.Invoke(array.ArrayReference) != true;

    private bool IsWritableArrayAccess(IOperation operation) => GetRefConditionalContext(operation) switch
    {
        IConditionalOperation { IsRef: true, Parent: IIncrementOrDecrementOperation increment } conditional
            when ReferenceEquals(conditional, increment.Target) => true,
        // Source operations retain ref-local initializers; the CFG lowers these to ref assignments.
        IOperation
        {
            Parent: IVariableInitializerOperation { Parent: IVariableDeclaratorOperation { Symbol.RefKind: RefKind.Ref } }
        }
        => true,
        // Taking a writable ref return checks covariance even before a value is stored.
        // ReturnsByRef excludes ref readonly returns, whose element reference cannot be written.
        IOperation { Parent: IReturnOperation } when operation is IArrayElementReferenceOperation or IConditionalOperation =>
            (semanticModel ?? operation.SemanticModel)?.GetEnclosingSymbol(operation.Syntax.SpanStart)
                is IMethodSymbol { ReturnsByRef: true },
        ICompoundAssignmentOperation { Target: IArrayElementReferenceOperation } => true,
        IIncrementOrDecrementOperation { Target: IArrayElementReferenceOperation } => true,
        IAssignmentOperation assignment =>
            !(assignment.Value.ConstantValue.HasValue && assignment.Value.ConstantValue.Value is null),
        // A ref conditional obtains a writable element reference even when assigning null.
        IConditionalOperation { IsRef: true, Parent: IAssignmentOperation assignment }
            conditional when ReferenceEquals(assignment.Target, conditional) => true,
        IOperation
        {
            Parent: ISimpleAssignmentOperation
            {
                IsRef: true,
                Target: ILocalReferenceOperation { Local.RefKind: RefKind.Ref }
                    or IParameterReferenceOperation { Parameter.RefKind: RefKind.Ref or RefKind.Out }
                    or IFieldReferenceOperation { Field.RefKind: RefKind.Ref }
            }
        }
        => true,
        IOperation
        {
            Parent: IArgumentOperation { Parameter.RefKind: RefKind.Ref or RefKind.Out }
        }
        => true,
        _ => false,
    };

    private static IOperation GetRefConditionalContext(IOperation operation)
    {
        while (operation.Parent is IConditionalOperation { IsRef: true } conditional)
        {
            operation = conditional;
        }

        return operation;
    }

    private static IEnumerable<TextSpan> GetCollectionCallSpans(IOperation root)
    {
        foreach (var collection in root.DescendantsAndSelf().OfType<ICollectionExpressionOperation>())
        {
            var hasEarlyCall = collection.ConstructMethod?.MethodKind == MethodKind.Constructor;
            foreach (var element in collection.Elements)
            {
                if (hasEarlyCall)
                {
                    yield return element.Syntax.Span;
                }

                hasEarlyCall |= element is ISpreadOperation;
            }
        }
    }

    private bool CanThrow(IOperation operation)
    {
        if (IsThrowingAccessOrCall(operation) || IsThrowingPropertyAccess(operation) || CanImplicitStringCallThrow(operation))
        {
            return true;
        }

        return operation switch
        {
            // Construction, Add, and spread iteration can invoke user code without invocation children.
            ICollectionExpressionOperation collection => collection.ConstructMethod is not null
                || collection.Elements.Any(static element => element is ISpreadOperation),
            ISpreadOperation => true,
            IRecursivePatternOperation { DeconstructSymbol: not null } => true,
            IListPatternOperation pattern => CanListPatternMemberThrow(pattern.LengthSymbol)
                || CanListPatternMemberThrow(pattern.IndexerSymbol),
            ISlicePatternOperation { SliceSymbol: not null } => true,
            // Deconstruct calls and element conversions need not appear as operation children.
            IDeconstructionAssignmentOperation deconstruction =>
                deconstruction.Syntax is not AssignmentExpressionSyntax syntax
                || semanticModel is not { } model
                || CanDeconstructionThrow(model.GetDeconstructionInfo(syntax)),
            // The non-null branch conversion is metadata, not an operation child.
            ICoalesceOperation coalesce => CanCoalesceConversionThrow(coalesce),
            // Tuple comparisons hide their element operators from the operation children.
            ITupleBinaryOperation comparison => CanTupleComparisonThrow(comparison),
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
        if (semanticModel is not { } model)
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
            || semanticModel is not { } model
            || model.Compilation.GetTypeByMetadataName("System.Runtime.CompilerServices.DefaultInterpolatedStringHandler") is null)
        {
            return false;
        }

        // Await and dynamic holes require the factory lowering instead of a ref-struct handler.
        if (EvaluatedInterpolationParts(text).Any(static part => part is IAwaitOperation || part.Type?.TypeKind == TypeKind.Dynamic))
        {
            return false;
        }

        var expressionType = model.Compilation.GetTypeByMetadataName("System.Linq.Expressions.Expression`1");
        return !text.Syntax.Ancestors().OfType<AnonymousFunctionExpressionSyntax>().Any(lambda =>
            model.GetTypeInfo(lambda).ConvertedType is INamedTypeSymbol converted
            && SymbolEqualityComparer.Default.Equals(converted.OriginalDefinition, expressionType));
    }

    // Deferred function bodies do not affect the enclosing string's lowering.
    private static IEnumerable<IOperation> EvaluatedInterpolationParts(IOperation operation)
    {
        yield return operation;
        if (operation is IAnonymousFunctionOperation or ILocalFunctionOperation)
        {
            yield break;
        }

        foreach (var child in operation.ChildOperations)
        {
            foreach (var part in EvaluatedInterpolationParts(child))
            {
                yield return part;
            }
        }
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
        IInvocationOperation or IFunctionPointerInvocationOperation
        or IEventAssignmentOperation or IObjectCreationOperation or IDynamicObjectCreationOperation
        or ITypeParameterObjectCreationOperation
        or IArrayCreationOperation or IAwaitOperation or IDynamicInvocationOperation
        // The implicit System.Index constructor is absent from the operation children.
        or IUnaryOperation { OperatorKind: UnaryOperatorKind.Hat }
        or IWithOperation { CloneMethod: not null }
        // Length and indexer/Slice calls are implicit, with no callable operation children.
        or IImplicitIndexerReferenceOperation;

    private static bool IsThrowingPropertyAccess(IOperation operation) => operation switch
    {
        // Setters run after the assigned value; ref-return getters run while evaluating the target.
        IAssignmentOperation assignment => IsThrowingSetter(assignment.Target),
        IIncrementOrDecrementOperation increment => IsThrowingSetter(increment.Target),
        IDynamicMemberReferenceOperation or IDynamicIndexerAccessOperation => !IsSimpleAssignmentTarget(operation),
        IPropertyReferenceOperation property => !IsSimpleAssignmentTarget(property)
            || property.Property.ReturnsByRef || property.Property.ReturnsByRefReadonly,
        _ => false,
    };

    private static bool IsThrowingSetter(IOperation target) => target switch
    {
        IDynamicMemberReferenceOperation or IDynamicIndexerAccessOperation => true,
        IImplicitIndexerReferenceOperation { IndexerSymbol: IPropertySymbol property } => IsValueProperty(property),
        IPropertyReferenceOperation property => IsValueProperty(property.Property),
        _ => false,
    };

    private static bool IsValueProperty(IPropertySymbol property) =>
        !property.ReturnsByRef && !property.ReturnsByRefReadonly;

    private static IFieldReferenceOperation? GetFieldAccess(IOperation operation) => operation switch
    {
        // A simple field write checks the receiver after evaluating the assigned value.
        ISimpleAssignmentOperation { Target: IFieldReferenceOperation field } => field,
        IFieldReferenceOperation field when !IsSimpleAssignmentTarget(field) => field,
        _ => null,
    };

    // Field-like event reads and direct writes use backing-field timing. Add/remove calls
    // are classified separately at event-assignment completion.
    private static IEventReferenceOperation? GetEventAccess(IOperation operation) => operation switch
    {
        ISimpleAssignmentOperation { Target: IEventReferenceOperation eventAccess } => eventAccess,
        IEventReferenceOperation eventAccess when !IsSimpleAssignmentTarget(eventAccess)
            && eventAccess.Parent is not IEventAssignmentOperation => eventAccess,
        _ => null,
    };

    private bool IsThrowingMethodReference(IOperation operation) =>
        operation is IMethodReferenceOperation { Method: { IsStatic: false, ReducedFrom: null } } method
        && HasNullableReceiver(method.Instance);

    private bool HasNullableReceiver(IOperation? receiver) =>
        receiver is { Type: { IsReferenceType: true } or ITypeParameterSymbol { IsValueType: false } }
            and not IInstanceReferenceOperation
        && isKnownNonNull?.Invoke(receiver) != true;

    private bool CanConversionThrow(IConversionOperation conversion) =>
        conversion.IsChecked || conversion.Conversion.IsUserDefined || CanNumericConversionOverflow(conversion)
        || CanTupleConversionThrow(conversion)
        || (!conversion.IsTryCast && !conversion.Conversion.IsIdentity
            && (conversion.Operand.Type?.TypeKind == TypeKind.Dynamic
                || (!conversion.Conversion.IsImplicit && !conversion.Conversion.IsNumeric
                    && !IsNullableTargetConversion(conversion) && !IsSafeEnumerationConversion(conversion))));

    private bool CanCoalesceConversionThrow(ICoalesceOperation coalesce) =>
        coalesce.ValueConversion.IsUserDefined
        || CanTupleConversionThrow(coalesce.Value.Type, coalesce.Type, coalesce);

    // Tuple-valued operands hide their element conversions from the operation children.
    private bool CanTupleConversionThrow(IConversionOperation conversion) =>
        CanTupleConversionThrow(conversion.Operand.Type, conversion.Type, conversion);

    private bool CanTupleConversionThrow(ITypeSymbol? sourceType, ITypeSymbol? targetType, IOperation operation) =>
        GetTupleType(sourceType) is { } source
        && GetTupleType(targetType) is { } target
        && CanTupleElementsThrow(source, target,
            (semanticModel ?? operation.SemanticModel)?.Compilation as CSharpCompilation);

    private static bool CanTupleComparisonThrow(ITupleBinaryOperation comparison) =>
        CanTupleElementComparisonThrow(comparison.LeftOperand.Type)
        || CanTupleElementComparisonThrow(comparison.RightOperand.Type);

    private static bool CanTupleElementComparisonThrow(ITypeSymbol? type) =>
        GetTupleType(type) is { } tuple
            ? tuple.TupleElements.Any(static element => CanTupleElementComparisonThrow(element.Type))
            // Only known built-in types exclude user-defined operators and conversions.
            : type?.TypeKind == TypeKind.Dynamic || GetArithmeticOperandType(type) is null or SpecialType.None;

    private static INamedTypeSymbol? GetTupleType(ITypeSymbol? type) => type switch
    {
        INamedTypeSymbol { IsTupleType: true } tuple => tuple,
        INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } nullable
            => nullable.TypeArguments[0] is INamedTypeSymbol { IsTupleType: true } tuple ? tuple : null,
        _ => null,
    };

    private static bool CanTupleElementsThrow(INamedTypeSymbol source, INamedTypeSymbol target,
        CSharpCompilation? compilation)
    {
        for (var index = 0; index < source.TupleElements.Length; index++)
        {
            var from = source.TupleElements[index].Type;
            var to = target.TupleElements[index].Type;
            if (SymbolEqualityComparer.Default.Equals(from, to))
            {
                continue;
            }

            if (GetTupleType(from) is { } nestedSource
                && GetTupleType(to) is { } nestedTarget)
            {
                if (CanTupleElementsThrow(nestedSource, nestedTarget, compilation))
                {
                    return true;
                }

                continue;
            }

            // Built-in implicit conversions preserve values; user code and explicit conversions
            // can throw (including decimal narrowing, unboxing and nullable unwrapping).
            if (from.TypeKind == TypeKind.Dynamic || compilation is null
                || compilation.ClassifyConversion(from, to) is not { IsImplicit: true, IsUserDefined: false })
            {
                return true;
            }
        }

        return false;
    }

    private bool IsSafeEnumerationConversion(IConversionOperation conversion) =>
        conversion.GetConversion().IsEnumeration
        && conversion.Operand.Type is not INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T }
        && !IsInCheckedContext(conversion);

    // Roslyn does not set IConversionOperation.IsChecked for every enumeration conversion.
    internal bool IsInCheckedContext(IOperation operation)
    {
        foreach (var syntax in operation.Syntax.Ancestors())
        {
            // Overflow context does not cross function-body boundaries.
            if (syntax is AnonymousFunctionExpressionSyntax or LocalFunctionStatementSyntax or MemberDeclarationSyntax)
            {
                break;
            }

            if (syntax is CheckedExpressionSyntax expression)
            {
                return expression.Keyword.IsKind(SyntaxKind.CheckedKeyword);
            }

            if (syntax is CheckedStatementSyntax statement)
            {
                return statement.Keyword.IsKind(SyntaxKind.CheckedKeyword);
            }
        }

        return semanticModel?.Compilation.Options is CSharpCompilationOptions { CheckOverflow: true };
    }

    // CommonConversion does not mark lifted numeric conversions as numeric. A nullable target preserves null;
    // a non-nullable target can still throw when unwrapping an empty nullable value.
    private static bool IsNullableTargetConversion(IConversionOperation conversion) =>
        conversion.Type is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T }
        && conversion.GetConversion().IsNullable;

    // Decimal narrowing and floating-point-to-decimal conversions can overflow even in unchecked code.
    private static bool CanNumericConversionOverflow(IConversionOperation conversion) =>
        CanNumericConversionOverflow(conversion.Operand.Type, conversion.Type);

    internal static bool CanNumericConversionOverflow(ITypeSymbol? source, ITypeSymbol? destination) =>
        GetArithmeticOperandType(source) switch
        {
            SpecialType.System_Decimal => GetArithmeticOperandType(destination) is
                SpecialType.System_SByte or SpecialType.System_Byte or SpecialType.System_Int16 or SpecialType.System_UInt16
                or SpecialType.System_Int32 or SpecialType.System_UInt32 or SpecialType.System_Int64 or SpecialType.System_UInt64
                or SpecialType.System_IntPtr or SpecialType.System_UIntPtr or SpecialType.System_Char,
            SpecialType.System_Single or SpecialType.System_Double =>
                GetArithmeticOperandType(destination) == SpecialType.System_Decimal,
            _ => false,
        };

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

        return operandType is INamedTypeSymbol { EnumUnderlyingType: { } underlyingType } ? underlyingType.SpecialType : operandType?.SpecialType;
    }

    [Flags]
    private enum ArithmeticExceptions
    {
        None = 0,
        Unknown = 1,
        DivideByZero = 2,
        Overflow = 4,
    }
}
