using ModularPipelines.Reporting;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.GitHub.Extensions;
using Moq;

namespace ModularPipelines.GitHub.UnitTests;

public class GitHubRunReportEnricherTests
{
    [Test]
    public async Task PopulatesGitHubCorrelationMetadata()
    {
        var environment = new Mock<IGitHubEnvironmentVariables>();
        environment.SetupGet(x => x.Sha).Returns("github-sha");
        environment.SetupGet(x => x.HeadRef).Returns("feature/github");
        environment.SetupGet(x => x.ServerUrl).Returns("https://github.com/");
        environment.SetupGet(x => x.Repository).Returns("owner/repository");
        environment.SetupGet(x => x.RunId).Returns("12345");
        var services = new ServiceCollection();
        services.AddScoped(_ => environment.Object);
        using var provider = services.BuildServiceProvider();
        var context = new RunReportEnrichmentContext(
            "run-id",
            "pipeline-id",
            "host",
            BuildSystem.GitHubActions);

        await new GitHubRunReportEnricher(provider.GetRequiredService<IServiceScopeFactory>())
            .EnrichAsync(context, CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(context.GitSha).IsEqualTo("github-sha");
            await Assert.That(context.GitBranch).IsEqualTo("feature/github");
            await Assert.That(context.CiRunUrl)
                .IsEqualTo("https://github.com/owner/repository/actions/runs/12345");
        }
    }

    [Test]
    public async Task GitHubIntegrationRegistersRunReportEnricher()
    {
        var services = new ServiceCollection();

        services.RegisterGitHubContext();

        await Assert.That(services.Any(descriptor =>
                descriptor.ServiceType == typeof(IRunReportEnricher)
                && descriptor.ImplementationType == typeof(GitHubRunReportEnricher)))
            .IsTrue();
    }
}
