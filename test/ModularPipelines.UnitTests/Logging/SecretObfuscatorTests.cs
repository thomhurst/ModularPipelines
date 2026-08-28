using ModularPipelines.Context;
using ModularPipelines.Logging;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Logging;
using ModularPipelines.TestHelpers;
using ModularPipelines.UnitTests.Models;
using Moq;

namespace ModularPipelines.UnitTests.Logging;

public class SecretObfuscatorTests
{
    private readonly Mock<IBuildSystemDetector> _buildSystemMock;
    private readonly Mock<IConsoleWriter> _consoleWriterMock;
    private readonly Mock<IBuildSystemCommandWriter> _commandWriterMock;

    private readonly StringBuilder _consoleOutput = new();
    private readonly StringBuilder _buildSystemCommands = new();

    public SecretObfuscatorTests()
    {
        _buildSystemMock = new Mock<IBuildSystemDetector>();

        _consoleWriterMock = new Mock<IConsoleWriter>();
        _consoleWriterMock.Setup(x => x.WriteLine(It.IsAny<string>()))
            .Callback<string>(value => _consoleOutput.AppendLine(value));

        _commandWriterMock = new Mock<IBuildSystemCommandWriter>();
        _commandWriterMock.Setup(x => x.WriteLine(It.IsAny<string>()))
            .Callback<string>(value => _buildSystemCommands.AppendLine(value));
    }

    [Test]
    public async Task GitHubActions_MasksSecrets()
    {
        _buildSystemMock.Setup(x => x.GetCurrentBuildSystem()).Returns(BuildSystem.GitHubActions);

        await RunAsync();

        var logOutput = _buildSystemCommands.ToString();
        await Assert.That(logOutput).Contains("::add-mask::This is a secret value!");
        await Assert.That(logOutput).DoesNotContain("::add-mask::This is NOT a secret value!");
    }

    [Test]
    public async Task GitHubActions_MaskCommands_BypassRichConsoleWriter()
    {
        _buildSystemMock.Setup(x => x.GetCurrentBuildSystem()).Returns(BuildSystem.GitHubActions);

        await RunAsync();

        _consoleWriterMock.Verify(
            writer => writer.WriteLine(It.Is<string>(
                value => value.StartsWith("::add-mask::", StringComparison.Ordinal))),
            Times.Never);
        await Assert.That(_consoleOutput.ToString())
            .DoesNotContain("VGhpcyBpcyBhIHNlY3JldCB2YWx1ZSE=");
    }

    [Test]
    public async Task DoesNotMaskSecrets_WhenNotGitHubActions()
    {
        _buildSystemMock.Setup(x => x.GetCurrentBuildSystem()).Returns(BuildSystem.Unknown);

        await RunAsync();

        var logOutput = _buildSystemCommands.ToString();
        await Assert.That(logOutput).DoesNotContain("::add-mask::This is a secret value!");
        await Assert.That(logOutput).DoesNotContain("::add-mask::This is NOT a secret value!");
    }

    private async Task<IPipeline> GetPipeline()
    {
        var builder = TestPipelineBuilder.Create();
        builder.Services.AddSingleton(_buildSystemMock.Object);
        builder.Services.Configure<MyModel>(builder.Configuration);
        builder.Services.AddSingleton(_consoleWriterMock.Object);
        builder.Services.AddSingleton(_commandWriterMock.Object);
        builder.AddModule<GlobalDummyModule>();
        return await builder.BuildAsync();
    }

    private async Task RunAsync()
    {
        var pipeline = await GetPipeline();
        await pipeline.RunAsync();
    }
}
