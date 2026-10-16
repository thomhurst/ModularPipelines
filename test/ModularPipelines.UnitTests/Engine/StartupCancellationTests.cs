using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using ModularPipelines.Validation;

namespace ModularPipelines.UnitTests.Engine;

public class StartupCancellationTests
{
    [Test]
    [Arguments("build")]
    [Arguments("validate")]
    [Arguments("run")]
    public async Task PreCanceledTokenPreventsStartup(string operation)
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var started = false;
        var builder = CreateBuilder();
        builder.Services.AddSingleton<IInitializer>(_ =>
        {
            started = true;
            return new LegacyInitializer(Task.CompletedTask);
        });
        builder.Services.AddSingleton<IPipelineValidator>(new CallbackValidator(_ =>
        {
            started = true;
            return Task.CompletedTask;
        }));

        await Assert.ThrowsAsync<OperationCanceledException>(() => StartAsync(builder, operation, cancellation.Token));
        await Assert.That(started).IsFalse();
    }

    [Test]
    [Arguments("build")]
    [Arguments("validate")]
    [Arguments("run")]
    public async Task CancellationReachesInitializerAndDisposesAfterItStops(string operation)
    {
        using var cancellation = new CancellationTokenSource();
        var initializer = new CancellableInitializer();
        var builder = CreateBuilder();
        builder.Services.AddSingleton<ICancellableInitializer>(_ => initializer);
        var startup = StartAsync(builder, operation, cancellation.Token);
        try
        {
            await initializer.Started.Task.WaitAsync(TimeSpan.FromSeconds(30));
            await Assert.That(initializer.Token).IsEqualTo(cancellation.Token);
            cancellation.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(() => startup);
            await Assert.That(initializer.Finished).IsTrue();
            await Assert.That(initializer.Disposed).IsTrue();
        }
        finally
        {
            cancellation.Cancel();
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task LegacyInitializationFinishesBeforeDisposalAndLaterBatchesDoNotStart(bool includeCancellableInitializer)
    {
        using var cancellation = new CancellationTokenSource();
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var initializer = new LegacyInitializer(release.Task);
        var later = new LaterInitializer();
        var builder = CreateBuilder();
        builder.Services.AddSingleton<IInitializer>(_ => initializer);
        builder.Services.AddSingleton(later);
        if (includeCancellableInitializer)
        {
            builder.Services.AddSingleton<ICancellableInitializer, CancellableInitializer>();
        }

        var startup = builder.BuildAsync(cancellation.Token);
        try
        {
            await initializer.Started.Task.WaitAsync(TimeSpan.FromSeconds(30));
            cancellation.Cancel();
            await Assert.That(startup.IsCompleted).IsFalse();
            await Assert.That(initializer.Disposed).IsFalse();
        }
        finally
        {
            release.TrySetResult();
        }

        await Assert.ThrowsAsync<OperationCanceledException>(() => startup);
        await Assert.That(initializer.Disposed).IsTrue();
        await Assert.That(later.Started).IsFalse();
    }

    [Test]
    public async Task BeforeInitializationReceivesCancellationAndPreventsInitializers()
    {
        using var cancellation = new CancellationTokenSource();
        var initializer = new LaterInitializer();
        var builder = CreateBuilder();
        builder.Services.AddSingleton(initializer);

        await Assert.ThrowsAsync<OperationCanceledException>(() => builder.BuildAsync((_, token) =>
        {
            if (token != cancellation.Token)
            {
                throw new InvalidOperationException("Startup callback received the wrong token.");
            }

            cancellation.Cancel();
            return Task.CompletedTask;
        }, cancellation.Token));

        await Assert.That(initializer.Started).IsFalse();
    }

    [Test]
    [Arguments("build")]
    [Arguments("validate")]
    public async Task PipelineValidationReceivesCancellation(string operation)
    {
        using var cancellation = new CancellationTokenSource();
        var received = CancellationToken.None;
        var builder = CreateBuilder();
        builder.Services.AddSingleton<IPipelineValidator>(new CallbackValidator(token =>
        {
            received = token;
            cancellation.Cancel();
            return Task.CompletedTask;
        }));

        await Assert.ThrowsAsync<OperationCanceledException>(() => StartAsync(builder, operation, cancellation.Token));
        await Assert.That(received).IsEqualTo(cancellation.Token);
    }

    [Test]
    public async Task AsyncOptionsValidationReceivesCancellation()
    {
        using var cancellation = new CancellationTokenSource();
        var validator = new StartupValidator(cancellation);
        var builder = CreateBuilder();
        builder.Services.AddSingleton<IAsyncStartupValidator>(validator);

        await Assert.ThrowsAsync<OperationCanceledException>(() => builder.BuildAsync(cancellation.Token));
        await Assert.That(validator.Token).IsEqualTo(cancellation.Token);
    }

    [Test]
    public async Task LegacyEntryPointForCancellableInitializerUsesDefaultToken()
    {
        var initializer = new ImmediateInitializer();

        await ((IInitializer) initializer).InitializeAsync();

        await Assert.That(initializer.Token).IsEqualTo(CancellationToken.None);
        await Assert.That(initializer.Calls).IsEqualTo(1);
    }

    [Test]
    public async Task CancellableInitializerForwardedThroughLegacyContractRunsOnce()
    {
        using var cancellation = new CancellationTokenSource();
        var builder = CreateBuilder();
        builder.Services.AddSingleton<ImmediateInitializer>();
        builder.Services.AddSingleton<IInitializer>(services => services.GetRequiredService<ImmediateInitializer>());

        await using var pipeline = await builder.BuildAsync(cancellation.Token);
        var initializer = pipeline.Services.GetRequiredService<ImmediateInitializer>();

        await Assert.That(initializer.Token).IsEqualTo(cancellation.Token);
        await Assert.That(initializer.Calls).IsEqualTo(1);
    }

    [Test]
    [Arguments("build")]
    [Arguments("validate")]
    public async Task CancellationInterruptsRunningValidation(string operation)
    {
        using var cancellation = new CancellationTokenSource();
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var builder = CreateBuilder();
        builder.Services.AddSingleton<IPipelineValidator>(new CallbackValidator(async token =>
        {
            started.SetResult();
            await Task.Delay(Timeout.InfiniteTimeSpan, token);
        }));

        var startup = StartAsync(builder, operation, cancellation.Token);
        try
        {
            await started.Task.WaitAsync(TimeSpan.FromSeconds(30));
        }
        finally
        {
            cancellation.Cancel();
        }

        await Assert.ThrowsAsync<OperationCanceledException>(() => startup);
    }

    private static PipelineBuilder CreateBuilder() => TestPipelineBuilder.Create().AddModule<NoOpModule>();

    private static async Task StartAsync(PipelineBuilder builder, string operation, CancellationToken token)
    {
        switch (operation)
        {
            case "build":
                await using (await builder.BuildAsync(token))
                {
                }

                break;
            case "validate":
                await builder.ValidateAsync(token);
                break;
            case "run":
                await builder.RunAsync(token);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(operation));
        }
    }

    private sealed class NoOpModule : Module
    {
        protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.CompletedTask;
    }

    private sealed class CancellableInitializer : ICancellableInitializer, IDisposable
    {
        public TaskCompletionSource Started { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public CancellationToken Token { get; private set; }

        public bool Finished { get; private set; }

        public bool Disposed { get; private set; }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            Token = cancellationToken;
            Started.SetResult();
            try
            {
                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            }
            finally
            {
                Finished = true;
            }
        }

        public void Dispose()
        {
            if (!Finished)
            {
                throw new InvalidOperationException("Disposed while initialization was running.");
            }

            Disposed = true;
        }
    }

    private sealed class LegacyInitializer(Task completion) : IInitializer, IDisposable
    {
        public TaskCompletionSource Started { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public bool Disposed { get; private set; }

        public Task InitializeAsync()
        {
            Started.SetResult();
            return completion;
        }

        public void Dispose() => Disposed = true;
    }

    private sealed class LaterInitializer : IInitializer
    {
        public int Order => int.MaxValue;

        public bool Started { get; private set; }

        public Task InitializeAsync()
        {
            Started = true;
            return Task.CompletedTask;
        }
    }

    private sealed class ImmediateInitializer : ICancellableInitializer
    {
        public CancellationToken Token { get; private set; }

        public int Calls { get; private set; }

        public Task InitializeAsync(CancellationToken cancellationToken)
        {
            Token = cancellationToken;
            Calls++;
            return Task.CompletedTask;
        }
    }

    private sealed class CallbackValidator(Func<CancellationToken, Task> callback) : IPipelineValidator
    {
        public int Order => int.MinValue;

        public async Task<ValidationResult> ValidateAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            await callback(cancellationToken);
            return ValidationResult.Success();
        }
    }

    private sealed class StartupValidator(CancellationTokenSource cancellation) : IAsyncStartupValidator
    {
        public CancellationToken Token { get; private set; }

        public Task ValidateAsync(CancellationToken cancellationToken)
        {
            Token = cancellationToken;
            cancellation.Cancel();
            cancellationToken.ThrowIfCancellationRequested();
            return Task.CompletedTask;
        }
    }
}
