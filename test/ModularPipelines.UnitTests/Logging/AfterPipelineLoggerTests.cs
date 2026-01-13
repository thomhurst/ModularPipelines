using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Logging;

public class AfterPipelineLoggerTests
{
    private class AfterPipelineLoggingModule : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            context.LogOnPipelineEnd("Blah!");
            await Task.CompletedTask;
            return true;
        }
    }

    private class AfterPipelineLoggingWithExceptionModule : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            context.LogOnPipelineEnd("Blah!");
            await Task.CompletedTask;
            throw new Exception();
        }
    }

    [Test]
    public async Task LogsAfterPipeline()
    {
        var stringBuilder = new StringBuilder();
        var host = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) =>
            {
                collection.AddSingleton(stringBuilder);
                collection.AddSingleton(typeof(ILogger<>), typeof(StringLogger<>));
            })
            .AddModule<AfterPipelineLoggingModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        await host.DisposeAsync();

        await Assert.That(stringBuilder.ToString().Trim()).EndsWith("Blah!");
    }

    [Test]
    public async Task LogsAfterPipelineWithException()
    {
        var stringBuilder = new StringBuilder();
        var host = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) =>
            {
                collection.AddSingleton(stringBuilder);
                collection.AddSingleton(typeof(ILogger<>), typeof(StringLogger<>));
            })
            .AddModule<AfterPipelineLoggingWithExceptionModule>()
            .BuildHostAsync();

        try
        {
            await host.ExecutePipelineAsync();
        }
        catch
        {
            // Ignored
        }

        await host.DisposeAsync();

        await Assert.That(stringBuilder.ToString().Trim()).EndsWith("Blah!");
    }
}