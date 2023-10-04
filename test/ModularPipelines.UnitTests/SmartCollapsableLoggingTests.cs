using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Azure.Pipelines;
using ModularPipelines.Enums;
using ModularPipelines.GitHub;
using ModularPipelines.Interfaces;
using ModularPipelines.TeamCity;
using Moq;

namespace ModularPipelines.UnitTests;

public class SmartCollapsableLoggingTests : TestBase
{
    [Test]
    public async Task AzurePipelines()
    {
        var stringBuilder = await Execute(BuildSystem.AzurePipelines);

        Assert.That(stringBuilder.ToString().Trim(),
            Is.EqualTo("""
                       ##[group]SmartCollapsableLogging
                       ##[group]MyGroup
                       Foo bar!
                       ##[endgroup]
                       ##[endgroup]
                       """)
        );
    }

    [Test]
    public async Task GitHub()
    {
        var stringBuilder = await Execute(BuildSystem.GitHubActions);
        
        Assert.That(stringBuilder.ToString().Trim(),
            Is.EqualTo("""
                       ::group::SmartCollapsableLogging
                       ::group::MyGroup
                       Foo bar!
                       ::endgroup::
                       ::endgroup::
                       """)
        );
    }

    [Test]
    public async Task TeamCity()
    {
        var stringBuilder = await Execute(BuildSystem.TeamCity);
        
        Assert.That(stringBuilder.ToString().Trim(),
            Is.EqualTo("""
                       ##teamcity[blockOpened name='SmartCollapsableLogging']
                       ##teamcity[blockOpened name='MyGroup']
                       Foo bar!
                       ##teamcity[blockClosed name='MyGroup']
                       ##teamcity[blockClosed name='SmartCollapsableLogging']
                       """)
        );
    }
    
    [TestCase(BuildSystem.Jenkins)]
    [TestCase(BuildSystem.GitLab)]
    [TestCase(BuildSystem.Bitbucket)]
    [TestCase(BuildSystem.TravisCI)]
    [TestCase(BuildSystem.AppVeyor)]
    [TestCase(BuildSystem.Unknown)]
    [TestCase(-1)]
    public async Task UnsupportedLogGroupSystems(BuildSystem buildSystem)
    {
        var stringBuilder = await Execute(buildSystem);
        
        Assert.That(stringBuilder.ToString().Trim(),
            Is.EqualTo("""
                       Foo bar!
                       """)
        );
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