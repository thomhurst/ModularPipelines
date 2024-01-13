using System.Reflection;
using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal class WaitHandler<T> : BaseHandler<T>, IWaitHandler
{
    public WaitHandler(Module<T> module) : base(module)
    {
    }

    public async Task<WaitResult> WaitForModuleDependencies()
    {
        try
        {
            await WaitForDependencies();
        }
        catch (DependencyFailedException) when (Context.EngineCancellationToken.IsCancellationRequested)
        {
            ModuleResultTaskCompletionSource.TrySetCanceled();

            return WaitResult.Abort;
        }

        return WaitResult.Continue;
    }

    private async Task WaitForDependencies()
    {
        if (!Module.DependentModules.Any())
        {
            Context.Logger.LogDebug("No dependent modules - Nothing to wait for");
            return;
        }

        var notInParallel = Module.GetType().GetCustomAttribute<NotInParallelAttribute>() != null;

        var dependenciesProcessor = Module.DependentModules
            .ToAsyncProcessorBuilder()
            .ForEachAsync(async dependsOnAttribute =>
            {
                var module = Context.GetModule(dependsOnAttribute.Type);

                if (dependsOnAttribute.IgnoreIfNotRegistered && module is null)
                {
                    Context.Logger.LogDebug("{Module} was not registered so not waiting",
                        dependsOnAttribute.Type.Name);

                    return;
                }

                if (module is null)
                {
                    throw new ModuleNotRegisteredException(
                        $"The module {dependsOnAttribute.Type.Name} has not been registered", null);
                }

                Context.Logger.LogDebug("Waiting for {Module}", dependsOnAttribute.Type.Name);

                try
                {
                    await module.ExecutionTask;
                }
                catch (Exception e) when (Module.ModuleRunType == ModuleRunType.AlwaysRun)
                {
                    Context.Logger.LogError(e, "Ignoring Exception due to 'AlwaysRun' set");
                }
                catch (DependencyFailedException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new DependencyFailedException(e, module);
                }
            });

        if (notInParallel)
        {
            await dependenciesProcessor.ProcessOneAtATime();
        }
        else
        {
            await dependenciesProcessor.ProcessInParallel();
        }
    }
}