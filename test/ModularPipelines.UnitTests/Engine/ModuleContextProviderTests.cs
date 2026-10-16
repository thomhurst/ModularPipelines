using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class ModuleContextProviderTests
{
    [Test]
    public async Task Reuses_Contexts_Per_Owner_Without_Sharing_Between_Equal_Owners()
    {
        await using var services = new ServiceCollection()
            .AddScoped<IPipelineContext>(_ => Mock.Of<IPipelineContext>())
            .BuildServiceProvider();
        await using var provider = new ModuleContextProvider(services);
        var firstOwner = new ScopeOwner(1);
        var equalOwner = new ScopeOwner(1);

        var pipelineContext = provider.GetModuleContext();
        var firstContext = provider.GetModuleContext(firstOwner);
        var secondContext = provider.GetModuleContext(equalOwner);

        await Assert.That(provider.GetModuleContext()).IsSameReferenceAs(pipelineContext);
        await Assert.That(provider.GetModuleContext(firstOwner)).IsSameReferenceAs(firstContext);
        await Assert.That(secondContext).IsNotSameReferenceAs(firstContext);
        await Assert.That(firstContext).IsNotSameReferenceAs(pipelineContext);
        await Assert.That(provider.GetScopes().Count()).IsEqualTo(3);
    }

    [Test]
    public async Task Concurrent_Calls_For_One_Owner_Create_One_Context_And_Scope()
    {
        var created = 0;
        await using var services = new ServiceCollection()
            .AddScoped<IPipelineContext>(_ =>
            {
                Interlocked.Increment(ref created);
                return Mock.Of<IPipelineContext>();
            })
            .BuildServiceProvider();
        await using var provider = new ModuleContextProvider(services);
        var owner = new object();

        var contexts = await Task.WhenAll(Enumerable.Range(0, 64)
            .Select(_ => Task.Run(() => provider.GetModuleContext(owner))));

        await Assert.That(contexts.All(context => ReferenceEquals(context, contexts[0]))).IsTrue();
        await Assert.That(created).IsEqualTo(1);
        await Assert.That(provider.GetScopes().Count()).IsEqualTo(1);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Disposal_Attempts_Every_Scope_And_Aggregates_Failures(bool asynchronously)
    {
        var firstFailure = new InvalidOperationException("first scope");
        var lastFailure = new ArgumentException("last scope");
        RecordingScope[] scopes = [new(firstFailure), new(), new(lastFailure)];
        var provider = CreateProvider(scopes);
        foreach (var scope in scopes)
        {
            provider.GetModuleContext(scope);
        }

        var exception = await Assert.ThrowsAsync<AggregateException>(async () =>
        {
            if (asynchronously)
            {
                await provider.DisposeAsync();
            }
            else
            {
                ((IDisposable) provider).Dispose();
            }
        });

        await Assert.That(exception!.InnerExceptions.Count).IsEqualTo(2);
        await Assert.That(exception.InnerExceptions.Contains(firstFailure)).IsTrue();
        await Assert.That(exception.InnerExceptions.Contains(lastFailure)).IsTrue();
        await Assert.That(scopes.All(scope => scope.AsyncDisposals == (asynchronously ? 1 : 0))).IsTrue();
        await Assert.That(scopes.All(scope => scope.SyncDisposals == (asynchronously ? 0 : 1))).IsTrue();
        await Assert.That(provider.GetScopes().Any()).IsFalse();
        Assert.Throws<ObjectDisposedException>(() => provider.GetModuleContext());

        await provider.DisposeAsync();
        ((IDisposable) provider).Dispose();
        await Assert.That(scopes.All(scope => scope.AsyncDisposals + scope.SyncDisposals == 1)).IsTrue();
    }

    [Test]
    public async Task Disposal_Captures_A_Scope_Whose_Creation_Is_In_Progress()
    {
        var entered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var release = new ManualResetEventSlim();
        var scope = new RecordingScope();
        var factory = new Mock<IServiceScopeFactory>();
        factory.Setup(value => value.CreateScope()).Returns(() =>
        {
            entered.SetResult();
            release.Wait();
            return scope;
        });
        var services = Mock.Of<IServiceProvider>(value => value.GetService(typeof(IServiceScopeFactory)) == factory.Object);
        var provider = new ModuleContextProvider(services);
        var creation = Task.Run(() => provider.GetModuleContext(new object()));
        Task disposal = Task.CompletedTask;
        try
        {
            await entered.Task.WaitAsync(TimeSpan.FromSeconds(10));
            disposal = Task.Run(async () => await provider.DisposeAsync());
        }
        finally
        {
            release.Set();
            await Task.WhenAll(creation, disposal).WaitAsync(TimeSpan.FromSeconds(10));
            await provider.DisposeAsync();
        }

        await Assert.That(scope.AsyncDisposals).IsEqualTo(1);
        Assert.Throws<ObjectDisposedException>(() => provider.GetModuleContext(new object()));
    }

    private static ModuleContextProvider CreateProvider(IEnumerable<RecordingScope> scopes)
    {
        var remaining = new Queue<RecordingScope>(scopes);
        var factory = new Mock<IServiceScopeFactory>();
        factory.Setup(value => value.CreateScope()).Returns(() => remaining.Dequeue());
        return new ModuleContextProvider(Mock.Of<IServiceProvider>(value =>
            value.GetService(typeof(IServiceScopeFactory)) == factory.Object));
    }

    private sealed record ScopeOwner(int Value);

    private sealed class RecordingScope(Exception? failure = null) : IServiceScope, IAsyncDisposable
    {
        public IServiceProvider ServiceProvider { get; } = Mock.Of<IServiceProvider>(value =>
            value.GetService(typeof(IPipelineContext)) == Mock.Of<IPipelineContext>());

        public int AsyncDisposals { get; private set; }

        public int SyncDisposals { get; private set; }

        public async ValueTask DisposeAsync()
        {
            await Task.Yield();
            AsyncDisposals++;
            if (failure is not null)
            {
                throw failure;
            }
        }

        public void Dispose()
        {
            SyncDisposals++;
            if (failure is not null)
            {
                throw failure;
            }
        }
    }
}
