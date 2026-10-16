using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.TeamCity.UnitTests;

[TUnit.Core.NotInParallel("ProcessEnvironment")]
public class TeamCityEnvironmentVariablesTests
{
    [Test]
    [Arguments(null)]
    [Arguments("https://teamcity.example/viewLog.html?buildId=42")]
    public async Task BuildUrlReadsDocumentedTeamCityVariable(string? value)
    {
        var previousValue = Environment.GetEnvironmentVariable("BUILD_URL");

        try
        {
            Environment.SetEnvironmentVariable("BUILD_URL", value);
            var services = new ServiceCollection();
            services.RegisterTeamCityContext();
            using var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var variables = scope.ServiceProvider.GetRequiredService<ITeamCityEnvironmentVariables>();

            await Assert.That(variables.BuildUrl).IsEqualTo(value);
        }
        finally
        {
            Environment.SetEnvironmentVariable("BUILD_URL", previousValue);
        }
    }
}
