using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Http;
using ModularPipelines.UnitTests.Helpers;

namespace ModularPipelines.UnitTests.Context;

public partial class HttpTests
{
    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task LegacyLogger_CancellationObservesInterruptedBufferingFault(
        bool logRequest, bool withOptions)
    {
        var failure = new ObjectDisposedException($"Interrupted body {Guid.NewGuid()}");

        await TaskFaultAssertions.AssertNoUnobservedException(
            failure, () => InterruptLegacyBufferingAsync(failure, logRequest, withOptions));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static async Task<WeakReference> InterruptLegacyBufferingAsync(
        Exception failure, bool logRequest, bool withOptions)
    {
        using var stream = new BlockingReadStream(
            ignoreAsyncCancellation: true, disposalException: failure);
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test/interrupted-body")
        {
            Content = new StreamContent(stream),
        };
        using var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StreamContent(stream),
        };
        using var cancellation = new CancellationTokenSource();
        var legacyLogger = new LegacyBodyLogger(logRequest);
        IHttpLogger logger = legacyLogger;
        var loggingTask = (logRequest, withOptions) switch
        {
            (true, true) => logger.PrintRequest(request, NullLogger.Instance, RequestOnly, cancellation.Token),
            (true, false) => logger.PrintRequest(request, NullLogger.Instance, cancellation.Token),
            (false, true) => logger.PrintResponse(response, NullLogger.Instance, ResponseOnly, cancellation.Token),
            (false, false) => logger.PrintResponse(response, NullLogger.Instance, cancellation.Token),
        };

        await stream.ReadStarted.WaitAsync(TimeSpan.FromSeconds(10));
        await cancellation.CancelAsync();
        var canceled = await Assert.ThrowsAsync<OperationCanceledException>(() => loggingTask);
        await Assert.That(canceled!.CancellationToken).IsEqualTo(cancellation.Token);
        await Assert.That(legacyLogger.BodyTask!.IsCompleted).IsFalse();

        // Cancellation stops waiting. Disposing the content interrupts the underlying buffer copy later.
        if (logRequest)
        {
            request.Dispose();
        }
        else
        {
            response.Dispose();
        }

        // Wait for completion without observing the fault under test.
        await legacyLogger.BodyTask.ContinueWith(
            static _ => { }, CancellationToken.None,
            TaskContinuationOptions.None, TaskScheduler.Default).WaitAsync(TimeSpan.FromSeconds(10));
        await Assert.That(legacyLogger.BodyTask.IsFaulted).IsTrue();
        return new WeakReference(legacyLogger.BodyTask);
    }
}
