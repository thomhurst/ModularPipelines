using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Enums;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Factory for creating ModuleResult instances.
/// </summary>
internal static class ModuleResultFactory
{
    /// <summary>
    /// Creates a successful ModuleResult for the specified value.
    /// </summary>
    public static ModuleResult<T> CreateSuccess<T>(T value, ModuleExecutionContext ctx)
    {
        return ModuleResult<T>.CreateSuccess(value, ctx);
    }

    /// <summary>
    /// Creates a failure ModuleResult for the specified exception (generic version).
    /// </summary>
    public static ModuleResult<T> CreateFailure<T>(Exception exception, ModuleExecutionContext ctx)
    {
        return ModuleResult<T>.CreateFailure(exception, ctx);
    }

    /// <summary>
    /// Creates a failure ModuleResult for the specified exception (non-generic version).
    /// </summary>
    /// <remarks>
    /// This returns the non-generic <see cref="ModuleResult.Failure"/> type, which can be
    /// implicitly converted to <see cref="ModuleResult{T}"/> for any T.
    /// </remarks>
    public static ModuleResult.Failure CreateFailure(Exception exception, ModuleExecutionContext ctx)
    {
        return ModuleResult.CreateFailure(exception, ctx);
    }

    /// <summary>
    /// Creates a skipped ModuleResult for the specified skip decision (generic version).
    /// </summary>
    public static ModuleResult<T> CreateSkipped<T>(SkipDecision decision, ModuleExecutionContext ctx)
    {
        return ModuleResult<T>.CreateSkipped(decision, ctx);
    }

    /// <summary>
    /// Creates a skipped ModuleResult for the specified skip decision (non-generic version).
    /// </summary>
    /// <remarks>
    /// This returns the non-generic <see cref="ModuleResult.Skipped"/> type, which can be
    /// implicitly converted to <see cref="ModuleResult{T}"/> for any T.
    /// </remarks>
    public static ModuleResult.Skipped CreateSkipped(SkipDecision decision, ModuleExecutionContext ctx)
    {
        return ModuleResult.CreateSkipped(decision, ctx);
    }

    /// <summary>
    /// Creates a skipped ModuleResult (type-erased version for engine use).
    /// </summary>
    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Type-erased result creation is used by dynamic and history paths that are unsupported in Native AOT.")]
    public static IModuleResult CreateSkipped(Type resultType, ModuleExecutionContext executionContext)
    {
        var method = typeof(ModuleResultFactory)
            .GetMethod(nameof(CreateSkippedGeneric), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
            .MakeGenericMethod(resultType);

        return (IModuleResult) method.Invoke(null, [executionContext.SkipResult ?? SkipDecision.DoNotSkip, executionContext])!;
    }

    /// <summary>
    /// Creates a failure ModuleResult (type-erased version for engine use).
    /// </summary>
    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Type-erased result creation is used by dynamic and history paths that are unsupported in Native AOT.")]
    public static IModuleResult CreateException(Type resultType, Exception exception, ModuleExecutionContext executionContext)
    {
        var method = typeof(ModuleResultFactory)
            .GetMethod(nameof(CreateFailureGeneric), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
            .MakeGenericMethod(resultType);

        return (IModuleResult) method.Invoke(null, [exception, executionContext])!;
    }

    /// <summary>
    /// Creates a copy of the result with a different status (type-erased version for engine use).
    /// </summary>
    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Type-erased result mutation is used by dynamic and history paths that are unsupported in Native AOT.")]
    public static IModuleResult WithStatus(IModuleResult result, ModuleStatus status)
    {
        // Handle non-generic Failure/Skipped types directly (most efficient path)
        if (result is ModuleResult.Failure failure)
        {
            return failure with { Status = status };
        }

        if (result is ModuleResult.Skipped skipped)
        {
            return skipped with { Status = status };
        }

        // Generic variants require reflection to call WithStatusGeneric<T>, which handles
        // the 'with' expression while preserving the closed ModuleResult<T> type.
        var resultType = result.GetType();

        // Get the generic type argument from ModuleResult<T>
        var genericType = resultType;
        while (genericType != null && (!genericType.IsGenericType || genericType.GetGenericTypeDefinition() != typeof(ModuleResult<>)))
        {
            genericType = genericType.BaseType;
        }

        if (genericType == null)
        {
            throw new InvalidOperationException($"Cannot determine generic type from {resultType}");
        }

        var valueType = genericType.GetGenericArguments()[0];
        var method = typeof(ModuleResultFactory)
            .GetMethod(nameof(WithStatusGeneric), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
            .MakeGenericMethod(valueType);

        return (IModuleResult) method.Invoke(null, [result, status])!;
    }

    private static IModuleResult CreateSkippedGeneric<T>(SkipDecision decision, ModuleExecutionContext ctx)
    {
        return ModuleResult<T>.CreateSkipped(decision, ctx);
    }

    private static IModuleResult CreateFailureGeneric<T>(Exception exception, ModuleExecutionContext ctx)
    {
        return ModuleResult<T>.CreateFailure(exception, ctx);
    }

    private static IModuleResult WithStatusGeneric<T>(ModuleResult<T> result, ModuleStatus status)
    {
        return result with { Status = status };
    }
}
