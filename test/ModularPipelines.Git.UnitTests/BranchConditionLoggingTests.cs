using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines;
using ModularPipelines.Context;
using ModularPipelines.Git;
using ModularPipelines.Git.Attributes;
using ModularPipelines.Git.Models;
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
    public async Task RunIfBranch_UsesDetachedPlaceholderForEmptyBranch()
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

        var result = await new RunIfBranchAttribute("main").EvaluateAsync(context, CancellationToken.None);
        var logMessage = logger.Invocations
            .Single(x => x.Method.Name == nameof(ILogger.Log))
            .Arguments[2]?
            .ToString();

        await Assert.That(result).IsFalse();
        await Assert.That(logMessage).IsEqualTo("Current Branch: (detached) | Can run on: main");
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
