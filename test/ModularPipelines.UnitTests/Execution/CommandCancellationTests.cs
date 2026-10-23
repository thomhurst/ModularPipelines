using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.ProcessTestHost;
using ModularPipelines.TestHelpers;
using ModularPipelines.UnitTests.Attributes;
using ModularPipelines.UnitTests.Helpers;
using EngineCancellationToken = ModularPipelines.Engine.EngineCancellationToken;

namespace ModularPipelines.UnitTests.Execution;

public class CommandCancellationTests : TestBase
{
    private static readonly TimeSpan AttemptTimeout = TimeSpan.FromSeconds(1);

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Default_Command_Token_Observes_Each_Attempt_Timeout(bool useShell)
    {
        var time = new FakeTimeProvider();
        var interceptor = new WaitingInterceptor();
        var builder = TestPipelineBuilder.Create(new TestHostSettings(), time)
            .AddModule<TimedCommandModule>();
        builder.Services.AddSingleton<ICommandInterceptor>(interceptor);
        await using var host = await builder.BuildAsync();
        var module = host.Services.GetServices<IModule>().OfType<TimedCommandModule>().Single();
        module.UseShell = useShell;
        var run = host.RunAsync();
        try
        {
            for (var attempt = 0; attempt < 2; attempt++)
            {
                var token = await interceptor.Started.Reader.ReadAsync().AsTask()
                    .WaitAsync(TestHostSettings.DefaultTestTimeout);
                await Assert.That(token.IsCancellationRequested).IsFalse();
                time.Advance(AttemptTimeout);
                await Assert.That(token.IsCancellationRequested).IsTrue();
            }

            var exception = await Assert.ThrowsAsync<ModuleFailedException>(
                () => run.WaitAsync(TestHostSettings.DefaultTestTimeout));
            var timeout = exception!.InnerException as ModuleTimeoutException;
            await Assert.That(timeout).IsNotNull();
            await Assert.That(timeout!.WasCancellationTokenRespected).IsTrue();
            await Assert.That(module.AttemptTokens.Count).IsEqualTo(2);
            await Assert.That(module.AttemptTokens[0]).IsNotEqualTo(module.AttemptTokens[1]);
            await Assert.That(interceptor.CanceledCommands).IsEqualTo(2);
        }
        finally
        {
            interceptor.Complete();
            await run.WaitAsync(TestHostSettings.DefaultTestTimeout).ContinueWith(_ => { });
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Default_Command_Token_Observes_Pipeline_Cancellation(bool useShell)
    {
        var interceptor = new WaitingInterceptor();
        var builder = TestPipelineBuilder.Create().AddModule<CommandModule>();
        builder.Services.AddSingleton<ICommandInterceptor>(interceptor);
        await using var host = await builder.BuildAsync();
        host.Services.GetServices<IModule>().OfType<CommandModule>().Single().UseShell = useShell;
        var run = host.RunAsync();
        try
        {
            var token = await interceptor.Started.Reader.ReadAsync().AsTask()
                .WaitAsync(TestHostSettings.DefaultTestTimeout);
            host.Services.GetRequiredService<EngineCancellationToken>().Cancel();
            await Assert.That(token.IsCancellationRequested).IsTrue();
            await run.WaitAsync(TestHostSettings.DefaultTestTimeout);
            await Assert.That(interceptor.CanceledCommands).IsEqualTo(1);
        }
        finally
        {
            interceptor.Complete();
            await run.WaitAsync(TestHostSettings.DefaultTestTimeout).ContinueWith(_ => { });
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Explicit_Command_Token_Takes_Precedence_Over_Module_Cancellation(bool useShell)
    {
        using var explicitCancellation = new CancellationTokenSource();
        var interceptor = new WaitingInterceptor();
        var builder = TestPipelineBuilder.Create().AddModule<CommandModule>();
        builder.Services.AddSingleton<ICommandInterceptor>(interceptor);
        await using var host = await builder.BuildAsync();
        var module = host.Services.GetServices<IModule>().OfType<CommandModule>().Single();
        module.UseShell = useShell;
        module.CallerToken = explicitCancellation.Token;
        var run = host.RunAsync();
        try
        {
            var token = await interceptor.Started.Reader.ReadAsync().AsTask()
                .WaitAsync(TestHostSettings.DefaultTestTimeout);
            host.Services.GetRequiredService<EngineCancellationToken>().Cancel();
            await Assert.That(module.AttemptTokens.Single().IsCancellationRequested).IsTrue();
            await Assert.That(token.IsCancellationRequested).IsFalse();
            await explicitCancellation.CancelAsync();
            await Assert.That(token.IsCancellationRequested).IsTrue();
            await run.WaitAsync(TestHostSettings.DefaultTestTimeout);
        }
        finally
        {
            interceptor.Complete();
            await run.WaitAsync(TestHostSettings.DefaultTestTimeout).ContinueWith(_ => { });
        }
    }

    [Test]
    public async Task Default_Command_Outside_Module_Completes()
    {
        var interceptor = new WaitingInterceptor();
        interceptor.Complete();
        var (command, _) = await GetService<ICommandContext>(services =>
            services.AddSingleton<ICommandInterceptor>(interceptor));

        await command.ExecuteCommandLineToolAsync(new CommandLineToolOptions("unused"),
            new CommandExecutionOptions { ExecutionTimeout = null });

        var token = await interceptor.Started.Reader.ReadAsync();
        await Assert.That(token.IsCancellationRequested).IsFalse();
    }

    [Test]
    public async Task Already_Canceled_Module_Token_Prevents_Command_Interception()
    {
        var interceptor = new WaitingInterceptor();
        interceptor.Complete();
        var (command, _) = await GetService<ICommandContext>(services =>
            services.AddSingleton<ICommandInterceptor>(interceptor));
        using var cancellation = new CancellationTokenSource();
        await cancellation.CancelAsync();
        using var scope = new ModuleCancellationScope(cancellation.Token);

        var exception = await Assert.ThrowsAsync<OperationCanceledException>(() =>
            command.ExecuteCommandLineToolAsync(new CommandLineToolOptions("unused")));

        await Assert.That(exception!.CancellationToken).IsEqualTo(cancellation.Token);
        await Assert.That(interceptor.Started.Reader.TryRead(out _)).IsFalse();
    }

    [Test]
    [RequiresTool("dotnet")]
    [TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
    public async Task Ambient_Cancellation_Stops_A_Running_Process_And_Preserves_The_Caller_Token()
    {
        var directory = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        using var cancellation = new CancellationTokenSource();
        using var scope = new ModuleCancellationScope(cancellation.Token);
        var command = await GetService<ICommandContext>();
        var runtimeConfig = Path.ChangeExtension(typeof(CommandCancellationTests).Assembly.Location, ".runtimeconfig.json");
        var execution = command.ExecuteCommandLineToolAsync(new CommandLineToolOptions("dotnet")
        {
            Arguments = ["exec", "--runtimeconfig", runtimeConfig, typeof(Host).Assembly.Location,
                "graceful-exit", directory, runtimeConfig],
        }, new CommandExecutionOptions { GracefulShutdownTimeout = TimeSpan.Zero });

        try
        {
            async Task WaitForReadyAsync()
            {
                while (!File.Exists(Path.Combine(directory, "parent.ready")))
                {
                    if (execution.IsCompleted)
                    {
                        await execution;
                        throw new InvalidOperationException("The process exited before signaling readiness.");
                    }

                    await Task.Delay(20);
                }
            }

            await WaitForReadyAsync().WaitAsync(TestHostSettings.DefaultTestTimeout);
            await cancellation.CancelAsync();
            var exception = await Assert.ThrowsAsync<OperationCanceledException>(
                () => execution.WaitAsync(TestHostSettings.DefaultTestTimeout));
            await Assert.That(exception!.CancellationToken).IsEqualTo(cancellation.Token);
        }
        finally
        {
            // Release the process even if the ambient-token fallback regresses.
            await File.WriteAllTextAsync(Path.Combine(directory, "parent-exit.trigger"), string.Empty);
            await execution.WaitAsync(TestHostSettings.DefaultTestTimeout).ContinueWith(_ => { });
            Directory.Delete(directory, recursive: true);
        }
    }

    private class CommandModule : Module<int>
    {
        public bool UseShell { get; set; }
        public CancellationToken CallerToken { get; set; }
        public List<CancellationToken> AttemptTokens { get; } = [];

        protected internal override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            AttemptTokens.Add(cancellationToken);
            // Exercise the async lambdas that the cancellation-token analyzer does not inspect.
            await Task.WhenAll(Enumerable.Range(0, 1).Select(async _ =>
            {
                await Task.Yield();
                var options = new CommandLineToolOptions("unused");
                if (UseShell)
                {
                    await context.Shell.RunAsync(options, cancellationToken: CallerToken);
                }
                else
                {
                    await context.Services.GetRequiredService<ICommandContext>()
                        .ExecuteCommandLineToolAsync(options, cancellationToken: CallerToken);
                }
            }));
            return 42;
        }
    }

    private sealed class TimedCommandModule : CommandModule
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithTimeout(AttemptTimeout)
            .WithRetry(1, TimeSpan.Zero);
    }

    private sealed class WaitingInterceptor : ICommandInterceptor
    {
        private readonly TaskCompletionSource _completion = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public Channel<CancellationToken> Started { get; } = Channel.CreateUnbounded<CancellationToken>();
        public int CanceledCommands { get; private set; }

        public async ValueTask<CommandResult> InvokeAsync(
            CommandInvocation invocation, CommandDelegate next, CancellationToken cancellationToken)
        {
            try
            {
                var wait = _completion.Task.WaitAsync(cancellationToken);
                Started.Writer.TryWrite(cancellationToken);
                await wait;
                return CommandResult.Ok();
            }
            finally
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    CanceledCommands++;
                }
            }
        }

        public void Complete() => _completion.TrySetResult();
    }
}
