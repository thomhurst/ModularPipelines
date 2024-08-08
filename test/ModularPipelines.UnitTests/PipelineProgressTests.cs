using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
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
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            return await NothingAsync();
        }
    }

    [DependsOn<Module1>]
    private class Module2 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            return await NothingAsync();
        }
    }

    [DependsOn<Module1>]
    private class Module3 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    [DependsOn<Module1>]
    private class Module4 : Module
    {
        protected internal override Task<SkipDecision> ShouldSkip(IPipelineContext context)
        {
            return SkipDecision.Skip("Testing").AsTask();
        }

        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await NothingAsync();
        }
    }

    [DependsOn<Module1>]
    private class Module5 : Module
    {
        protected internal override Task<bool> ShouldIgnoreFailures(IPipelineContext context, Exception exception)
        {
            return true.AsTask();
        }

        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    private class Module6 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            foreach (var guid in Enumerable.Range(0, 20)
                         .Select(x => Guid.NewGuid().ToString("N")))
            {
                await SubModule(guid, () => Task.CompletedTask);
            }

            return await NothingAsync();
        }
    }

    private class Module7 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            foreach (var guid in Enumerable.Range(0, 20)
                         .Select(x => Guid.NewGuid().ToString("N")))
            {
                await SubModule(guid, async () => await Task.FromResult(guid));
            }

            return await NothingAsync();
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
            Throws.Exception().OfType<ModuleFailedException>();
    }
}