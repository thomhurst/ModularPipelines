using System.Net;
using ModularPipelines.Exceptions;

namespace ModularPipelines.UnitTests.Api;

public class ExceptionApiSurfaceTests
{
    [Test]
    public async Task PipelineExceptionsExposeConsistentHierarchy()
    {
        using (Assert.Multiple())
        {
            await Assert.That(typeof(PipelineException).IsAssignableFrom(typeof(MissingCommandMetadataException)))
                .IsTrue();
            await Assert.That(typeof(PipelineException).IsAssignableFrom(typeof(MissingSecretMetadataException)))
                .IsTrue();
            await Assert.That(typeof(OperationCanceledException).IsAssignableFrom(typeof(PipelineCanceledException)))
                .IsTrue();
            await Assert.That(typeof(HttpRequestException).IsAssignableFrom(typeof(PipelineHttpResponseException)))
                .IsTrue();
        }
    }

    [Test]
    public async Task RenamedAndRemovedExceptionsAreAbsent()
    {
        var assembly = typeof(PipelineException).Assembly;

        using (Assert.Multiple())
        {
            await Assert.That(assembly.GetType("ModularPipelines.Exceptions.PipelineCancelledException"))
                .IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.Exceptions.ModuleReferencingSelfException"))
                .IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.Exceptions.HttpResponseException"))
                .IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.Exceptions.SubModuleFailedException"))
                .IsNull();
        }
    }

    [Test]
    public async Task ExceptionDetailsArePubliclyConstructibleAndInspectable()
    {
        var timeout = new ModuleTimeoutException(
            typeof(ExceptionApiSurfaceTests),
            TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(12),
            wasCancellationTokenRespected: false);
        using var cancellationTokenSource = new CancellationTokenSource();
        var canceled = new PipelineCanceledException("Canceled", cancellationTokenSource.Token);
        var http = new PipelineHttpResponseException(
            HttpStatusCode.BadGateway,
            "Bad Gateway",
            "response",
            new Uri("https://example.test"));

        using (Assert.Multiple())
        {
            await Assert.That(timeout.ModuleType).IsEqualTo(typeof(ExceptionApiSurfaceTests));
            await Assert.That(timeout.ElapsedTime).IsEqualTo(TimeSpan.FromSeconds(12));
            await Assert.That(canceled.CancellationToken).IsEqualTo(cancellationTokenSource.Token);
            await Assert.That(http.StatusCode).IsEqualTo(HttpStatusCode.BadGateway);
            await Assert.That(typeof(DependencyFailedException)
                    .GetProperty(nameof(DependencyFailedException.FailingModuleType))!
                    .GetMethod!
                    .IsPublic)
                .IsTrue();
        }
    }
}
