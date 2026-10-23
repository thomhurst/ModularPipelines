using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines;
using ModularPipelines.Context;
using ModularPipelines.Git;
using Moq;

namespace ModularPipelines.Git.UnitTests;

public class BranchConditionLoggingTests
{
    [Test]
    public async Task Repeatable_Branch_Conditions_Share_An_Alternative_Group()
    {
        RunConditionAttribute exactBranch = new RunIfBranchAttribute("main");
        RunConditionAttribute branchPrefix = new RunIfBranchStartsWithAttribute("release/");

        using (Assert.Multiple())
        {
            await Assert.That(exactBranch.Intent).IsEqualTo(ConditionIntent.Run);
            await Assert.That(branchPrefix.Intent).IsEqualTo(ConditionIntent.Run);
            await Assert.That(exactBranch.GroupKey).IsNotNull();
            await Assert.That(exactBranch.GroupKey).IsEqualTo(branchPrefix.GroupKey);
        }
    }

    [Test]
    public async Task RunOnly_Branch_Conditions_Are_Separate_Requirements()
    {
        RunConditionAttribute exactBranch = new RunOnlyOnBranchAttribute("main");
        RunConditionAttribute branchPrefix = new RunOnlyIfBranchStartsWithAttribute("release/");
        RunConditionAttribute skipBranch = new SkipIfBranchAttribute("main");

        using (Assert.Multiple())
        {
            await Assert.That(exactBranch.Intent).IsEqualTo(ConditionIntent.Run);
            await Assert.That(branchPrefix.Intent).IsEqualTo(ConditionIntent.Run);
            await Assert.That(exactBranch.GroupKey).IsNull();
            await Assert.That(branchPrefix.GroupKey).IsNull();
            await Assert.That(skipBranch.Intent).IsEqualTo(ConditionIntent.Skip);
        }
    }

    [Test]
    [Arguments("en-US", "feature/topic", "feature/", true)]
    [Arguments("tr-TR", "feature/topic", "FEATURE/", false)]
    [Arguments("en-US", "feature/topic", "feature\0/", false)]
    [Arguments("en-US", "\u00e9/topic", "e\u0301/", false)]
    [Arguments("en-US", null, "feature/", false)]
    public async Task Branch_Prefix_Conditions_Use_Ordinal_Comparison(
        string culture, string? branchName, string prefix, bool expected)
    {
        var originalCulture = System.Globalization.CultureInfo.CurrentCulture;
        try
        {
            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo(culture);
            var gitInformation = new Mock<IGitInformation>();
            gitInformation.Setup(x => x.GetInfoAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GitRepositoryInfo(
                    new ModularPipelines.FileSystem.FolderPath(TestContext.WorkingDirectory))
                {
                    BranchName = branchName,
                });
            var git = Mock.Of<IGit>(x => x.Information == gitInformation.Object);
            var tools = Mock.Of<IToolsContext>(x => x.Get<IGit>() == git);
            var logger = Mock.Of<ILogger>();
            var context = Mock.Of<IPipelineContext>(x => x.Logger == logger && x.Tools == tools);

            using (Assert.Multiple())
            {
                await Assert.That(await new RunIfBranchStartsWithAttribute(prefix)
                    .EvaluateAsync(context, CancellationToken.None)).IsEqualTo(expected);
                await Assert.That(await new RunOnlyIfBranchStartsWithAttribute(prefix)
                    .EvaluateAsync(context, CancellationToken.None)).IsEqualTo(expected);
            }
        }
        finally
        {
            System.Globalization.CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Branch_Conditions_UseDetachedPlaceholderForEmptyBranch(bool skip)
    {
        var logger = new Mock<ILogger>();
        logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(true);
        var gitInformation = new Mock<IGitInformation>();
        gitInformation.Setup(x => x.GetInfoAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GitRepositoryInfo(
                new ModularPipelines.FileSystem.FolderPath(TestContext.WorkingDirectory)));
        var git = Mock.Of<IGit>(x => x.Information == gitInformation.Object);
        var tools = Mock.Of<IToolsContext>(x => x.Get<IGit>() == git);
        var context = Mock.Of<IPipelineContext>(x =>
            x.Logger == logger.Object &&
            x.Tools == tools);

        RunConditionAttribute condition = skip ? new SkipIfBranchAttribute("main") : new RunIfBranchAttribute("main");
        var result = await condition.EvaluateAsync(context, CancellationToken.None);
        var logMessage = logger.Invocations
            .Single(x => x.Method.Name == nameof(ILogger.Log))
            .Arguments[2]?
            .ToString();

        await Assert.That(result).IsFalse();
        await Assert.That(logMessage).IsEqualTo(skip
            ? "Current Branch: (detached) | Will skip on: main"
            : "Current Branch: (detached) | Can run on: main");
    }

    [Test]
    public async Task RunIfBranch_Propagates_Cancellation_Token()
    {
        var gitInformation = new Mock<IGitInformation>();
        gitInformation.Setup(x => x.GetInfoAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GitRepositoryInfo(
                new ModularPipelines.FileSystem.FolderPath(TestContext.WorkingDirectory)));
        var git = Mock.Of<IGit>(x => x.Information == gitInformation.Object);
        var tools = Mock.Of<IToolsContext>(x => x.Get<IGit>() == git);
        var logger = Mock.Of<ILogger>();
        var context = Mock.Of<IPipelineContext>(x =>
            x.Logger == logger &&
            x.Tools == tools);
        using var cancellationTokenSource = new CancellationTokenSource();

        await new RunIfBranchAttribute("main")
            .EvaluateAsync(context, cancellationTokenSource.Token);

        gitInformation.Verify(
            x => x.GetInfoAsync(cancellationTokenSource.Token),
            Times.Once);
    }
}
