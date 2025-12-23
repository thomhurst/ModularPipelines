using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

[TUnit.Core.NotInParallel(nameof(ConcurrencyOptionsTests))]
public class ConcurrencyOptionsTests : TestBase
{
    public class SimpleModule : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("Done");
        }
    }

    public class SimpleModule2 : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("Done");
        }
    }

    [Test]
    public async Task ConcurrencyOptions_HasCorrectDefaultValues()
    {
        var options = new ConcurrencyOptions();

        // Default MaxParallelism should be ProcessorCount * 4
        var expectedMaxParallelism = Environment.ProcessorCount * 4;
        await Assert.That(options.MaxParallelism).IsEqualTo(expectedMaxParallelism);

        // Default MaxCpuIntensiveModules should be ProcessorCount
        await Assert.That(options.MaxCpuIntensiveModules).IsEqualTo(Environment.ProcessorCount);

        // Default MaxIoIntensiveModules should be null (unlimited)
        await Assert.That(options.MaxIoIntensiveModules).IsNull();
    }

    [Test]
    public async Task Pipeline_RespectsMaxParallelismSetting()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<SimpleModule>()
            .AddModule<SimpleModule2>()
            .ConfigurePipelineOptions((_, options) =>
            {
                options.Concurrency.MaxParallelism = 2;
            })
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Pipeline_RespectsMaxCpuIntensiveModulesSetting()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<SimpleModule>()
            .ConfigurePipelineOptions((_, options) =>
            {
                options.Concurrency.MaxCpuIntensiveModules = 1;
            })
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Pipeline_RespectsMaxIoIntensiveModulesSetting()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<SimpleModule>()
            .ConfigurePipelineOptions((_, options) =>
            {
                options.Concurrency.MaxIoIntensiveModules = 10;
            })
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task PipelineOptions_HasConcurrencyProperty()
    {
        var options = new PipelineOptions();

        await Assert.That(options.Concurrency).IsNotNull();
        await Assert.That(options.Concurrency).IsTypeOf<ConcurrencyOptions>();
    }
}
