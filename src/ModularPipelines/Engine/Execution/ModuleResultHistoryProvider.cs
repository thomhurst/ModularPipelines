using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Reporting;

namespace ModularPipelines.Engine.Execution;

internal interface IModuleResultHistoryProvider
{
    Task<IModuleResult?> TryGetAsync(IModule module, IPipelineContext pipelineContext);
}

internal sealed class ModuleResultHistoryProvider(
    IModuleResultRepository resultRepository,
    ILogger<ModuleResultHistoryProvider> logger) : IModuleResultHistoryProvider
{
    public async Task<IModuleResult?> TryGetAsync(IModule module, IPipelineContext pipelineContext)
    {
        if (!resultRepository.IsEnabled)
        {
            return null;
        }

        try
        {
            var getResult = ResultRepositoryDelegateFactory.GetResultDelegateFor(module.ResultType);
            return await getResult(resultRepository, module, pipelineContext).ConfigureAwait(false);
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
