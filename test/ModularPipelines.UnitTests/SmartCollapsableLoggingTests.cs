using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Enums;
using ModularPipelines.Interfaces;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests;

public class SmartCollapsableLoggingTests : TestBase
{
    [Test]
    public async Task AzurePipelines()
    {
        var stringBuilder = await Execute(BuildSystem.AzurePipelines);
        await Assert.That(stringBuilder.ToString().Trim()).
            IsEqualTo("""
                       ##[group]SmartCollapsableLoggingTests
                       ##[group]MyGroup
                       Foo bar!
                       ##[endgroup]
                       ##[endgroup]
                       """);
    }

    [Test]
    public async Task GitHub()
    {
        var stringBuilder = await Execute(BuildSystem.GitHubActions);
        await Assert.That(stringBuilder.ToString().Trim()).
            IsEqualTo("""
                       ::group::SmartCollapsableLoggingTests
                       ::group::MyGroup
                       Foo bar!
                       ::endgroup::
                       ::endgroup::
                       """);
    }

    [Test]
    public async Task TeamCity()
    {
        var stringBuilder = await Execute(BuildSystem.TeamCity);
        await Assert.That(stringBuilder.ToString().Trim()).
            IsEqualTo("""
                       ##teamcity[blockOpened name='SmartCollapsableLoggingTests']
                       ##teamcity[blockOpened name='MyGroup']
                       Foo bar!
                       ##teamcity[blockClosed name='MyGroup']
                       ##teamcity[blockClosed name='SmartCollapsableLoggingTests']
                       """);
    }

    [Test]
    [Arguments(BuildSystem.Jenkins)]
    [Arguments(BuildSystem.Bitbucket)]
    [Arguments(BuildSystem.AppVeyor)]
    public async Task UnsupportedLogGroupSystems(BuildSystem buildSystem)
    {
        var stringBuilder = await Execute(buildSystem);
        // These systems don't support collapsible sections, so only content is logged
        await Assert.That(stringBuilder.ToString().Trim()).IsEqualTo("Foo bar!");
    }

    [Test]
    [Arguments(BuildSystem.Unknown)]
    [Arguments(-1)]
    public async Task UnknownBuildSystem_UsesDefaultFormatter(BuildSystem buildSystem)
    {
        var stringBuilder = await Execute(buildSystem);
        // Unknown systems use PlayIcon from MarkupFormatter
        await Assert.That(stringBuilder.ToString().Trim()).IsEqualTo("""
                                                                      [bold cyan]▶[/] SmartCollapsableLoggingTests
                                                                      [bold cyan]▶[/] MyGroup
                                                                      Foo bar!
                                                                      """);
    }

    [Test]
    public async Task GitLab()
    {
        var stringBuilder = await Execute(BuildSystem.GitLab);
        var output = stringBuilder.ToString().Trim();
        // GitLab format includes timestamps, so we check for key parts
        await Assert.That(output).Contains("section_start:");
        await Assert.That(output).Contains(":smartcollapsableloggingtests");
        await Assert.That(output).Contains("SmartCollapsableLoggingTests");
        await Assert.That(output).Contains(":mygroup");
        await Assert.That(output).Contains("MyGroup");
        await Assert.That(output).Contains("Foo bar!");
        await Assert.That(output).Contains("section_end:");
    }

    [Test]
    public async Task TravisCI()
    {
        var stringBuilder = await Execute(BuildSystem.TravisCI);
        var output = stringBuilder.ToString().Trim();
        // TravisCI format with ANSI codes
        await Assert.That(output).Contains("travis_fold:start:smartcollapsableloggingtests");
        await Assert.That(output).Contains("SmartCollapsableLoggingTests");
        await Assert.That(output).Contains("travis_fold:start:mygroup");
        await Assert.That(output).Contains("MyGroup");
        await Assert.That(output).Contains("Foo bar!");
        await Assert.That(output).Contains("travis_fold:end:mygroup");
        await Assert.That(output).Contains("travis_fold:end:smartcollapsableloggingtests");
    }

    private async Task<StringBuilder> Execute(BuildSystem buildSystem)
    {
        var stringBuilder = new StringBuilder();

        var buildSystemDetectorMock = new Mock<IBuildSystemDetector>();

        buildSystemDetectorMock.Setup(x => x.GetCurrentBuildSystem())
            .Returns(buildSystem);

        var azurePipelines = await GetService<ICollapsableLogging>((_, collection) =>
        {
            collection.AddSingleton(buildSystemDetectorMock.Object);
            collection.AddSingleton<IConsoleWriter>(new StringBuilderConsoleWriter(stringBuilder));
        });

        azurePipelines.T.WriteConsoleLogGroup("MyGroup", "Foo bar!");

        await azurePipelines.Host.DisposeAsync();

        return stringBuilder;
    }
}