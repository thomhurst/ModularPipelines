using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
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
        Func<CancellationToken, Task>? prepareExecutionAsync,
        Func<IModuleResult, CancellationToken, Task>? finalizeExecutionAsync,
        bool completeModule,
        CancellationToken cancellationToken);

    private static readonly ConcurrentDictionary<Type, ExecuteModuleDelegate> ExecutorCache = new();

    private static readonly ConcurrentDictionary<Type, MethodInfo> ExecuteAndCastAsyncMethodCache = new();

    /// <summary>
    /// Base method definitions, cached once on first use.
    /// </summary>
    private static readonly MethodInfo ExecuteAndCastAsyncMethodDefinition =
        typeof(ModuleExecutionDelegateFactory).GetMethod(nameof(ExecuteAndCastAsync), BindingFlags.NonPublic | BindingFlags.Static)!;

    /// <summary>
    /// Gets a cached delegate for executing a module with the specified result type.
    /// </summary>
    /// <param name="resultType">The result type of the module (T in Module&lt;T&gt;).</param>
    /// <returns>A delegate that executes the module and returns its result.</returns>
    public static ExecuteModuleDelegate GetExecutor(Type resultType)
    {
        return ExecutorCache.GetOrAdd(resultType, CreateExecutor);
    }

    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Generated runtime metadata handles statically known modules; this factory is the documented fallback for dynamic modules.")]
    private static ExecuteModuleDelegate CreateExecutor(Type resultType)
    {
        // Parameters for the delegate
        var pipelineParam = Expression.Parameter(typeof(IModuleExecutionPipeline), "pipeline");
        var moduleParam = Expression.Parameter(typeof(IModule), "module");
        var contextParam = Expression.Parameter(typeof(ModuleExecutionContext), "executionContext");
        var moduleContextParam = Expression.Parameter(typeof(IModuleContext), "moduleContext");
        var prepareExecutionParam = Expression.Parameter(
            typeof(Func<CancellationToken, Task>),
            "prepareExecutionAsync");
        var finalizeExecutionParam = Expression.Parameter(
            typeof(Func<IModuleResult, CancellationToken, Task>),
            "finalizeExecutionAsync");
        var completeModuleParam = Expression.Parameter(typeof(bool), "completeModule");
        var cancellationTokenParam = Expression.Parameter(typeof(CancellationToken), "cancellationToken");

        // Get the generic types
        var moduleType = typeof(Module<>).MakeGenericType(resultType);
        var executionContextType = typeof(ModuleExecutionContext<>).MakeGenericType(resultType);

        // Cast module to Module<T>
        var castModule = Expression.Convert(moduleParam, moduleType);

        // Cast executionContext to ModuleExecutionContext<T>
        var castContext = Expression.Convert(contextParam, executionContextType);

        // We need to create an async wrapper that awaits the task and casts the result to IModuleResult
        // Since Expression trees can't directly represent async/await, we'll use a helper method (cached)
        var helperMethod = ExecuteAndCastAsyncMethodCache.GetOrAdd(
            resultType,
            MakeExecuteAndCastAsyncMethod);

        var callHelper = Expression.Call(
            helperMethod,
            pipelineParam,
            castModule,
            castContext,
            moduleContextParam,
            prepareExecutionParam,
            finalizeExecutionParam,
            completeModuleParam,
            cancellationTokenParam);

        // Create and compile the lambda
        var lambda = Expression.Lambda<ExecuteModuleDelegate>(
            callHelper,
            pipelineParam,
            moduleParam,
            contextParam,
            moduleContextParam,
            prepareExecutionParam,
            finalizeExecutionParam,
            completeModuleParam,
            cancellationTokenParam);

        return lambda.Compile();
    }

    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Generated runtime metadata handles statically known modules; this factory is the documented fallback for dynamic modules.")]
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2060",
        Justification = "Generated runtime metadata handles statically known modules; this factory is the documented fallback for dynamic modules.")]
    private static MethodInfo MakeExecuteAndCastAsyncMethod(Type resultType)
    {
        return ExecuteAndCastAsyncMethodDefinition.MakeGenericMethod(resultType);
    }

    private static async Task<IModuleResult> ExecuteAndCastAsync<T>(
        IModuleExecutionPipeline pipeline,
        Module<T> module,
        ModuleExecutionContext<T> executionContext,
        IModuleContext moduleContext,
        Func<CancellationToken, Task>? prepareExecutionAsync,
        Func<IModuleResult, CancellationToken, Task>? finalizeExecutionAsync,
        bool completeModule,
        CancellationToken cancellationToken)
    {
        var result = await pipeline.ExecuteAsync(
                module,
                executionContext,
                moduleContext,
                cancellationToken,
                prepareExecutionAsync,
                finalizeExecutionAsync,
                completeModule)
            .ConfigureAwait(false);
        return result;
    }
}
