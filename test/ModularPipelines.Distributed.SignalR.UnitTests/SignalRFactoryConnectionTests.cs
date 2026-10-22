using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
        var factory = CreateFactory(server, TimeSpan.FromSeconds(1));

        await Assert.That(async () => await factory.CreateWorkerAsync(CancellationToken.None).WaitAsync(Timeout))
            .Throws<OperationCanceledException>();

        await Assert.That(attempts).IsEqualTo(1);
    }

    private static SignalRDistributedCoordinatorFactory CreateFactory(WebApplication server, TimeSpan? connectionTimeout = null) => new(
        MsOptions.Create(new SignalRDistributedOptions
        {
            ListenUrl = new Uri(server.Urls.Single()),
            ConnectionTimeout = connectionTimeout ?? Timeout,
            MaxReconnectAttempts = 0,
        }),
        MsOptions.Create(new DistributedOptions()),
        NullLoggerFactory.Instance);

    private static async Task<WebApplication> StartServerAsync(
        Func<HttpContext, RequestDelegate, Task> middleware,
        HttpTransportType transports = HttpTransportType.WebSockets | HttpTransportType.ServerSentEvents | HttpTransportType.LongPolling)
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseUrls("http://127.0.0.1:0");
        builder.Logging.ClearProviders();
        builder.Services.AddSignalR();
        var server = builder.Build();
        server.Use(middleware);
        server.MapHub<ConnectionTestHub>("/pipeline-hub", options => options.Transports = transports);
        try
        {
            await server.StartAsync();
            return server;
        }
        catch
        {
            await server.DisposeAsync();
            throw;
        }
    }

    public sealed class ConnectionTestHub : Microsoft.AspNetCore.SignalR.Hub;
}
