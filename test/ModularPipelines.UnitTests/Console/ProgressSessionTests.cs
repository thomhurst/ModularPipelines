using MEL.Spectre;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Console;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Console;

[TUnit.Core.NotInParallel]
public class ProgressSessionTests
{
    [Test]
    public async Task PauseAsync_CompletesImmediatelyWhenNoRefreshIsActive()
    {
        var outputCoordinator = new Mock<IOutputCoordinator>();
        var coordinator = CreateCoordinator(outputCoordinator.Object);
        await using var session = new ProgressSession(
            coordinator,
            new OrganizedModules([], []),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            NullLoggerFactory.Instance,
            CancellationToken.None);

        var pauseTask = session.PauseAsync();

        await Assert.That(pauseTask.IsCompleted).IsTrue();
        await session.ResumeAsync();
    }

    private static ConsoleCoordinator CreateCoordinator(IOutputCoordinator outputCoordinator)
    {
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(provider => provider.Secrets).Returns([]);
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(obfuscator => obfuscator.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()))
            .Returns((string? value, object? _) => value ?? string.Empty);
        var nonSpectreLoggerFactory = new Mock<INonSpectreLoggerFactory>();
        nonSpectreLoggerFactory
            .Setup(factory => factory.CreateLoggers(It.IsAny<string>()))
            .Returns([]);
        var spectreLoggerFilter = new Mock<ISpectreLoggerFilter>();
        spectreLoggerFilter
            .Setup(filter => filter.IsEnabled(It.IsAny<string>(), It.IsAny<LogLevel>()))
            .Returns(true);

        return new ConsoleCoordinator(
            Mock.Of<IBuildSystemFormatterProvider>(),
            Mock.Of<IResultsPrinter>(),
            secretObfuscator.Object,
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            NullLoggerFactory.Instance,
            Mock.Of<IBuildSystemDetector>(),
            Mock.Of<IServiceProvider>(),
            outputCoordinator,
            Mock.Of<ISpectreConsoleLoggerControl>(),
            nonSpectreLoggerFactory.Object,
            spectreLoggerFilter.Object);
    }
}
