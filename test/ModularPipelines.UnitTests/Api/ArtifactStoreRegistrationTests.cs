using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.TestHelpers;
using static ModularPipelines.UnitTests.Api.ArtifactContextApiTests;

namespace ModularPipelines.UnitTests.Api;

public class ArtifactStoreRegistrationTests
{
    [Test]
    public async Task Factory_Rejects_Earlier_Direct_Store()
    {
        var builder = Pipeline.CreateBuilder().AddDistributedArtifactStore<TestArtifactStore>();
        var registrations = builder.Services.ToArray();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            builder.AddDistributedArtifactStoreFactory<TestArtifactStoreFactory>());

        await Assert.That(exception.Message).Contains("artifact store backend");
        await Assert.That(builder.Services.SequenceEqual(registrations)).IsTrue();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Different_Backend_Is_Rejected(bool factory)
    {
        var builder = Pipeline.CreateBuilder();
        if (factory)
        {
            builder.AddDistributedArtifactStoreFactory<TestArtifactStoreFactory>();
        }
        else
        {
            builder.AddDistributedArtifactStore<TestArtifactStore>();
        }

        var registrations = builder.Services.ToArray();
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            if (factory)
            {
                builder.AddDistributedArtifactStoreFactory<TestDisposableArtifactStoreFactory>();
            }
            else
            {
                builder.AddDistributedArtifactStore<TestDisposableArtifactStore>();
            }
        });

        await Assert.That(exception.Message).Contains("artifact store backend");
        await Assert.That(builder.Services.SequenceEqual(registrations)).IsTrue();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Repeated_Backend_Is_A_NoOp(bool factory)
    {
        var builder = Pipeline.CreateBuilder();
        void Register()
        {
            if (factory)
            {
                builder.AddDistributedArtifactStoreFactory<TestArtifactStoreFactory>();
            }
            else
            {
                builder.AddDistributedArtifactStore<TestArtifactStore>();
            }
        }

        Register();
        var registrations = builder.Services.ToArray();
        Register();

        await Assert.That(builder.Services.SequenceEqual(registrations)).IsTrue();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Keyed_And_Unrelated_Registrations_Are_Preserved(bool factory)
    {
        var builder = Pipeline.CreateBuilder();
        builder.Services.AddKeyedSingleton<IDistributedArtifactStore, TestDisposableArtifactStore>("other");
        builder.Services.AddKeyedSingleton<IDistributedArtifactStoreFactory, TestDisposableArtifactStoreFactory>("other");
        builder.Services.AddSingleton(new object());
        var registrations = builder.Services.ToArray();

        if (factory)
        {
            builder.AddDistributedArtifactStoreFactory<TestArtifactStoreFactory>();
        }
        else
        {
            builder.AddDistributedArtifactStore<TestArtifactStore>();
        }

        await Assert.That(builder.Services.Take(registrations.Length).SequenceEqual(registrations)).IsTrue();
    }

    [Test]
    public async Task Earlier_Conflicting_Registration_Is_Not_Hidden_By_A_Duplicate()
    {
        var builder = Pipeline.CreateBuilder();
        builder.Services.AddSingleton<IDistributedArtifactStore, TestDisposableArtifactStore>();
        builder.Services.AddSingleton<IDistributedArtifactStore, TestArtifactStore>();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            builder.AddDistributedArtifactStore<TestArtifactStore>());

        await Assert.That(exception.Message).Contains(typeof(TestDisposableArtifactStore).FullName!);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Existing_Delegate_Or_Instance_Is_Rejected(bool instance)
    {
        var builder = Pipeline.CreateBuilder();
        if (instance)
        {
            builder.Services.AddSingleton<IDistributedArtifactStore>(new TestArtifactStore());
        }
        else
        {
            builder.Services.AddSingleton<IDistributedArtifactStore>(_ => new TestArtifactStore());
        }

        var exception = Assert.Throws<InvalidOperationException>(() =>
            builder.AddDistributedArtifactStoreFactory<TestArtifactStoreFactory>());

        await Assert.That(exception.Message).Contains("artifact store backend");
    }

    [Test]
    public async Task Default_Store_Remains_Available_Without_Explicit_Registration()
    {
        await using var pipeline = await TestPipelineBuilder.Create().AddModule<EmptyModule>().BuildAsync();

        await Assert.That(pipeline.Services.GetRequiredService<IDistributedArtifactStore>())
            .IsTypeOf<FileSystemDistributedArtifactStore>();
    }

    private sealed class EmptyModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult(string.Empty);
    }
}
