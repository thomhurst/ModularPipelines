using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ModularPipelines.Context;
using ModularPipelines.Generated;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Caching;

internal static class ModuleCacheResultAccessor
{
    public static Task<IModuleResult?> GetResultAsync(
        IModuleCacheResultRepository repository,
        IModule module,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        if (GeneratedModuleMetadata.TryGetRuntime(module.GetType(), out var runtime))
        {
            return runtime.GetCachedResultAsync(
                repository,
                module,
                pipelineContext,
                cancellationToken);
        }

        return GetDynamicResultAsync(repository, module, pipelineContext, cancellationToken);
    }

    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Generated runtime metadata handles statically known modules; reflection supports dynamic modules.")]
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2060",
        Justification = "Generated runtime metadata handles statically known modules; reflection supports dynamic modules.")]
    private static Task<IModuleResult?> GetDynamicResultAsync(
        IModuleCacheResultRepository repository,
        IModule module,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        var method = typeof(ModuleCacheResultAccessor)
            .GetMethod(nameof(GetResultAsyncCore), BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(module.ResultType);

        return (Task<IModuleResult?>) method.Invoke(
            null,
            [repository, module, pipelineContext, cancellationToken])!;
    }

    private static async Task<IModuleResult?> GetResultAsyncCore<T>(
        IModuleCacheResultRepository repository,
        Module<T> module,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        return await repository.GetResultAsync(module, pipelineContext, cancellationToken)
            .ConfigureAwait(false);
    }
}
