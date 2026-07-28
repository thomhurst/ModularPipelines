using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains;
using ModularPipelines.Git;
using ModularPipelines.Git.Attributes;
using ModularPipelines.Git.Models;
using ModularPipelines.Logging;
using Moq;

namespace ModularPipelines.UnitTests.Attributes;

public class BranchConditionLoggingTests
{
    [Test]
    public async Task RunIfBranch_UsesDetachedPlaceholderForEmptyBranch()
    {
        var logger = new Mock<IModuleLogger>();
        logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(true);
        var gitInformation = new Mock<IGitInformation>();
        gitInformation.Setup(x => x.GetInfoAsync())
            .ReturnsAsync(new GitRepositoryInfo(
                new ModularPipelines.FileSystem.Folder(TestContext.WorkingDirectory)));
        var git = Mock.Of<IGit>(x => x.Information == gitInformation.Object);
        var services = new Mock<IServicesContext>();
        services.Setup(x => x.Get<IGit>()).Returns(git);
        var context = Mock.Of<IPipelineContext>(x =>
            x.Logger == logger.Object &&
            x.Services == services.Object);

        var result = await new RunIfBranchAttribute("main").Condition(context);
        var logMessage = logger.Invocations
            .Single(x => x.Method.Name == nameof(ILogger.Log))
            .Arguments[2]
            .ToString();

        await Assert.That(result).IsFalse();
        await Assert.That(logMessage).IsEqualTo("Current Branch: (detached) | Can run on: main");
    }
}
