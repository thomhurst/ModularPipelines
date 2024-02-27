using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Enums;
using ModularPipelines.Interfaces;
using ModularPipelines.TestHelpers;
using Moq;
using TUnit.Assertions.Extensions;

namespace ModularPipelines.UnitTests;

public class SmartCollapsableLoggingInternalTests : TestBase
{
    [Test]
    public async Task AzurePipelines()
    {
        var stringBuilder = await Execute(BuildSystem.AzurePipelines);
        await Assert.That(stringBuilder.ToString().Trim()).
            Is.EqualTo("""
                       ##[group]MyGroup
                       Foo bar!
                       ##[endgroup]
                       """);
    }

    [Test]
    public async Task GitHub()
    {
        var stringBuilder = await Execute(BuildSystem.GitHubActions);
        await Assert.That(stringBuilder.ToString().Trim()).
            Is.EqualTo("""
                       ::group::MyGroup
                       Foo bar!
                       ::endgroup::
                       """);
    }

    [Test]
    public async Task TeamCity()
    {
        var stringBuilder = await Execute(BuildSystem.TeamCity);
        await Assert.That(stringBuilder.ToString().Trim()).
            Is.EqualTo("""
                       ##teamcity[blockOpened name='MyGroup']
                       Foo bar!
                       ##teamcity[blockClosed name='MyGroup']
                       """);
    }

    [DataDrivenTest]
    [Arguments(BuildSystem.Jenkins)]
    [Arguments(BuildSystem.GitLab)]
    [Arguments(BuildSystem.Bitbucket)]
    [Arguments(BuildSystem.TravisCI)]
    [Arguments(BuildSystem.AppVeyor)]
    [Arguments(BuildSystem.Unknown)]
    [Arguments(-1)]
    public async Task UnsupportedLogGroupSystems(BuildSystem buildSystem)
    {
        var stringBuilder = await Execute(buildSystem);
        await Assert.That(stringBuilder.ToString().Trim()).
            Is.EqualTo("""
                       ----------MyGroup Start----------
                       Foo bar!
                       -----------MyGroup End-----------
                       """);
    }

    private async Task<StringBuilder> Execute(BuildSystem buildSystem)
    {
        var stringBuilder = new StringBuilder();

        var buildSystemDetectorMock = new Mock<IBuildSystemDetector>();

        buildSystemDetectorMock.Setup(x => x.GetCurrentBuildSystem())
            .Returns(buildSystem);

        var azurePipelines = await GetService<IInternalCollapsableLogging>((_, collection) =>
        {
            collection.AddSingleton(buildSystemDetectorMock.Object);
            collection.AddSingleton<IConsoleWriter>(new StringBuilderConsoleWriter(stringBuilder));
        });

        azurePipelines.T.WriteConsoleLogGroupInternal("MyGroup", "Foo bar!");

        await azurePipelines.Host.DisposeAsync();

        return stringBuilder;
    }
}