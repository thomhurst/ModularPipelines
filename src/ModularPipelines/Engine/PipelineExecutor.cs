using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Extensions;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;

namespace ModularPipelines.Engine;

internal class PipelineExecutor : IPipelineExecutor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly EngineCancellationToken _engineCancellationToken;
    private readonly IDependencyDetector _dependencyDetector;
    private readonly IModuleLoggerContainer _moduleLoggerContainer;

    public PipelineExecutor(
        IServiceProvider serviceProvider,
        EngineCancellationToken engineCancellationToken,
        IDependencyDetector dependencyDetector,
        IModuleLoggerContainer moduleLoggerContainer)
    {
        _serviceProvider = serviceProvider;
        _engineCancellationToken = engineCancellationToken;
        _dependencyDetector = dependencyDetector;
        _moduleLoggerContainer = moduleLoggerContainer;
    }

    public async Task<IReadOnlyList<ModuleBase>> ExecuteAsync()
    {
        _dependencyDetector.Check();

        await using var serviceScope = await _serviceProvider.CreateInitializedAsyncScope();
        
        await Get<IPipelineSetupExecutor>().OnStartAsync();

        await Get<IRequirementChecker>().CheckRequirementsAsync();

        var organizedModules = await Get<IModuleRetriever>().GetOrganizedModules();

        Get<IPipelineConsolePrinter>().PrintProgress(organizedModules, _engineCancellationToken.Token);

        var runnableModules = organizedModules.RunnableModules.Select(x => x.Module).ToList();

        try
        {
            await Get<IModuleExecutor>().ExecuteAsync(runnableModules);
        }
        catch
        {
            // Give time for the console to update modules to Failed
            await Task.Delay(100);
            _engineCancellationToken.Cancel();
            throw;
        }
        finally
        {
            await WaitForAlwaysRunModules(runnableModules);

            await Dispose(runnableModules);

            await Get<IPipelineSetupExecutor>().OnEndAsync(organizedModules.AllModules);

            await Task.Delay(200);

            _moduleLoggerContainer.PrintAllLoggers();
        }

        return organizedModules.AllModules;

        T Get<T>() where T : notnull => serviceScope.ServiceProvider.GetRequiredService<T>();
    }

    private async Task WaitForAlwaysRunModules(IEnumerable<ModuleBase> runnableModules)
    {
        try
        {
            await Task.WhenAll(runnableModules.Where(m => m.ModuleRunType == ModuleRunType.AlwaysRun).Select(m => m.ResultTaskInternal));
        }
        catch
        {
            // Ignored
        }
    }

    private async Task Dispose(IEnumerable<ModuleBase> modulesToProcess)
    {
        foreach (var module in modulesToProcess)
        {
            await Dispose(module);
        }
    }

    private static async Task Dispose(ModuleBase module)
    {
        if (module is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync();
        }

        if (module is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
