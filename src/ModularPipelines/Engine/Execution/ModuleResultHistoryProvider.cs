using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Reporting;

namespace ModularPipelines.Engine.Execution;

internal interface IModuleResultHistoryProvider
{
    Task<IModuleResult?> TryGetAsync(IModule module, IPipelineContext pipelineContext, CancellationToken cancellationToken = default);
}

internal sealed class ModuleResultHistoryProvider(
    IModuleResultRepository resultRepository,
    ILogger<ModuleResultHistoryProvider> logger) : IModuleResultHistoryProvider
{
    public async Task<IModuleResult?> TryGetAsync(IModule module, IPipelineContext pipelineContext, CancellationToken cancellationToken = default)
    {
        if (!resultRepository.IsEnabled)
        {
            return null;
        }

        try
        {
            var getResult = ResultRepositoryDelegateFactory.GetResultDelegateFor(module.ResultType);
            var result = await getResult(resultRepository, module, pipelineContext, cancellationToken).ConfigureAwait(false);
            return result is ModuleResult moduleResult
                ? moduleResult with { ModuleId = ModuleId.FromType(module.GetType()) }
                : result;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception) when (exception is not (OutOfMemoryException or StackOverflowException))
        {
            logger.LogWarning(
                exception,
                "Failed to get historical result for module {ModuleName}",
                module.GetType().Name);
            return null;
        }
    }
}
