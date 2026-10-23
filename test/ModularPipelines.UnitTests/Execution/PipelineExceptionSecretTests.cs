using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Execution;

public class PipelineExceptionSecretTests
{
    private const string Secret = "pipeline-exception-secret";

    [Test]
    [Arguments(FailureMode.FailFast)]
    [Arguments(FailureMode.ContinueOnFailure)]
    public async Task ModuleFailure_IsMaskedAfterDisposal(FailureMode failureMode)
    {
        var builder = TestPipelineBuilder.Create()
            .ConfigureOptions(options => options with
            {
                FailureMode = failureMode,
                ThrowOnPipelineFailure = true,
            })
            .AddModule<SecretFailureModule>();

        Exception failure;
        await using (var pipeline = await builder.BuildAsync())
        {
            failure = (await Assert.ThrowsAsync<Exception>(() => pipeline.RunAsync()))!;
        }

        await AssertMasked(failure);
        await Assert.That(failure.ToString()).Contains("**********");
    }

    [Test]
    public async Task AggregateFailure_MasksEveryBranchAndDiagnostic()
    {
        var original = new AggregateException("aggregate failure",
            new InvalidOperationException("first failure"),
            new Exception($"second failure: {Secret}", new Exception(Secret)));
        original.Data[Secret] = Secret;
        original.HelpLink = $"https://example.test/{Secret}";
        original.Source = Secret;

        var failure = await RunWithFailure(original, registerSecret: true);

        await AssertMasked(failure);
        await Assert.That(failure).IsAssignableTo<AggregateException>();
        await Assert.That(((AggregateException) failure).InnerExceptions.Count).IsEqualTo(2);
        await Assert.That(failure.Message).IsEqualTo("aggregate failure (first failure) (second failure: **********)");
        await Assert.That(original.InnerExceptions[1].Message).Contains(Secret);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task FailureWithoutSensitiveText_PreservesOriginalException(bool registerSecret)
    {
        var original = new InvalidOperationException("ordinary failure");

        var failure = await RunWithFailure(original, registerSecret);

        await Assert.That(failure).IsSameReferenceAs(original);
    }

    [Test]
    public async Task CustomFormatting_DoesNotHideSensitiveInnerExceptionFromMasking()
    {
        var failure = await RunWithFailure(new HiddenInnerException(new Exception(Secret)), registerSecret: true);

        await AssertMasked(failure);
        await Assert.That(failure.InnerException!.Message).IsEqualTo("**********");
    }

    [Test]
    public async Task SecretOnlyInData_IsMasked()
    {
        var original = new Exception("ordinary failure");
        original.Data["token"] = Secret;

        var failure = await RunWithFailure(original, registerSecret: true);

        await AssertMasked(failure);
        await Assert.That(failure.Data["token"]).IsEqualTo("**********");
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task SummaryFailure_InspectsResultExceptions(bool containsSecret, bool aggregate)
    {
        var resultException = new HiddenInnerException(
            new Exception(containsSecret ? Secret : "ordinary failure"));
        var result = Mock.Of<IModuleResult>(result => result.Status == ModuleStatus.Failed
            && result.Name == nameof(SecretFailureModule)
            && result.ExceptionOrDefault == resultException);
        var now = DateTimeOffset.UtcNow;
        var summary = new PipelineSummary([], [result], TimeSpan.Zero, now, now);
        var summaryFailure = new PipelineFailedException(summary, [nameof(SecretFailureModule)]);
        Exception original = aggregate ? new AggregateException(summaryFailure) : summaryFailure;

        var failure = await RunWithFailure(original, registerSecret: true);

        await AssertMasked(failure);
        if (containsSecret)
        {
            await Assert.That(failure).IsNotSameReferenceAs(original);
            var snapshot = aggregate ? ((AggregateException) failure).InnerExceptions[0] : failure;
            await Assert.That(snapshot is PipelineFailedException).IsFalse();
        }
        else
        {
            await Assert.That(failure).IsSameReferenceAs(original);
        }
    }

    [Test]
    public async Task SensitiveCancellation_PreservesCancellationAndToken()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var original = new OperationCanceledException(Secret, new Exception(Secret), cancellation.Token);

        var failure = await RunWithFailure(original, registerSecret: true);

        await AssertMasked(failure);
        await Assert.That(failure).IsAssignableTo<OperationCanceledException>();
        await Assert.That(((OperationCanceledException) failure).CancellationToken).IsEqualTo(cancellation.Token);
    }

    private static async Task<Exception> RunWithFailure(Exception exception, bool registerSecret)
    {
        var orchestrator = new Mock<IExecutionOrchestrator>();
        orchestrator.Setup(service => service.ExecuteAsync(It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromException<PipelineSummary>(exception));
        var builder = TestPipelineBuilder.Create().AddModule<SecretFailureModule>();
        builder.Services.AddSingleton(orchestrator.Object);
        await using var pipeline = await builder.BuildAsync();
        if (registerSecret)
        {
            pipeline.Services.GetRequiredService<ISecretRegistry>().AddSecret(Secret);
        }

        var run = pipeline.RunAsync();
        var failure = await Assert.ThrowsAsync<Exception>(() => run);
        await Assert.That(run.IsCanceled).IsEqualTo(exception is OperationCanceledException);
        return failure!;
    }

    private static async Task AssertMasked(Exception exception)
    {
        await Assert.That(exception.Message).DoesNotContain(Secret);
        await Assert.That(exception.ToString()).DoesNotContain(Secret);
        await Assert.That(exception.StackTrace ?? string.Empty).DoesNotContain(Secret);
        await Assert.That(exception.HelpLink ?? string.Empty).DoesNotContain(Secret);
        await Assert.That(exception.Source ?? string.Empty).DoesNotContain(Secret);
        foreach (var key in exception.Data.Keys)
        {
            await Assert.That(key.ToString()!).DoesNotContain(Secret);
            await Assert.That(exception.Data[key]?.ToString() ?? string.Empty).DoesNotContain(Secret);
        }

        if (exception is AggregateException aggregate)
        {
            foreach (var inner in aggregate.InnerExceptions)
            {
                await AssertMasked(inner);
            }
        }
        else if (exception.InnerException is { } inner)
        {
            await AssertMasked(inner);
        }
    }

    private sealed class SecretFailureModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            context.Security.Secrets.AddSecret(Secret);
            throw new InvalidOperationException($"auth failed for {Secret}");
        }
    }

    private sealed class HiddenInnerException(Exception inner) : Exception("ordinary failure", inner)
    {
        public override string ToString() => Message;
    }
}
