using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Engine;

public class ModuleDisposalTests
{
    [Test]
    [Arguments("type", false)]
    [Arguments("type", true)]
    [Arguments("factory", false)]
    [Arguments("factory", true)]
    [Arguments("instance", false)]
    [Arguments("instance", true)]
    [Arguments("runtime", false)]
    [Arguments("runtime", true)]
    public async Task Modules_Stay_Alive_For_Dependents_And_Dispose_Once_With_Pipeline(
        string registration, bool showProgress)
    {
        var builder = CreateBuilder(registration, showProgress);
        builder.AddModule<DependentModule>();
        var pipeline = await builder.BuildAsync();
        var modules = pipeline.Services.GetServices<IModule>().OfType<ResourceModule>().ToArray();

        try
        {
            var summary = await pipeline.RunAsync();
            await Assert.That(summary.Succeeded).IsTrue();
            foreach (var module in modules)
            {
                await Assert.That(module.DisposeCount).IsEqualTo(0);
            }
        }
        finally
        {
            await pipeline.DisposeAsync();
        }

        await pipeline.DisposeAsync();
        foreach (var module in modules)
        {
            await Assert.That(module.DisposeCount).IsEqualTo(1);
        }
    }

    [Test]
    [Arguments("type", false)]
    [Arguments("type", true)]
    [Arguments("factory", false)]
    [Arguments("factory", true)]
    [Arguments("instance", false)]
    [Arguments("instance", true)]
    public async Task Unexecuted_Modules_Dispose_Once_With_Pipeline(string registration, bool showProgress)
    {
        var pipeline = await CreateBuilder(registration, showProgress).BuildAsync();
        var modules = pipeline.Services.GetServices<IModule>().OfType<ResourceModule>().ToArray();

        await pipeline.DisposeAsync();
        await pipeline.DisposeAsync();

        foreach (var module in modules)
        {
            await Assert.That(module.DisposeCount).IsEqualTo(1);
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Skipped_And_Failed_Modules_Dispose_Once_With_Pipeline(bool showProgress)
    {
        var builder = TestPipelineBuilder.Create(new TestHostSettings { ShowProgressInConsole = showProgress });
        builder.AddModule<SkippedModule>().AddModule<FailingModule>();
        var pipeline = await builder.BuildAsync();
        var modules = pipeline.Services.GetServices<IModule>().OfType<ResourceModule>().ToArray();
        try
        {
            await Assert.ThrowsAsync<ModuleFailedException>(() => pipeline.RunAsync());
            foreach (var module in modules)
            {
                await Assert.That(module.DisposeCount).IsEqualTo(0);
            }
        }
        finally
        {
            await pipeline.DisposeAsync();
        }

        foreach (var module in modules)
        {
            await Assert.That(module.DisposeCount).IsEqualTo(1);
        }
    }

    private static PipelineBuilder CreateBuilder(string registration, bool showProgress)
    {
        var builder = TestPipelineBuilder.Create(new TestHostSettings { ShowProgressInConsole = showProgress });
        switch (registration)
        {
            case "type":
                builder.AddModule<SyncResourceModule>().AddModule<AsyncResourceModule>();
                break;
            case "factory":
                builder.AddModule(_ => new SyncResourceModule()).AddModule(_ => new AsyncResourceModule());
                break;
            case "instance":
                var sync = new SyncResourceModule();
                var async = new AsyncResourceModule();
                builder.AddModule(sync).AddModule(sync).AddModule(async).AddModule(async);
                break;
            case "runtime":
                builder.AddModules(typeof(SyncResourceModule), typeof(AsyncResourceModule));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(registration));
        }

        return builder;
    }

    private abstract class ResourceModule : Module<int>
    {
        public int DisposeCount { get; private set; }

        public void ReleaseResource()
        {
            if (++DisposeCount != 1)
            {
                throw new InvalidOperationException("Module disposed more than once");
            }
        }

        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(42);
    }

    private sealed class SyncResourceModule : ResourceModule, IDisposable
    {
        public void Dispose() => ReleaseResource();
    }

    private sealed class AsyncResourceModule : ResourceModule, IAsyncDisposable, IDisposable
    {
        public async ValueTask DisposeAsync()
        {
            await Task.Yield();
            ReleaseResource();
        }

        public void Dispose() => throw new InvalidOperationException("Async disposal should take precedence");
    }

    [DependsOn<SyncResourceModule>]
    [DependsOn<AsyncResourceModule>]
    private sealed class DependentModule : Module<int>
    {
        protected internal override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var sync = context.GetModule<SyncResourceModule>();
            var async = context.GetModule<AsyncResourceModule>();
            await Assert.That(sync.DisposeCount).IsEqualTo(0);
            await Assert.That(async.DisposeCount).IsEqualTo(0);
            return (await sync).Value + (await async).Value;
        }
    }

    private sealed class SkippedModule : ResourceModule, IDisposable
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithSkipWhen(_ => true);

        public void Dispose() => ReleaseResource();
    }

    private sealed class FailingModule : ResourceModule, IDisposable
    {
        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromException<int>(new InvalidOperationException("Module execution failed"));

        public void Dispose() => ReleaseResource();
    }
}
