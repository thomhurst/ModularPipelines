using System.Reflection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Helpers;
using ModularPipelines.Build.Modules;
using ModularPipelines.Build.Modules.UnitTests;
using ModularPipelines.Distributed;
using Moq;

namespace ModularPipelines.Build.UnitTests;

public class BuildOutputPublicationTests
{
    [Test]
    [Arguments(1, false, false)]
    [Arguments(1, true, false)]
    [Arguments(2, false, true)]
    [Arguments(2, true, false)]
    public async Task Redis_Producer_Runs_Only_For_Distributed_Fallback(int instances, bool useTransport, bool expected)
    {
        var sharing = new BuildOutputSharing(Microsoft.Extensions.Options.Options.Create(new DistributedOptions { TotalInstances = instances }),
            Mock.Of<IHostApplicationLifetime>(), useTransport ? new UnusedTransport() : null);
        var context = Mock.Of<IPipelineContext>(x => x.Services.GetRequiredService<BuildOutputSharing>() == sharing);
        var condition = typeof(PublishRedisBuildOutputModule).GetCustomAttributes<RunConditionAttribute>()
            .Single(attribute => attribute.ConditionNames == nameof(UsesRedisBuildOutput));

        await Assert.That(await condition.EvaluateAsync(context, CancellationToken.None)).IsEqualTo(expected);
    }

    [Test]
    public async Task Only_Redis_Fallback_Declares_The_Build_Output_Artifact()
    {
        await Assert.That(typeof(BuildSolutionsModule).GetCustomAttributes<ProducesArtifactAttribute>()).IsEmpty();
        var artifact = typeof(PublishRedisBuildOutputModule).GetCustomAttributes<ProducesArtifactAttribute>().Single();
        await Assert.That(artifact.Name).IsEqualTo("build-output");
    }

    [Test]
    public async Task Workers_Wait_For_Optional_Redis_Publication_After_Building()
    {
        var producerDependency = typeof(PublishRedisBuildOutputModule)
            .GetCustomAttribute<DependsOnAttribute<BuildSolutionsModule>>();
        var workerBuildDependency = typeof(RunUnitTestModule)
            .GetCustomAttribute<DependsOnAttribute<BuildSolutionsModule>>();
        var workerPublicationDependency = typeof(RunUnitTestModule)
            .GetCustomAttribute<DependsOnAttribute<PublishRedisBuildOutputModule>>();

        await Assert.That(producerDependency).IsNotNull();
        await Assert.That(producerDependency!.Optional).IsFalse();
        await Assert.That(workerBuildDependency).IsNotNull();
        await Assert.That(workerBuildDependency!.Optional).IsFalse();
        await Assert.That(workerPublicationDependency).IsNotNull();
        await Assert.That(workerPublicationDependency!.Optional).IsTrue();
    }

    private sealed class UnusedTransport : IBuildOutputTransport
    {
        public Task PublishAsync(string repositoryRoot, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("A run condition must not publish outputs.");

        public Task DownloadAsync(string repositoryRoot, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("A run condition must not download outputs.");
    }
}
