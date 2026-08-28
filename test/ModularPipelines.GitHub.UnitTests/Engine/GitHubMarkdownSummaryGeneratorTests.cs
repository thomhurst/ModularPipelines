using ModularPipelines.Enums;

namespace ModularPipelines.GitHub.UnitTests.Engine;

public class GitHubMarkdownSummaryGeneratorTests
{
    [Test]
    public async Task CachedResultUsesSuccessfulColor()
    {
        var status = GitHubMarkdownSummaryGenerator.GetStatusString(ModuleStatus.RestoredFromCache);

        using (Assert.Multiple())
        {
            await Assert.That(status).Contains("lightgreen");
            await Assert.That(status).Contains(nameof(ModuleStatus.RestoredFromCache));
        }
    }

    [Test]
    public async Task EveryStatusCanBeRendered()
    {
        foreach (var status in Enum.GetValues<ModuleStatus>())
        {
            var renderedStatus = GitHubMarkdownSummaryGenerator.GetStatusString(status);

            await Assert.That(renderedStatus).Contains(status.ToString());
        }
    }
}
