using ModularPipelines.Reporting;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Models;
using Moq;

namespace ModularPipelines.Git.UnitTests;

public class GitRunReportEnricherTests
{
    [Test]
    public async Task PopulatesRepositoryCorrelationMetadata()
    {
        var information = new Mock<IGitInformation>();
        information.Setup(x => x.GetInfoAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GitRepositoryInfo(new FolderPath("repository"))
            {
                LastCommitSha = "abc123",
                BranchName = "feature/reporting",
            });
        var context = new RunReportEnrichmentContext(
            "run-id",
            "pipeline-id",
            "host",
            BuildSystem.Unknown);

        await new GitRunReportEnricher(information.Object)
            .EnrichAsync(context, CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(context.GitSha).IsEqualTo("abc123");
            await Assert.That(context.GitBranch).IsEqualTo("feature/reporting");
        }
    }

    [Test]
    public async Task GitIntegrationRegistersRunReportEnricher()
    {
        var services = new ServiceCollection();

        services.RegisterGitContext();

        await Assert.That(services.Any(descriptor =>
                descriptor.ServiceType == typeof(IRunReportEnricher)
                && descriptor.ImplementationType == typeof(GitRunReportEnricher)))
            .IsTrue();
    }
}
