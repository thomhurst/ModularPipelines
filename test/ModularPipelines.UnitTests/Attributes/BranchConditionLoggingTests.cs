using Microsoft.Extensions.Logging;
using Moq;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains;
using ModularPipelines.Git;
using ModularPipelines.Git.Attributes;
using ModularPipelines.Logging;

namespace ModularPipelines.UnitTests.Attributes;

public class BranchConditionLoggingTests
{
    [Test]
    public async Task RunIfBranch_UsesDetachedPlaceholderForEmptyBranch()
    {
        var logger = new Mock<IModuleLogger>();
        logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(true);
        var gitInformation = Mock.Of<IGitInformation>(x => x.BranchName == string.Empty);
        var git = Mock.Of<IGit>(x => x.Information == gitInformation);
        var services = new Mock<IServicesContext>();
        services.Setup(x => x.Get<IGit>()).Returns(git);
        var context = Mock.Of<IPipelineHookContext>(x =>
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
