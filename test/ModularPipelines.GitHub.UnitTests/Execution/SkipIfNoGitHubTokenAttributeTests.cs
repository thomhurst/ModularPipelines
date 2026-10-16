using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.GitHub.UnitTests.Execution;

public class SkipIfNoGitHubTokenAttributeTests : TestBase
{
    [Test]
    [Arguments("configured-token", null, false)]
    [Arguments("configured-token", "", false)]
    [Arguments("configured-token", "environment-token", false)]
    [Arguments(null, "environment-token", false)]
    [Arguments("", "environment-token", false)]
    [Arguments("  ", "environment-token", false)]
    [Arguments(null, null, true)]
    [Arguments("", "", true)]
    [Arguments("  ", "  ", true)]
    public async Task Skips_Only_When_No_Usable_Token_Exists(
        string? configuredToken, string? environmentToken, bool shouldSkip)
    {
        var environment = new Mock<IEnvironmentVariablesContext>();
        environment.Setup(x => x.Get("GITHUB_TOKEN", EnvironmentVariableTarget.Process)).Returns(environmentToken);
        var result = await GetService<IPipelineContext>(services =>
        {
            services.Configure<GitHubOptions>(options => options.AccessToken = configuredToken);
            services.AddSingleton(environment.Object);
        });

        await using var pipeline = result.Pipeline;
        var shouldSkipResult = await new SkipIfNoGitHubTokenAttribute().EvaluateAsync(result.T, default);

        await Assert.That(shouldSkipResult).IsEqualTo(shouldSkip);
    }
}
