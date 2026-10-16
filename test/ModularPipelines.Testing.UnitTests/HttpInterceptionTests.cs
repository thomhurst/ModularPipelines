using System.Net;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Testing.UnitTests;

public class HttpInterceptionTests
{
    [Test]
    public async Task InterceptsHttpAndSnapshotsDisposedRequests()
    {
        var run = await ModuleTester.For<HttpModule, string>()
            .InterceptHttp((request, _) => ValueTask.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(request.RequestUri!.AbsolutePath),
            }))
            .ExecuteAsync();

        await Assert.That(run.Value).IsEqualTo("/release");
        await Assert.That(run.HttpRequests.Count).IsEqualTo(1);
        var request = run.HttpRequests[0];
        await Assert.That(request.Method).IsEqualTo(HttpMethod.Post);
        await Assert.That(request.RequestUri).IsEqualTo(new Uri("https://module-test.invalid/release"));
        await Assert.That(request.Headers.GetValues("X-Test").Single()).IsEqualTo("header");
        await Assert.That(request.Content!.Headers.ContentType!.MediaType).IsEqualTo("text/plain");
        await Assert.That(await request.Content.ReadAsStringAsync()).IsEqualTo("payload");
    }

    [Test]
    public async Task InterceptsDownloaderAndReturnsUntypedRun()
    {
        var run = await ModuleTester.For<DownloadModule>()
            .InterceptHttp((_, _) => ValueTask.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("installer"),
            }))
            .ExecuteAsync();

        await Assert.That(run.Value).IsEqualTo("installer");
        await Assert.That(run.HttpRequests.Count).IsEqualTo(1);
        await Assert.That(run.HttpRequests[0].Method).IsEqualTo(HttpMethod.Get);
        await Assert.That(run.HttpRequests[0].RequestUri).IsEqualTo(new Uri("https://module-test.invalid/installer"));
    }

    [Test]
    public async Task ConcurrentRequestsAndRepeatedRunsHaveSeparateRecordings()
    {
        var builder = ModuleTester.For<ConcurrentHttpModule, string>()
            .InterceptHttp(async (request, cancellationToken) =>
            {
                await Task.Yield();
                cancellationToken.ThrowIfCancellationRequested();
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(request.RequestUri!.AbsolutePath),
                };
            });

        var runs = await Task.WhenAll(builder.ExecuteAsync(), builder.ExecuteAsync());

        foreach (var run in runs)
        {
            await Assert.That(run.Value).IsEqualTo("/first,/second");
            await Assert.That(run.HttpRequests.Count).IsEqualTo(2);
            await Assert.That(run.HttpRequests.Select(request => request.RequestUri!.AbsolutePath).Order())
                .IsEquivalentTo(["/first", "/second"]);
        }
    }

    [Test]
    public async Task RecordsRequestWhenHandlerFails()
    {
        var failure = new InvalidOperationException("intercepted failure");
        var run = await ModuleTester.For<HttpModule, string>()
            .InterceptHttp((_, _) => throw failure)
            .ExecuteAsync();

        await Assert.That(run.Exception).IsSameReferenceAs(failure);
        await Assert.That(run.HttpRequests.Count).IsEqualTo(1);
    }

    [Test]
    [Timeout(10_000)]
    public async Task PassesCallerCancellationToHandler(CancellationToken cancellationToken)
    {
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var execution = ModuleTester.For<HttpModule, string>()
            .InterceptHttp(async (_, token) =>
            {
                started.SetResult();
                await Task.Delay(Timeout.InfiniteTimeSpan, token);
                return new HttpResponseMessage(HttpStatusCode.OK);
            })
            .ExecuteAsync(source.Token);

        await started.Task.WaitAsync(cancellationToken);
        await source.CancelAsync();
        var run = await execution.WaitAsync(cancellationToken);

        await Assert.That(run.Exception).IsAssignableTo<OperationCanceledException>();
        await Assert.That(run.HttpRequests.Count).IsEqualTo(1);
    }

    [Test]
    public async Task DoesNotReplaceOtherNamedClients()
    {
        var run = await ModuleTester.For<NamedClientModule, string>()
            .ConfigurePipeline(builder => builder.Services.AddHttpClient("custom")
                .ConfigurePrimaryHttpMessageHandler(() => new CustomHandler()))
            .InterceptHttp((_, _) => throw new InvalidOperationException("Wrong client intercepted."))
            .ExecuteAsync();

        await Assert.That(run.Value).IsEqualTo("custom response");
        await Assert.That(run.HttpRequests.Count).IsEqualTo(0);
    }

    [Test]
    public async Task PreservesRetriesAndRecordsEveryAttempt()
    {
        var attempts = 0;
        var run = await ModuleTester.For<HttpModule, string>()
            .ConfigurePipeline(builder => builder.ConfigureOptions(options => options with
            {
                Http = options.Http with
                {
                    Resilience = new HttpResilienceOptions
                    {
                        MaxRetryAttempts = 1,
                        InitialDelay = TimeSpan.Zero,
                        JitterFactor = 0,
                    },
                },
            }))
            .InterceptHttp((_, _) => ValueTask.FromResult(new HttpResponseMessage(
                Interlocked.Increment(ref attempts) == 1 ? HttpStatusCode.ServiceUnavailable : HttpStatusCode.OK)
            {
                Content = new StringContent("retried response"),
            }))
            .ExecuteAsync();

        await Assert.That(run.Value).IsEqualTo("retried response");
        await Assert.That(run.HttpRequests.Count).IsEqualTo(2);
        foreach (var request in run.HttpRequests)
        {
            await Assert.That(await request.Content!.ReadAsStringAsync()).IsEqualTo("payload");
        }
    }

    [Test]
    public async Task ExplicitClientWorksWithoutInterception()
    {
        using var client = new HttpClient(new CustomHandler());
        var run = await ModuleTester.For<ExplicitClientModule, string>()
            .WithService(client)
            .ExecuteAsync();

        await Assert.That(run.Value).IsEqualTo("custom response");
        await Assert.That(run.HttpRequests.Count).IsEqualTo(0);
    }

    [Test]
    public async Task DoesNotReplaceExplicitClient()
    {
        using var client = new HttpClient(new CustomHandler());
        var run = await ModuleTester.For<ExplicitClientModule, string>()
            .WithService(client)
            .InterceptHttp((_, _) => throw new InvalidOperationException("Wrong client intercepted."))
            .ExecuteAsync();

        await Assert.That(run.Value).IsEqualTo("custom response");
        await Assert.That(run.HttpRequests.Count).IsEqualTo(0);
    }

    [Test]
    public async Task RejectsNullHandler()
    {
        await Assert.That(() => ModuleTester.For<HttpModule>().InterceptHttp(null!))
            .Throws<ArgumentNullException>();
        await Assert.That(() => ModuleTester.For<HttpModule, string>().InterceptHttp(null!))
            .Throws<ArgumentNullException>();
    }

    public sealed class HttpModule : Module<string>
    {
        protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "https://module-test.invalid/release")
            {
                Content = new StringContent("payload"),
            };
            request.Headers.Add("X-Test", "header");
            using var response = await context.Network.Http.SendAsync(request, cancellationToken);
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
    }

    public sealed class DownloadModule : Module<string>
    {
        protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var path = await context.Network.Downloader.DownloadFileAsync(
                new DownloadFileOptions(new Uri("https://module-test.invalid/installer")), cancellationToken);
            return await context.Files.GetFile(path).ReadAsync(cancellationToken);
        }
    }

    public sealed class ConcurrentHttpModule : Module<string>
    {
        protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            async Task<string> SendAsync(string path)
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, $"https://module-test.invalid/{path}");
                using var response = await context.Network.Http.SendAsync(request, cancellationToken);
                return await response.Content.ReadAsStringAsync(cancellationToken);
            }

            return string.Join(',', await Task.WhenAll(SendAsync("first"), SendAsync("second")));
        }
    }

    public sealed class NamedClientModule(IHttpClientFactory factory) : Module<string>
    {
        protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            using var client = factory.CreateClient("custom");
            return await client.GetStringAsync("https://module-test.invalid/custom", cancellationToken);
        }
    }

    public sealed class ExplicitClientModule(HttpClient client) : Module<string>
    {
        protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://module-test.invalid/explicit");
            using var response = await context.Network.Http.SendAsync(new HttpOptions(request)
            {
                HttpClient = client,
            }, cancellationToken);
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
    }

    private sealed class CustomHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("custom response"),
            });
    }
}
