using ModularPipelines.Enums;

namespace ModularPipelines.GitHub.UnitTests.Engine;

public class GitHubMarkdownSummaryGeneratorTests
{
    [Test]
    public async Task CachedResultUsesSuccessfulColor()
    {
        var status = GitHubMarkdownSummaryGenerator.GetStatusString(Status.CachedResult);

        using (Assert.Multiple())
        {
            await Assert.That(status).Contains("lightgreen");
            await Assert.That(status).Contains(nameof(Status.CachedResult));
        }
    }
}
