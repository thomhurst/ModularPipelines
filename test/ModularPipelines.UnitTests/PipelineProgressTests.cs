using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using ModularPipelines.TestHelpers;
using Spectre.Console;

namespace ModularPipelines.UnitTests;

[TUnit.Core.NotInParallel]
public class PipelineProgressTests
{
    private static bool _originalInteractive;

    [Before(Class)]
    public static void Setup()
    {
        _originalInteractive = AnsiConsole.Profile.Capabilities.Interactive;
        AnsiConsole.Profile.Capabilities.Interactive = true;
    }

    [After(Class)]
    public static void CleanUp()
    {
        AnsiConsole.Profile.Capabilities.Interactive = _originalInteractive;
    }

    private class Module1 : Module
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            // Reduced delay from 1 second to 50ms for faster test execution
            await Task.Delay(TimeSpan.FromMilliseconds(50), cancellationToken);
            return null;
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class Module2 : Module
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            // Reduced delay from 1 second to 50ms for faster test execution
            await Task.Delay(TimeSpan.FromMilliseconds(50), cancellationToken);
            return null;
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class Module3 : Module
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class Module4 : Module, IModuleSkipLogic
    {
        public Task<SkipDecision> ShouldSkipAsync(IPipelineContext context)
        {
            return SkipDecision.Skip("Testing").AsTask();
        }

        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return null;
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class Module5 : Module
    {
        // Note: ShouldIgnoreFailures no longer exists in Module<T> architecture
        // This module will now fail instead of ignoring the exception
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    // TODO: SubModules removed in v3.0 - these test modules need to be rewritten
    // private class Module6 : Module
    // {
    //     public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    //     {
    //         foreach (var guid in Enumerable.Range(0, 20)
    //                      .Select(x => Guid.NewGuid().ToString("N")))
    //         {
    //             await SubModule(guid, () => Task.CompletedTask);
    //         }
    //
    //         return null;
    //     }
    // }
    //
    // private class Module7 : Module
    // {
    //     public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    //     {
    //         foreach (var guid in Enumerable.Range(0, 20)
    //                      .Select(x => Guid.NewGuid().ToString("N")))
    //         {
    //             await SubModule(guid, async () => await Task.FromResult(guid));
    //         }
    //
    //         return null;
    //     }
    // }

    [Test, TUnit.Core.Retry(5)]
    public async Task Can_Show_Progress()
    {
        await Assert.That(async () =>
                await TestPipelineHostBuilder.Create()
                    .ConfigurePipelineOptions((_, options) =>
                    {
                        options.PrintResults = true;
                        options.ShowProgressInConsole = true;
                    })
                    .AddModule<Module1>()
                    .AddModule<Module2>()
                    .AddModule<Module3>()
                    .AddModule<Module4>()
                    .AddModule<Module5>()
                    // TODO: SubModules removed in v3.0
                    // .AddModule<Module6>()
                    // .AddModule<Module7>()
                    .ExecutePipelineAsync()).
            Throws<ModuleFailedException>();
    }
}