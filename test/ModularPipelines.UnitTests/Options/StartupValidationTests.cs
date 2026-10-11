using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Options;

public class StartupValidationTests
{
    [Test]
    public async Task ValidateOnStart_Failures_Fail_The_Build()
    {
        var builder = TestPipelineBuilder.Create();
        builder.Services.AddOptions<SampleOptions>()
            .Validate(_ => false, "first failure")
            .ValidateOnStart();

        var exception = await Assert.ThrowsAsync<OptionsValidationException>(() => builder.BuildAsync());

        await Assert.That(exception!.Failures).Contains("first failure");
    }

    [Test]
    public async Task Sync_Only_Startup_Validator_Replacement_Takes_Precedence()
    {
        var validator = new SyncOnlyStartupValidator();
        var asyncValidator = new RecordingAsyncStartupValidator();
        var builder = TestPipelineBuilder.Create();
        builder.AddModule<NoOpModule>();
        builder.Services.AddOptions<SampleOptions>().ValidateOnStart();
        builder.Services.AddSingleton<IAsyncStartupValidator>(asyncValidator);
#pragma warning disable SYSLIB0066 // Covers consumers that still replace the obsolete validator.
        builder.Services.Replace(ServiceDescriptor.Singleton<IStartupValidator>(validator));
#pragma warning restore SYSLIB0066

        await using var pipeline = await builder.BuildAsync();

        using (Assert.Multiple())
        {
            await Assert.That(validator.Calls).IsEqualTo(1);
            await Assert.That(asyncValidator.Calls).IsEqualTo(0);
        }
    }

    [Test]
    public async Task Async_Validator_Failures_Are_Aggregated()
    {
        var builder = TestPipelineBuilder.Create();
        builder.Services.AddSingleton<IAsyncStartupValidator>(new FailingAsyncStartupValidator("one"));
        builder.Services.AddSingleton<IAsyncStartupValidator>(new FailingAsyncStartupValidator("two"));

        var exception = await Assert.ThrowsAsync<AggregateException>(() => builder.BuildAsync());

        await Assert.That(exception!.InnerExceptions.Count).IsEqualTo(2);
    }

    private sealed class SampleOptions;

    private sealed class NoOpModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(true);
    }

#pragma warning disable SYSLIB0066 // Covers consumers that still replace the obsolete validator.
    private sealed class SyncOnlyStartupValidator : IStartupValidator
#pragma warning restore SYSLIB0066
    {
        public int Calls { get; private set; }

        public void Validate() => Calls++;
    }

    private sealed class RecordingAsyncStartupValidator : IAsyncStartupValidator
    {
        public int Calls { get; private set; }

        public Task ValidateAsync(CancellationToken cancellationToken)
        {
            Calls++;
            return Task.CompletedTask;
        }
    }

    private sealed class FailingAsyncStartupValidator(string name) : IAsyncStartupValidator
    {
        public Task ValidateAsync(CancellationToken cancellationToken) =>
            throw new OptionsValidationException(name, typeof(SampleOptions), [name]);
    }
}
