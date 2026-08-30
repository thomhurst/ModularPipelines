using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Console;

public class OutputCoordinatorDeferredFlushTests : TestBase
{
    private class Module1 : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            context.Logger.LogInformation("Module1 output");
            await Task.Delay(50, cancellationToken);
            return true;
        }
    }

    private class Module2 : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            context.Logger.LogInformation("Module2 output");
            await Task.Yield();
            return true;
        }
    }

    [Test]
    public async Task Pipeline_Completes_When_Progress_Disabled()
    {
        var host = await TestPipelineBuilder.Create()
            .ConfigurePipelineOptions(options => options with
            {
                Console = options.Console with { ShowProgress = false },
            })
            .AddModule<Module1>()
            .BuildAsync();

        await using (host)
        {
            var result = await host.RunAsync();
            await Assert.That(result.Status).IsEqualTo(ModularPipelines.ModuleStatus.Succeeded);
        }
    }

    [Test]
    public async Task Pipeline_With_Multiple_Modules_Completes_Successfully()
    {
        var host = await TestPipelineBuilder.Create()
            .ConfigurePipelineOptions(options => options with
            {
                Console = options.Console with { ShowProgress = false },
            })
            .AddModule<Module1>()
            .AddModule<Module2>()
            .BuildAsync();

        await using (host)
        {
            var result = await host.RunAsync();
            await Assert.That(result.Status).IsEqualTo(ModularPipelines.ModuleStatus.Succeeded);
        }
    }
}
