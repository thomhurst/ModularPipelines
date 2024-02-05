using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Enums;
using ModularPipelines.Interfaces;
using ModularPipelines.TestHelpers;
using Moq;
using TUnit.Assertions;
using TUnit.Core;

namespace ModularPipelines.UnitTests;

public class SmartCollapsableLoggingInternalTests : TestBase
{
    [Test]
    public async Task AzurePipelines()
    {
        var stringBuilder = await Execute(BuildSystem.AzurePipelines);

        Assert.That(stringBuilder.ToString().Trim()).
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

        Assert.That(stringBuilder.ToString().Trim()).
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

        Assert.That(stringBuilder.ToString().Trim()).
            Is.EqualTo("""
                       ##teamcity[blockOpened name='MyGroup']
                       Foo bar!
                       ##teamcity[blockClosed name='MyGroup']
                       """);
    }

    [TestWithData(BuildSystem.Jenkins)]
    [TestWithData(BuildSystem.GitLab)]
    [TestWithData(BuildSystem.Bitbucket)]
    [TestWithData(BuildSystem.TravisCI)]
    [TestWithData(BuildSystem.AppVeyor)]
    [TestWithData(BuildSystem.Unknown)]
    [TestWithData(-1)]
    public async Task UnsupportedLogGroupSystems(BuildSystem buildSystem)
    {
        var stringBuilder = await Execute(buildSystem);

        Assert.That(stringBuilder.ToString().Trim()).
            Is.EqualTo("""
                       Foo bar!
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