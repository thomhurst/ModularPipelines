using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

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
        IPipelineHookContext context);

    private static readonly ConcurrentDictionary<Type, GetResultDelegate> GetResultCache = new();

    /// <summary>
    /// Cache for generic MethodInfo instances created via MakeGenericMethod.
    /// Key is the result type, value is the specialized MethodInfo.
    /// </summary>
    private static readonly ConcurrentDictionary<Type, MethodInfo> GetResultAsyncMethodCache = new();
    private static readonly ConcurrentDictionary<Type, MethodInfo> GetResultAndCastAsyncMethodCache = new();

    /// <summary>
    /// Base method definitions, cached once on first use.
    /// </summary>
    private static readonly MethodInfo GetResultAsyncMethodDefinition =
        typeof(IModuleResultRepository).GetMethod(nameof(IModuleResultRepository.GetResultAsync))!;

    private static readonly MethodInfo GetResultAndCastAsyncMethodDefinition =
        typeof(ResultRepositoryDelegateFactory).GetMethod(nameof(GetResultAndCastAsync), BindingFlags.NonPublic | BindingFlags.Static)!;

    /// <summary>
    /// Gets a cached delegate for calling GetResultAsync with the specified result type.
    /// </summary>
    public static GetResultDelegate GetResultDelegateFor(Type resultType)
    {
        return GetResultCache.GetOrAdd(resultType, CreateGetResultDelegate);
    }

    private static GetResultDelegate CreateGetResultDelegate(Type resultType)
    {
        var repositoryParam = Expression.Parameter(typeof(IModuleResultRepository), "repository");
        var moduleParam = Expression.Parameter(typeof(IModule), "module");
        var contextParam = Expression.Parameter(typeof(IPipelineHookContext), "context");

        // Get types
        var moduleType = typeof(Module<>).MakeGenericType(resultType);

        // Cast module to Module<T>
        var castModule = Expression.Convert(moduleParam, moduleType);

        // Get the GetResultAsync<T> method (cached)
        var method = GetResultAsyncMethodCache.GetOrAdd(
            resultType,
            static type => GetResultAsyncMethodDefinition.MakeGenericMethod(type));

        // Call: repository.GetResultAsync<T>((Module<T>)module, context)
        var callMethod = Expression.Call(repositoryParam, method, castModule, contextParam);

        // We need an async helper since expression trees can't represent async (cached)
        var helperMethod = GetResultAndCastAsyncMethodCache.GetOrAdd(
            resultType,
            static type => GetResultAndCastAsyncMethodDefinition.MakeGenericMethod(type));

        var callHelper = Expression.Call(helperMethod, repositoryParam, castModule, contextParam);

        var lambda = Expression.Lambda<GetResultDelegate>(callHelper, repositoryParam, moduleParam, contextParam);
        return lambda.Compile();
    }

    private static async Task<IModuleResult?> GetResultAndCastAsync<T>(
        IModuleResultRepository repository,
        Module<T> module,
        IPipelineHookContext context)
    {
        var result = await repository.GetResultAsync(module, context).ConfigureAwait(false);
        return result;
    }
}
