using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Factory for creating ModuleResult instances without reflection.
/// </summary>
/// <remarks>
/// Replaces reflection-based constructor invocation with compiled expression trees.
/// </remarks>
internal static class ModuleResultFactory
{
    /// <summary>
    /// Delegate for creating a ModuleResult with a null value (for skipped modules).
    /// </summary>
    internal delegate IModuleResult CreateSkippedResultDelegate(ModuleExecutionContext executionContext);

    /// <summary>
    /// Delegate for creating a ModuleResult with an exception.
    /// </summary>
    internal delegate IModuleResult CreateExceptionResultDelegate(Exception exception, ModuleExecutionContext executionContext);

    private static readonly ConcurrentDictionary<Type, CreateSkippedResultDelegate> SkippedResultCache = new();
    private static readonly ConcurrentDictionary<Type, CreateExceptionResultDelegate> ExceptionResultCache = new();

    /// <summary>
    /// Creates a skipped ModuleResult for the specified result type.
    /// </summary>
    public static IModuleResult CreateSkipped(Type resultType, ModuleExecutionContext executionContext)
    {
        var factory = SkippedResultCache.GetOrAdd(resultType, CreateSkippedFactory);
        return factory(executionContext);
    }

    /// <summary>
    /// Creates an exception ModuleResult for the specified result type.
    /// </summary>
    public static IModuleResult CreateException(Type resultType, Exception exception, ModuleExecutionContext executionContext)
    {
        var factory = ExceptionResultCache.GetOrAdd(resultType, CreateExceptionFactory);
        return factory(exception, executionContext);
    }

    private static CreateSkippedResultDelegate CreateSkippedFactory(Type resultType)
    {
        var contextParam = Expression.Parameter(typeof(ModuleExecutionContext), "executionContext");
        var resultGenericType = typeof(ModuleResult<>).MakeGenericType(resultType);
        var typedContextType = typeof(ModuleExecutionContext<>).MakeGenericType(resultType);

        // Cast to typed context
        var castContext = Expression.Convert(contextParam, typedContextType);

        // Find the internal constructor: ModuleResult<T>(T? value, ModuleExecutionContext context)
        var constructor = resultGenericType.GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            new[] { resultType, typeof(ModuleExecutionContext) },
            null);

        if (constructor == null)
        {
            throw new InvalidOperationException(
                $"Could not find internal constructor for ModuleResult<{resultType.Name}>(T?, ModuleExecutionContext)");
        }

        // Create: new ModuleResult<T>(default(T), executionContext)
        var defaultValue = Expression.Default(resultType);
        var newResult = Expression.New(constructor, defaultValue, contextParam);

        // Cast to IModuleResult
        var castToInterface = Expression.Convert(newResult, typeof(IModuleResult));

        var lambda = Expression.Lambda<CreateSkippedResultDelegate>(castToInterface, contextParam);
        return lambda.Compile();
    }

    private static CreateExceptionResultDelegate CreateExceptionFactory(Type resultType)
    {
        var exceptionParam = Expression.Parameter(typeof(Exception), "exception");
        var contextParam = Expression.Parameter(typeof(ModuleExecutionContext), "executionContext");
        var resultGenericType = typeof(ModuleResult<>).MakeGenericType(resultType);

        // Find the internal constructor: ModuleResult<T>(Exception exception, ModuleExecutionContext context)
        // Note: The constructor takes the base class ModuleExecutionContext, not ModuleExecutionContext<T>
        var constructor = resultGenericType.GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            new[] { typeof(Exception), typeof(ModuleExecutionContext) },
            null);

        if (constructor == null)
        {
            throw new InvalidOperationException(
                $"Could not find internal constructor for ModuleResult<{resultType.Name}>(Exception, ModuleExecutionContext)");
        }

        // Create: new ModuleResult<T>(exception, executionContext)
        var newResult = Expression.New(constructor, exceptionParam, contextParam);

        // Cast to IModuleResult
        var castToInterface = Expression.Convert(newResult, typeof(IModuleResult));

        var lambda = Expression.Lambda<CreateExceptionResultDelegate>(castToInterface, exceptionParam, contextParam);
        return lambda.Compile();
    }
}
