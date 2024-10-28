using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Enums;
using ModularPipelines.Interfaces;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests;

public class SmartCollapsableLoggingInternalTests : TestBase
{
    [Test]
    public async Task AzurePipelines()
    {
        var stringBuilder = await Execute(BuildSystem.AzurePipelines);
        await Assert.That(stringBuilder.ToString().Trim()).
            IsEqualTo("""
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
            IsEqualTo("""
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
            IsEqualTo("""
                       ##teamcity[blockOpened name='MyGroup']
                       Foo bar!
                       ##teamcity[blockClosed name='MyGroup']
                       """);
    }

    [Test]
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
            IsEqualTo("""
                       ----------MyGroup Start----------
                       Foo bar!
                       -----------MyGroup End-----------
                       """);
    }

    private async Task<StringBuilder> Execute(BuildSystem buildSystem)
    {
        var stringBuilder = new StringBuilder();

        var buildSystemDetectorMock = new Mock<IBuildSystemDetector>();

        var loggerMock = new Mock<ILogger<SmartCollapsableLogging>>();

        loggerMock.Setup(x => x.IsEnabled(LogLevel.Information))
            .Returns(true);
        
        buildSystemDetectorMock.Setup(x => x.GetCurrentBuildSystem())
            .Returns(buildSystem);

        var buildSystemLogger = await GetService<IInternalCollapsableLogging>((_, collection) =>
        {
            collection.AddSingleton(loggerMock.Object);
            collection.AddSingleton(buildSystemDetectorMock.Object);
            collection.AddSingleton<IConsoleWriter>(new StringBuilderConsoleWriter(stringBuilder));
        });

        buildSystemLogger.T.WriteConsoleLogGroupInternal("MyGroup", "Foo bar!", LogLevel.Information);

        await buildSystemLogger.Host.DisposeAsync();

        return stringBuilder;
    }
}