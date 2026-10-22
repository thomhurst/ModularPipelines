using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.TestHelpers;
using Moq;
using PipelineEngineCancellationToken = ModularPipelines.Engine.EngineCancellationToken;

namespace ModularPipelines.UnitTests.Execution;

public class ShutdownLifetimeTests
{
    private sealed class ProbeModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [Test]
    public async Task Lifetime_Callback_Failure_Does_Not_Prevent_Engine_Cancellation()
    {
        var lifetime = new Mock<IHostApplicationLifetime>();
        lifetime.Setup(application => application.StopApplication())
            .Throws(new InvalidOperationException("lifetime callback failed"));
        using var cancellation = new PipelineEngineCancellationToken(
            new ModularPipelines.Engine.PrimaryExceptionContainer(), lifetime.Object);

        await Assert.That(() => cancellation.RequestShutdown()).Throws<InvalidOperationException>();

        await Assert.That(cancellation.Token.IsCancellationRequested).IsTrue();
        await Assert.That(cancellation.NonFailureCancellationToken.IsCancellationRequested).IsTrue();
        await Assert.That(cancellation.ExplicitCancellationToken.IsCancellationRequested).IsFalse();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Shutdown_Signals_Stop_The_Host_Without_Explicit_Cancellation(bool sigterm)
    {
        await using var pipeline = await TestPipelineBuilder.Create().AddModule<ProbeModule>().BuildAsync();
        var cancellation = pipeline.Services.GetRequiredService<PipelineEngineCancellationToken>();
        var lifetime = pipeline.Services.GetRequiredService<IHostApplicationLifetime>();
        cancellation.IsRunActive = () => true;

        var handled = sigterm ? cancellation.HandleSigterm() : cancellation.HandleCancelKeyPress();

        await Assert.That(handled).IsTrue();
        await Assert.That(lifetime.ApplicationStopping.IsCancellationRequested).IsTrue();
        await Assert.That(cancellation.NonFailureCancellationToken.IsCancellationRequested).IsTrue();
        await Assert.That(cancellation.ExplicitCancellationToken.IsCancellationRequested).IsFalse();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Failure_And_Explicit_Cancellation_Do_Not_Stop_The_Host(bool failure)
    {
        await using var pipeline = await TestPipelineBuilder.Create().AddModule<ProbeModule>().BuildAsync();
        var cancellation = pipeline.Services.GetRequiredService<PipelineEngineCancellationToken>();
        var lifetime = pipeline.Services.GetRequiredService<IHostApplicationLifetime>();

        if (failure)
        {
            cancellation.CancelWithException(new InvalidOperationException("module failed"));
        }
        else
        {
            cancellation.CancelWithReason("user canceled");
        }

        await Assert.That(lifetime.ApplicationStopping.IsCancellationRequested).IsFalse();
        await Assert.That(cancellation.NonFailureCancellationToken.IsCancellationRequested).IsEqualTo(!failure);
        await Assert.That(cancellation.ExplicitCancellationToken.IsCancellationRequested).IsEqualTo(!failure);
    }

    [Test]
    public async Task Normal_Completion_Does_Not_Request_Shutdown()
    {
        await using var pipeline = await TestPipelineBuilder.Create().AddModule<ProbeModule>().BuildAsync();

        await pipeline.RunAsync();

        var cancellation = pipeline.Services.GetRequiredService<PipelineEngineCancellationToken>();
        var lifetime = pipeline.Services.GetRequiredService<IHostApplicationLifetime>();
        await Assert.That(lifetime.ApplicationStopping.IsCancellationRequested).IsFalse();
        await Assert.That(cancellation.IsCanceled).IsFalse();
    }
}
