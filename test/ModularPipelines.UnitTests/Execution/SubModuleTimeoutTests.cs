using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Execution;

public class SubModuleTimeoutTests
{
    private static readonly TimeSpan AttemptTimeout = TimeSpan.FromSeconds(1);

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task Default_SubModule_Token_Observes_Each_Attempt_Timeout(bool returnsResult, bool succeedsOnRetry)
    {
        var time = new FakeTimeProvider();
        await using var host = await TestPipelineBuilder.Create(new TestHostSettings(), time)
            .AddModule<TimedSubModule>()
            .BuildAsync();
        var module = host.Services.GetServices<IModule>().OfType<TimedSubModule>().Single();
        module.ReturnsResult = returnsResult;
        module.SucceedsOnRetry = succeedsOnRetry;

        var run = host.RunAsync();
        try
        {
            for (var attempt = 0; attempt < (succeedsOnRetry ? 1 : 2); attempt++)
            {
                var token = await module.StartedChildren.Reader.ReadAsync().AsTask()
                    .WaitAsync(TestHostSettings.DefaultTestTimeout);
                // Expire only an attempt whose child is ready. The successful retry keeps
                // its full budget regardless of how long its asynchronous hop is queued.
                await Assert.That(token.IsCancellationRequested).IsFalse();
                time.Advance(AttemptTimeout);
                await Assert.That(token.IsCancellationRequested).IsTrue();
            }

            if (succeedsOnRetry)
            {
                await run.WaitAsync(TestHostSettings.DefaultTestTimeout);
            }
            else
            {
                var exception = await Assert.ThrowsAsync<ModuleFailedException>(
                    () => run.WaitAsync(TestHostSettings.DefaultTestTimeout));
                var timeoutException = exception!.InnerException as ModuleTimeoutException;
                await Assert.That(timeoutException).IsNotNull();
                await Assert.That(timeoutException!.WasCancellationTokenRespected).IsTrue();
            }

            await Assert.That(module.AttemptTokens.Count).IsEqualTo(2);
            await Assert.That(module.SubModuleTokens.SequenceEqual(module.AttemptTokens)).IsTrue();
            await Assert.That(module.AttemptTokens[0]).IsNotEqualTo(module.AttemptTokens[1]);
            await Assert.That(module.CanceledSubModules).IsEqualTo(succeedsOnRetry ? 1 : 2);
            await Assert.That(module.AttemptTokens[1].IsCancellationRequested).IsEqualTo(!succeedsOnRetry);
        }
        finally
        {
            // Release abandoned work even when the regression causes the timeout token to be ignored.
            module.Complete();
        }
    }

    private sealed class TimedSubModule : Module<int>
    {
        private readonly TaskCompletionSource<int> _completion = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public bool ReturnsResult { get; set; }
        public bool SucceedsOnRetry { get; set; }
        public Channel<CancellationToken> StartedChildren { get; } = Channel.CreateUnbounded<CancellationToken>();
        public List<CancellationToken> AttemptTokens { get; } = [];
        public List<CancellationToken> SubModuleTokens { get; } = [];
        public int CanceledSubModules { get; private set; }

        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithTimeout(AttemptTimeout)
            .WithRetry(1, TimeSpan.Zero);

        protected internal override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            AttemptTokens.Add(cancellationToken);
            await Task.Yield();

            async Task<int> Body(CancellationToken token)
            {
                SubModuleTokens.Add(token);
                if (SucceedsOnRetry && context.Attempt == 2)
                {
                    token.ThrowIfCancellationRequested();
                    return 42;
                }

                try
                {
                    var completion = _completion.Task.WaitAsync(token);
                    StartedChildren.Writer.TryWrite(token);
                    return await completion;
                }
                finally
                {
                    if (token.IsCancellationRequested)
                    {
                        CanceledSubModules++;
                    }
                }
            }

            if (ReturnsResult)
            {
                return await context.RunSubModuleAsync("Timed child", Body);
            }

            await context.RunSubModuleAsync("Timed child", token => (Task) Body(token));
            return 42;
        }

        public void Complete() => _completion.TrySetResult(42);
    }
}
