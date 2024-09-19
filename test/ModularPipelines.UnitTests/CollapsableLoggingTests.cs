using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Azure.Pipelines;
using ModularPipelines.GitHub;
using ModularPipelines.TeamCity;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class CollapsableLoggingTests : TestBase
{
    [Test]
    public async Task AzurePipelines()
    {
        var stringBuilder = new StringBuilder();

        var azurePipelines = await GetService<IAzurePipeline>((_, collection) =>
        {
            collection.AddSingleton<IConsoleWriter>(new StringBuilderConsoleWriter(stringBuilder));
        });

        azurePipelines.T.WriteConsoleLogGroup("MyGroup", "Foo bar!");

        await azurePipelines.Host.DisposeAsync();
        await Assert.That(stringBuilder.ToString().Trim()).
            IsEqualTo("""
                       ----------CollapsableLoggingTests Start----------
                       ##[group]MyGroup
                       Foo bar!
                       ##[endgroup]
                       -----------CollapsableLoggingTests End-----------
                       """);
    }

    [Test]
    public async Task GitHub()
    {
        var stringBuilder = new StringBuilder();

        var gitHub = await GetService<IGitHub>((_, collection) =>
        {
            collection.AddSingleton<IConsoleWriter>(new StringBuilderConsoleWriter(stringBuilder));
        });

        gitHub.T.WriteConsoleLogGroup("MyGroup", "Foo bar!");

        await gitHub.Host.DisposeAsync();
        await Assert.That(stringBuilder.ToString().Trim()).
            IsEqualTo("""
                       ----------CollapsableLoggingTests Start----------
                       ::group::MyGroup
                       Foo bar!
                       ::endgroup::
                       -----------CollapsableLoggingTests End-----------
                       """);
    }

    [Test]
    public async Task TeamCity()
    {
        var stringBuilder = new StringBuilder();

        var teamCity = await GetService<ITeamCity>((_, collection) =>
        {
            collection.AddSingleton<IConsoleWriter>(new StringBuilderConsoleWriter(stringBuilder));
        });

        teamCity.T.WriteConsoleLogGroup("MyGroup", "Foo bar!");

        await teamCity.Host.DisposeAsync();
        await Assert.That(stringBuilder.ToString().Trim()).
            IsEqualTo("""
                       ----------CollapsableLoggingTests Start----------
                       ##teamcity[blockOpened name='MyGroup']
                       Foo bar!
                       ##teamcity[blockClosed name='MyGroup']
                       -----------CollapsableLoggingTests End-----------
                       """);
    }
}