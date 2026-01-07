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
    /// Creates a failure ModuleResult for the specified exception.
    /// </summary>
    public static ModuleResult<T> CreateFailure<T>(Exception exception, ModuleExecutionContext ctx)
    {
        return ModuleResult<T>.CreateFailure(exception, ctx);
    }

    /// <summary>
    /// Creates a skipped ModuleResult for the specified skip decision.
    /// </summary>
    public static ModuleResult<T> CreateSkipped<T>(SkipDecision decision, ModuleExecutionContext ctx)
    {
        return ModuleResult<T>.CreateSkipped(decision, ctx);
    }

    /// <summary>
    /// Creates a skipped ModuleResult (type-erased version for engine use).
    /// </summary>
    public static IModuleResult CreateSkipped(Type resultType, ModuleExecutionContext executionContext)
    {
        var method = typeof(ModuleResultFactory)
            .GetMethod(nameof(CreateSkippedGeneric), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
            .MakeGenericMethod(resultType);

        return (IModuleResult)method.Invoke(null, [executionContext.SkipResult ?? SkipDecision.DoNotSkip, executionContext])!;
    }

    /// <summary>
    /// Creates a failure ModuleResult (type-erased version for engine use).
    /// </summary>
    public static IModuleResult CreateException(Type resultType, Exception exception, ModuleExecutionContext executionContext)
    {
        var method = typeof(ModuleResultFactory)
            .GetMethod(nameof(CreateFailureGeneric), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
            .MakeGenericMethod(resultType);

        return (IModuleResult)method.Invoke(null, [exception, executionContext])!;
    }

    private static IModuleResult CreateSkippedGeneric<T>(SkipDecision decision, ModuleExecutionContext ctx)
    {
        return ModuleResult<T>.CreateSkipped(decision, ctx);
    }

    private static IModuleResult CreateFailureGeneric<T>(Exception exception, ModuleExecutionContext ctx)
    {
        return ModuleResult<T>.CreateFailure(exception, ctx);
    }
}
