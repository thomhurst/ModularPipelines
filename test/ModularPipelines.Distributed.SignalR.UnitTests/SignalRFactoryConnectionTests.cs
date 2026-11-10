using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Distributed.SignalR.Coordination;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class SignalRFactoryConnectionTests
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

    [Test]
    public async Task Unauthorized_Negotiation_Fails_Without_Retrying()
    {
        var attempts = 0;
        await using var server = await StartServerAsync((context, _) =>
        {
            Interlocked.Increment(ref attempts);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        });
        var factory = CreateFactory(server);

        var exception = await Assert.That(async () =>
                await factory.CreateWorkerAsync(CancellationToken.None).WaitAsync(Timeout))
            .Throws<HttpRequestException>();

        await Assert.That(exception!.StatusCode).IsEqualTo(HttpStatusCode.Unauthorized);
        await Assert.That(attempts).IsEqualTo(1);
    }

    [Test]
    [Arguments(HttpTransportType.LongPolling, 1)]
    [Arguments(HttpTransportType.WebSockets | HttpTransportType.LongPolling, 2)]
    public async Task Unauthorized_Transport_Fails_Without_Retrying(HttpTransportType transports, int expectedAttempts)
    {
        var transportAttempts = 0;
        await using var server = await StartServerAsync(async (context, next) =>
        {
            if (context.Request.Path.Value!.EndsWith("/negotiate", StringComparison.Ordinal))
            {
                await next(context);
                return;
            }

            Interlocked.Increment(ref transportAttempts);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }, transports);
        var factory = CreateFactory(server);

        var exception = await Assert.That(async () =>
                await factory.CreateWorkerAsync(CancellationToken.None).WaitAsync(Timeout))
            .Throws<HttpRequestException>();

        await Assert.That(exception!.StatusCode).IsEqualTo(HttpStatusCode.Unauthorized);
        await Assert.That(transportAttempts).IsEqualTo(expectedAttempts);
    }

    [Test]
    public async Task Transient_Negotiation_Failure_Retries_And_Connects()
    {
        var attempts = 0;
        await using var server = await StartServerAsync(async (context, next) =>
        {
            if (context.Request.Path.Value!.EndsWith("/negotiate", StringComparison.Ordinal)
                && Interlocked.Increment(ref attempts) == 1)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return;
            }

            await next(context);
        });
        var factory = CreateFactory(server);

        await using var worker = (IAsyncDisposable) await factory.CreateWorkerAsync(CancellationToken.None).WaitAsync(Timeout);

        await Assert.That(attempts).IsEqualTo(2);
    }

    [Test]
    public async Task Transient_Transport_Failure_Retries_And_Connects()
    {
        var negotiations = 0;
        var transportAttempts = 0;
        await using var server = await StartServerAsync(async (context, next) =>
        {
            if (context.Request.Path.Value!.EndsWith("/negotiate", StringComparison.Ordinal))
            {
                Interlocked.Increment(ref negotiations);
            }
            else if (Interlocked.Increment(ref transportAttempts) == 1)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return;
            }

            await next(context);
        }, HttpTransportType.LongPolling);
        var factory = CreateFactory(server);

        await using var worker = (IAsyncDisposable) await factory.CreateWorkerAsync(CancellationToken.None).WaitAsync(Timeout);

        await Assert.That(negotiations).IsEqualTo(2);
    }

    [Test]
    public async Task Caller_Cancellation_Stops_An_In_Flight_Connection()
    {
        var requested = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        await using var server = await StartServerAsync(async (context, _) =>
        {
            requested.TrySetResult();
            await Task.Delay(System.Threading.Timeout.InfiniteTimeSpan, context.RequestAborted);
        });
        var factory = CreateFactory(server);
        using var cancellation = new CancellationTokenSource();
        var connection = factory.CreateWorkerAsync(cancellation.Token);

        await requested.Task.WaitAsync(Timeout);
        await cancellation.CancelAsync();

        await Assert.That(async () => await connection.WaitAsync(Timeout)).Throws<OperationCanceledException>();
    }

    [Test]
    public async Task Connection_Timeout_Stops_Transient_Retries()
    {
        var attempts = 0;
        await using var server = await StartServerAsync((context, _) =>
        {
            Interlocked.Increment(ref attempts);
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            return Task.CompletedTask;
        });
        var clock = new RetryTrackingTimeProvider();
        var connectionTimeout = TimeSpan.FromSeconds(1);
        var factory = CreateFactory(server, connectionTimeout, clock);
        var connection = factory.CreateWorkerAsync(CancellationToken.None);

        // Wait for the transient failure to reach the retry delay before expiring the timeout.
        await clock.RetryScheduled.Task.WaitAsync(Timeout);
        await Assert.That(connection.IsCompleted).IsFalse();
        clock.Advance(connectionTimeout);

        await Assert.That(async () => await connection.WaitAsync(Timeout))
            .Throws<OperationCanceledException>();

        await Assert.That(attempts).IsEqualTo(1);
    }

    [Test]
    [Arguments(HttpTransportType.WebSockets)]
    [Arguments(HttpTransportType.LongPolling)]
    public async Task Server_Disposal_Drains_Disconnect_Before_Disposing_Services(HttpTransportType transport)
    {
        var disconnect = new BlockingDisconnectFilter();
        await using var server = await StartServerAsync((context, next) => next(context), transport, disconnect);
        disconnect.RootServices = server.Application.Services;
        var stopping = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var subscription = server.Application.Lifetime.ApplicationStopping.Register(() => stopping.TrySetResult());
        await using var worker = (IAsyncDisposable) await CreateFactory(server).CreateWorkerAsync(CancellationToken.None).WaitAsync(Timeout);
        var workerDisposal = worker.DisposeAsync().AsTask();
        Task? serverDisposal = null;
        try
        {
            await disconnect.Started.Task.WaitAsync(Timeout);
            serverDisposal = Task.Run(async () => await server.DisposeAsync());
            await stopping.Task.WaitAsync(Timeout);

            await Assert.That(serverDisposal.IsCompleted).IsFalse();
            // The disconnect callback still needs the root provider to create its hub scope.
            using var scope = server.Application.Services.CreateScope();
            await Assert.That(scope.ServiceProvider.GetRequiredService<IHubContext<ConnectionTestHub>>()).IsNotNull();
        }
        finally
        {
            disconnect.Finish.TrySetResult();
            await Task.WhenAll(workerDisposal, serverDisposal ?? Task.CompletedTask, disconnect.Completed.Task).WaitAsync(Timeout);
        }
    }

    private static SignalRDistributedCoordinatorFactory CreateFactory(
        TestServer server,
        TimeSpan? connectionTimeout = null,
        TimeProvider? timeProvider = null) => new(
        MsOptions.Create(new SignalRDistributedOptions
        {
            ListenUrl = new Uri(server.Application.Urls.Single()),
            ConnectionTimeout = connectionTimeout ?? Timeout,
            MaxReconnectAttempts = 0,
        }),
        MsOptions.Create(new DistributedOptions()),
        NullLoggerFactory.Instance,
        timeProvider: timeProvider);

    private static async Task<TestServer> StartServerAsync(
        Func<HttpContext, RequestDelegate, Task> middleware,
        HttpTransportType transports = HttpTransportType.WebSockets | HttpTransportType.ServerSentEvents | HttpTransportType.LongPolling,
        IHubFilter? filter = null)
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseUrls("http://127.0.0.1:0");
        builder.Logging.ClearProviders();
        builder.Services.AddSignalR(options =>
        {
            if (filter is not null)
            {
                options.AddFilter(filter);
            }
        });
        var server = builder.Build();
        server.Use(middleware);
        server.MapHub<ConnectionTestHub>("/pipeline-hub", options => options.Transports = transports);
        try
        {
            await server.StartAsync();
            return new TestServer(server);
        }
        catch
        {
            await new TestServer(server).DisposeAsync();
            throw;
        }
    }

    private sealed class TestServer(WebApplication application) : IAsyncDisposable
    {
        public WebApplication Application { get; } = application;

        public async ValueTask DisposeAsync()
        {
            try
            {
                // Stop the host while its provider is still available to disconnect callbacks.
                await Application.StopAsync();
            }
            finally
            {
                await Application.DisposeAsync();
            }
        }
    }

    private sealed class BlockingDisconnectFilter : IHubFilter
    {
        public IServiceProvider RootServices { private get; set; } = null!;

        public TaskCompletionSource Started { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public TaskCompletionSource Finish { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public TaskCompletionSource Completed { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public async Task OnDisconnectedAsync(HubLifetimeContext context, Exception? exception,
            Func<HubLifetimeContext, Exception?, Task> next)
        {
            Started.TrySetResult();
            await Finish.Task;
            try
            {
                using var scope = RootServices.CreateScope();
                await next(context, exception);
                Completed.TrySetResult();
            }
            catch (Exception failure)
            {
                Completed.TrySetException(failure);
                throw;
            }
        }
    }

    private sealed class RetryTrackingTimeProvider : FakeTimeProvider
    {
        public TaskCompletionSource RetryScheduled { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public override ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
        {
            var timer = base.CreateTimer(callback, state, dueTime, period);
            if (dueTime == TimeSpan.FromSeconds(2))
            {
                RetryScheduled.TrySetResult();
            }

            return timer;
        }
    }

    public sealed class ConnectionTestHub : Microsoft.AspNetCore.SignalR.Hub;
}
