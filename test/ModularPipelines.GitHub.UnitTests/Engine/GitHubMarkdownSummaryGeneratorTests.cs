using ModularPipelines.Enums;
using ModularPipelines.Models;
using Moq;

namespace ModularPipelines.GitHub.UnitTests.Engine;

public class GitHubMarkdownSummaryGeneratorTests
{
    [Test]
    [Arguments(ModuleStatus.Failed, false, true)]
    [Arguments(ModuleStatus.TimedOut, false, true)]
    [Arguments(ModuleStatus.DependencyFailed, false, true)]
    [Arguments(ModuleStatus.Canceled, false, true)]
    [Arguments(ModuleStatus.Failed, true, true)]
    [Arguments(ModuleStatus.FailureIgnored, true, false)]
    [Arguments(ModuleStatus.FailureIgnored, false, false)]
    [Arguments(ModuleStatus.Succeeded, false, false)]
    [Arguments(ModuleStatus.Skipped, false, false)]
    [Arguments(ModuleStatus.RestoredFromCache, false, false)]
    public async Task MermaidMarksOnlyUnignoredFailuresCritical(ModuleStatus status, bool hasException, bool expectedCritical)
    {
        var start = DateTimeOffset.UtcNow;
        var duration = TimeSpan.FromSeconds(1);
        var result = new Mock<IModuleResult>();
        result.SetupGet(x => x.Name).Returns("TestModule");
        result.SetupGet(x => x.Status).Returns(status);
        result.SetupGet(x => x.ExceptionOrDefault).Returns(hasException ? new InvalidOperationException("failure") : null);
        result.SetupGet(x => x.StartTime).Returns(start);
        result.SetupGet(x => x.EndTime).Returns(start + duration);
        result.SetupGet(x => x.Duration).Returns(duration);
        var summary = new PipelineSummary([], [result.Object], duration, start, start + duration);

        var mermaid = GitHubMarkdownSummaryGenerator.GenerateMermaidSummary(summary);

        await Assert.That(mermaid).Contains("TestModule :");
        await Assert.That(mermaid.Contains("TestModule :crit,", StringComparison.Ordinal)).IsEqualTo(expectedCritical);
    }

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
