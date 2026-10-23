using ModularPipelines.Engine;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Engine;

public class ModuleCancellationScopeTests
{
    [Test]
    public async Task Nested_Scopes_Flow_Across_Await_And_Restore_The_Previous_Token()
    {
        using var module = new CancellationTokenSource();
        using var attempt = new CancellationTokenSource();
        using (new ModuleCancellationScope(module.Token))
        {
            using (new ModuleCancellationScope(attempt.Token))
            {
                await Task.Yield();
                await Assert.That(ModuleCancellationScope.CurrentToken).IsEqualTo(attempt.Token);
            }

            await Assert.That(ModuleCancellationScope.CurrentToken).IsEqualTo(module.Token);
        }

        await Assert.That(ModuleCancellationScope.CurrentToken).IsEqualTo(CancellationToken.None);
    }

    [Test]
    public async Task Flowed_Children_Retain_Their_Original_Canceled_Token_After_Scope_End()
    {
        using var cancellation = new CancellationTokenSource();
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        Task<CancellationToken> child;
        using (new ModuleCancellationScope(cancellation.Token))
        {
            child = Task.Run(async () =>
            {
                await release.Task;
                return ModuleCancellationScope.CurrentToken;
            });
            await cancellation.CancelAsync();
        }

        release.SetResult();
        var token = await child;
        await Assert.That(token).IsEqualTo(cancellation.Token);
        await Assert.That(token.IsCancellationRequested).IsTrue();
        await Assert.That(ModuleCancellationScope.CurrentToken).IsEqualTo(CancellationToken.None);
    }

    [Test]
    public async Task Parallel_Scopes_Do_Not_Replace_Each_Others_Tokens()
    {
        using var first = new CancellationTokenSource();
        using var second = new CancellationTokenSource();
        var bothStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var started = 0;

        async Task<CancellationToken> ReadToken(CancellationToken token)
        {
            using var scope = new ModuleCancellationScope(token);
            await Task.Yield();
            // Both scopes must be active before either reads its token.
            if (Interlocked.Increment(ref started) == 2)
            {
                bothStarted.TrySetResult();
            }

            await bothStarted.Task.WaitAsync(TestHostSettings.DefaultTestTimeout);
            return ModuleCancellationScope.CurrentToken;
        }

        var tokens = await Task.WhenAll(Task.Run(() => ReadToken(first.Token)), Task.Run(() => ReadToken(second.Token)));
        await Assert.That(tokens).IsEquivalentTo(new[] { first.Token, second.Token });
    }
}
