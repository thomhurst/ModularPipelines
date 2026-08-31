using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Spectre.Console;

namespace ModularPipelines.UnitTests.Engine;

[TUnit.Core.NotInParallel]
public class PipelineProgressTests
{
    private class Module1 : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // Reduced delay from 1 second to 50ms for faster test execution
            await Task.Delay(TimeSpan.FromMilliseconds(50), cancellationToken);
            return true;
        }
    }

    [ModularPipelines.DependsOn<Module1>]
    private class Module2 : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // Reduced delay from 1 second to 50ms for faster test execution
            await Task.Delay(TimeSpan.FromMilliseconds(50), cancellationToken);
            return true;
        }
    }

    [ModularPipelines.DependsOn<Module1>]
    private class Module3 : ThrowingTestModule<bool>
    {
    }

    [ModularPipelines.DependsOn<Module1>]
    private class Module4 : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => SkipDecision.Skip("Testing"));

        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return true;
        }
    }

    [ModularPipelines.DependsOn<Module1>]
    private class Module5 : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithIgnoreFailures();

        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    private class Module6 : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    [Test]
    public async Task Can_Show_Progress()
    {
        using var output = new StringWriter();
        var console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Ansi = AnsiSupport.No,
            ColorSystem = ColorSystemSupport.NoColors,
            Interactive = InteractionSupport.Yes,
            Out = new AnsiConsoleOutput(output),
        });

        await Assert.That(async () =>
                await TestPipelineBuilder.Create()
                    .ConfigureServices(services => services.AddSingleton<IAnsiConsole>(console))
                    .ConfigureOptions(options => options with
                    {
                        Console = options.Console with
                        {
                            PrintResults = false,
                            ShowProgress = true,
                        },
                    })
                    .AddModule<Module1>()
                    .AddModule<Module2>()
                    .AddModule<Module3>()
                    .AddModule<Module4>()
                    .AddModule<Module5>()
                    .AddModule<Module6>()
                    .AddModule<Module7>()
                    .RunAsync()).
            Throws<ModuleFailedException>();
    }
}
