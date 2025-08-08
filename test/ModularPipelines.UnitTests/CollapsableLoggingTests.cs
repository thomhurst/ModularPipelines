using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Azure.Pipelines;
using ModularPipelines.Enums;
using ModularPipelines.GitHub;
using ModularPipelines.TeamCity;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class CollapsableLoggingTests : TestBase
{
    private class AzurePipelinesBuildSystemDetector : IBuildSystemDetector
    {
        public BuildSystem GetCurrentBuildSystem() => BuildSystem.AzurePipelines;
    }

    private class GitHubActionsBuildSystemDetector : IBuildSystemDetector
    {
        public BuildSystem GetCurrentBuildSystem() => BuildSystem.GitHubActions;
    }

    private class TeamCityBuildSystemDetector : IBuildSystemDetector
    {
        public BuildSystem GetCurrentBuildSystem() => BuildSystem.TeamCity;
    }

    [Test]
    public async Task AzurePipelines()
    {
        var stringBuilder = new StringBuilder();

        var azurePipelines = await GetService<IAzurePipeline>((_, collection) =>
        {
            collection.AddSingleton<IConsoleWriter>(new StringBuilderConsoleWriter(stringBuilder));
            collection.AddSingleton<IBuildSystemDetector>(new AzurePipelinesBuildSystemDetector());
        });

        azurePipelines.T.WriteConsoleLogGroup("MyGroup", "Foo bar!");

        await azurePipelines.Host.DisposeAsync();
        await Assert.That(stringBuilder.ToString().Trim()).
            IsEqualTo("""
                       ##[group]CollapsableLoggingTests
                       ##[group]MyGroup
                       Foo bar!
                       ##[endgroup]
                       ##[endgroup]
                       """);
    }

    [Test]
    public async Task GitHub()
    {
        var stringBuilder = new StringBuilder();

        var gitHub = await GetService<IGitHub>((_, collection) =>
        {
            collection.AddSingleton<IConsoleWriter>(new StringBuilderConsoleWriter(stringBuilder));
            collection.AddSingleton<IBuildSystemDetector>(new GitHubActionsBuildSystemDetector());
        });

        gitHub.T.WriteConsoleLogGroup("MyGroup", "Foo bar!");

        await gitHub.Host.DisposeAsync();
        await Assert.That(stringBuilder.ToString().Trim()).
            IsEqualTo("""
                       ::group::CollapsableLoggingTests
                       ::group::MyGroup
                       Foo bar!
                       ::endgroup::
                       ::endgroup::
                       """);
    }

    [Test]
    public async Task TeamCity()
    {
        var stringBuilder = new StringBuilder();

        var teamCity = await GetService<ITeamCity>((_, collection) =>
        {
            collection.AddSingleton<IConsoleWriter>(new StringBuilderConsoleWriter(stringBuilder));
            collection.AddSingleton<IBuildSystemDetector>(new TeamCityBuildSystemDetector());
        });

        teamCity.T.WriteConsoleLogGroup("MyGroup", "Foo bar!");

        await teamCity.Host.DisposeAsync();
        await Assert.That(stringBuilder.ToString().Trim()).
            IsEqualTo("""
                       ##teamcity[blockOpened name='CollapsableLoggingTests']
                       ##teamcity[blockOpened name='MyGroup']
                       Foo bar!
                       ##teamcity[blockClosed name='MyGroup']
                       ##teamcity[blockClosed name='CollapsableLoggingTests']
                       """);
    }
}