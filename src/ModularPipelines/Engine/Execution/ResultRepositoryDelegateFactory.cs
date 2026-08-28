using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Reporting;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Factory for creating cached delegates to call IModuleResultRepository methods without reflection.
/// </summary>
internal static class ResultRepositoryDelegateFactory
{
    /// <summary>
    /// Delegate for calling GetResultAsync on a repository.
    /// </summary>
    internal delegate Task<IModuleResult?> GetResultDelegate(
        IModuleResultRepository repository,
        IModule module,
        IPipelineContext context);

    private static readonly ConcurrentDictionary<Type, GetResultDelegate> GetResultCache = new();

    private static readonly ConcurrentDictionary<Type, MethodInfo> GetResultAndCastAsyncMethodCache = new();

    /// <summary>
    /// Base method definitions, cached once on first use.
    /// </summary>
    private static readonly MethodInfo GetResultAndCastAsyncMethodDefinition =
        typeof(ResultRepositoryDelegateFactory).GetMethod(nameof(GetResultAndCastAsync), BindingFlags.NonPublic | BindingFlags.Static)!;

    /// <summary>
    /// Gets a cached delegate for calling GetResultAsync with the specified result type.
    /// </summary>
    public static GetResultDelegate GetResultDelegateFor(Type resultType)
    {
        return GetResultCache.GetOrAdd(resultType, CreateGetResultDelegate);
    }

    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Runtime result repositories support dynamic result types only outside Native AOT.")]
    private static GetResultDelegate CreateGetResultDelegate(Type resultType)
    {
        var repositoryParam = Expression.Parameter(typeof(IModuleResultRepository), "repository");
        var moduleParam = Expression.Parameter(typeof(IModule), "module");
        var contextParam = Expression.Parameter(typeof(IPipelineContext), "context");

        // Get types
        var moduleType = typeof(Module<>).MakeGenericType(resultType);

        // Cast module to Module<T>
        var castModule = Expression.Convert(moduleParam, moduleType);

        // We need an async helper since expression trees can't represent async (cached)
        var helperMethod = GetResultAndCastAsyncMethodCache.GetOrAdd(
            resultType,
            MakeGetResultAndCastAsyncMethod);

        var callHelper = Expression.Call(helperMethod, repositoryParam, castModule, contextParam);

        var lambda = Expression.Lambda<GetResultDelegate>(callHelper, repositoryParam, moduleParam, contextParam);
        return lambda.Compile();
    }

    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Runtime result repositories support dynamic result types only outside Native AOT.")]
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2060",
        Justification = "Runtime result repositories support dynamic result types only outside Native AOT.")]
    private static MethodInfo MakeGetResultAndCastAsyncMethod(Type resultType)
    {
        return GetResultAndCastAsyncMethodDefinition.MakeGenericMethod(resultType);
    }

    private static async Task<IModuleResult?> GetResultAndCastAsync<T>(
        IModuleResultRepository repository,
        Module<T> module,
        IPipelineContext context)
    {
        var result = await repository.GetResultAsync(module, context).ConfigureAwait(false);
        return result;
    }
}
