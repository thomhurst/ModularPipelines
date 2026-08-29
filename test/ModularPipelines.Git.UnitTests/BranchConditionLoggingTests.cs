using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains;
using ModularPipelines.Git;
using ModularPipelines.Git.Attributes;
using ModularPipelines.Git.Models;
using ModularPipelines.Logging;
using Moq;

namespace ModularPipelines.Git.UnitTests;

public class BranchConditionLoggingTests
{
    [Test]
    public async Task Repeatable_Branch_Conditions_Share_An_Alternative_Group()
    {
        IGroupedConditionAttribute exactBranch = new RunIfBranchAttribute("main");
        IGroupedConditionAttribute branchPrefix = new RunIfBranchStartsWithAttribute("release/");

        using (Assert.Multiple())
        {
            await Assert.That(exactBranch.Logic).IsEqualTo(ConditionLogic.Any);
            await Assert.That(branchPrefix.Logic).IsEqualTo(ConditionLogic.Any);
            await Assert.That(exactBranch).IsAssignableTo<RunIfAnyAttribute>();
            await Assert.That(branchPrefix).IsAssignableTo<RunIfAnyAttribute>();
            await Assert.That(exactBranch.ConditionGroupType).IsEqualTo(branchPrefix.ConditionGroupType);
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
        var services = new Mock<IServicesContext>();
        services.Setup(x => x.GetRequiredService<IGit>()).Returns(git);
        var context = Mock.Of<IPipelineContext>(x =>
            x.Logger == logger.Object &&
            x.Services == services.Object);

        var result = await new RunIfBranchAttribute("main").EvaluateAsync(context);
        var logMessage = logger.Invocations
            .Single(x => x.Method.Name == nameof(ILogger.Log))
            .Arguments[2]
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
        var services = new Mock<IServicesContext>();
        services.Setup(x => x.GetRequiredService<IGit>()).Returns(git);
        var logger = Mock.Of<ILogger>();
        var context = Mock.Of<IPipelineContext>(x =>
            x.Logger == logger &&
            x.Services == services.Object);
        using var cancellationTokenSource = new CancellationTokenSource();

        await new RunIfBranchAttribute("main")
            .EvaluateAsync(context, cancellationTokenSource.Token);

        gitInformation.Verify(
            x => x.GetInfoAsync(cancellationTokenSource.Token),
            Times.Once);
    }
}
