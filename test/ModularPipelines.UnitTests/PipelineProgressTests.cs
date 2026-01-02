using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
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

    private class Module1 : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // Reduced delay from 1 second to 50ms for faster test execution
            await Task.Delay(TimeSpan.FromMilliseconds(50), cancellationToken);
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class Module2 : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // Reduced delay from 1 second to 50ms for faster test execution
            await Task.Delay(TimeSpan.FromMilliseconds(50), cancellationToken);
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class Module3 : ThrowingTestModule<bool>
    {
    }

    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class Module4 : SimpleTestModule<bool>, ISkippable
    {
        protected override bool Result => true;

        public Task<SkipDecision> ShouldSkip(IPipelineContext context)
        {
            return SkipDecision.Skip("Testing").AsTask();
        }
    }

    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class Module5 : ThrowingTestModule<bool>, IIgnoreFailures
    {
        public Task<bool> ShouldIgnoreFailures(IPipelineContext context, Exception exception)
        {
            return true.AsTask();
        }
    }

    private class Module6 : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // SubModule functionality now needs to be handled differently in the new architecture
            // For now, just execute some work
            for (var i = 0; i < 20; i++)
            {
                await Task.Yield();
            }
            return true;
        }
    }

    private class Module7 : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // SubModule functionality now needs to be handled differently in the new architecture
            // For now, just execute some work
            for (var i = 0; i < 20; i++)
            {
                await Task.Yield();
            }
            return true;
        }
    }

    [Test, Retry(5)]
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
                    .AddModule<Module6>()
                    .AddModule<Module7>()
                    .ExecutePipelineAsync()).
            Throws<ModuleFailedException>();
    }
}
