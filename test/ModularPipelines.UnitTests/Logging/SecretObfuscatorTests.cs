using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.TestHelpers;
using ModularPipelines.UnitTests.Models;
using Moq;

namespace ModularPipelines.UnitTests.Logging;

public class SecretObfuscatorTests
{
    private readonly Mock<IBuildSystemDetector> _buildSystemMock;
    private readonly Mock<IConsoleWriter> _consoleWriterMock;

    private readonly StringBuilder _stringBuilder = new();

    public SecretObfuscatorTests()
    {
        _buildSystemMock = new Mock<IBuildSystemDetector>();

        _consoleWriterMock = new Mock<IConsoleWriter>();
        _consoleWriterMock.Setup(x => x.LogToConsole(It.IsAny<string>()))
            .Callback<string>(value => _stringBuilder.AppendLine(value));
    }

    [Test]
    public async Task GitHubActions_MasksSecrets()
    {
        _buildSystemMock.Setup(x => x.GetCurrentBuildSystem()).Returns(BuildSystem.GitHubActions);

        await ExecutePipelineAsync();

        var logOutput = _stringBuilder.ToString();
        await Assert.That(logOutput).Contains("::add-mask::This is a secret value!");
        await Assert.That(logOutput).DoesNotContain("::add-mask::This is NOT a secret value!");
    }

    [Test]
    public async Task DoesNotMaskSecrets_WhenNotGitHubActions()
    {
        _buildSystemMock.Setup(x => x.GetCurrentBuildSystem()).Returns(BuildSystem.Unknown);

        await ExecutePipelineAsync();

        var logOutput = _stringBuilder.ToString();
        await Assert.That(logOutput).DoesNotContain("::add-mask::This is a secret value!");
        await Assert.That(logOutput).DoesNotContain("::add-mask::This is NOT a secret value!");
    }

    private IPipeline GetPipeline()
    {
        var builder = TestPipelineHostBuilder.Create();
        builder.Services.AddSingleton(_buildSystemMock.Object);
        builder.Services.Configure<MyModel>(builder.Configuration);
        builder.Services.AddSingleton(_consoleWriterMock.Object);
        builder.Services.AddModule<GlobalDummyModule>();
        return builder.Build();
    }

    private async Task ExecutePipelineAsync()
    {
        var pipeline = GetPipeline();
        await pipeline.RunAsync();
    }
}
