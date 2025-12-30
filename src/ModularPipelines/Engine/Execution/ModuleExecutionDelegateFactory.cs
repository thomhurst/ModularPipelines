using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Factory for creating cached delegates to execute modules without runtime reflection.
/// </summary>
/// <remarks>
/// This class replaces the reflection-heavy pattern of using MakeGenericMethod and GetProperty("Result")
/// with compiled expression trees that are cached per result type.
/// </remarks>
internal static class ModuleExecutionDelegateFactory
{
    /// <summary>
    /// Delegate signature for executing a module and returning its result.
    /// </summary>
    internal delegate Task<IModuleResult> ExecuteModuleDelegate(
        IModuleExecutionPipeline pipeline,
        IModule module,
        ModuleExecutionContext executionContext,
        IModuleContext moduleContext,
        CancellationToken cancellationToken);

    private static readonly ConcurrentDictionary<Type, ExecuteModuleDelegate> ExecutorCache = new();

    /// <summary>
    /// Gets a cached delegate for executing a module with the specified result type.
    /// </summary>
    /// <param name="resultType">The result type of the module (T in Module&lt;T&gt;).</param>
    /// <returns>A delegate that executes the module and returns its result.</returns>
    public static ExecuteModuleDelegate GetExecutor(Type resultType)
    {
        return ExecutorCache.GetOrAdd(resultType, CreateExecutor);
    }

    private static ExecuteModuleDelegate CreateExecutor(Type resultType)
    {
        // Parameters for the delegate
        var pipelineParam = Expression.Parameter(typeof(IModuleExecutionPipeline), "pipeline");
        var moduleParam = Expression.Parameter(typeof(IModule), "module");
        var contextParam = Expression.Parameter(typeof(ModuleExecutionContext), "executionContext");
        var moduleContextParam = Expression.Parameter(typeof(IModuleContext), "moduleContext");
        var cancellationTokenParam = Expression.Parameter(typeof(CancellationToken), "cancellationToken");

        // Get the generic types
        var moduleType = typeof(Module<>).MakeGenericType(resultType);
        var executionContextType = typeof(ModuleExecutionContext<>).MakeGenericType(resultType);
        var moduleResultType = typeof(ModuleResult<>).MakeGenericType(resultType);
        var taskType = typeof(Task<>).MakeGenericType(moduleResultType);

        // Cast module to Module<T>
        var castModule = Expression.Convert(moduleParam, moduleType);

        // Cast executionContext to ModuleExecutionContext<T>
        var castContext = Expression.Convert(contextParam, executionContextType);

        // Get the ExecuteAsync method
        var executeMethod = typeof(IModuleExecutionPipeline)
            .GetMethod(nameof(IModuleExecutionPipeline.ExecuteAsync))!
            .MakeGenericMethod(resultType);

        // Call pipeline.ExecuteAsync<T>(module, executionContext, moduleContext, cancellationToken)
        var callExecute = Expression.Call(
            pipelineParam,
            executeMethod,
            castModule,
            castContext,
            moduleContextParam,
            cancellationTokenParam);

        // We need to create an async wrapper that awaits the task and casts the result to IModuleResult
        // Since Expression trees can't directly represent async/await, we'll use a helper method
        var helperMethod = typeof(ModuleExecutionDelegateFactory)
            .GetMethod(nameof(ExecuteAndCastAsync), BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(resultType);

        var callHelper = Expression.Call(
            helperMethod,
            pipelineParam,
            castModule,
            castContext,
            moduleContextParam,
            cancellationTokenParam);

        // Create and compile the lambda
        var lambda = Expression.Lambda<ExecuteModuleDelegate>(
            callHelper,
            pipelineParam,
            moduleParam,
            contextParam,
            moduleContextParam,
            cancellationTokenParam);

        return lambda.Compile();
    }

    private static async Task<IModuleResult> ExecuteAndCastAsync<T>(
        IModuleExecutionPipeline pipeline,
        Module<T> module,
        ModuleExecutionContext<T> executionContext,
        IModuleContext moduleContext,
        CancellationToken cancellationToken)
    {
        var result = await pipeline.ExecuteAsync(module, executionContext, moduleContext, cancellationToken)
            .ConfigureAwait(false);
        return result;
    }
}
