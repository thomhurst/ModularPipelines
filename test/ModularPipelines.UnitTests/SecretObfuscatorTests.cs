using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Extensions;
using ModularPipelines.Host;
using ModularPipelines.TestHelpers;
using ModularPipelines.UnitTests.Models;
using Moq;

namespace ModularPipelines.UnitTests;

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
        _buildSystemMock.Setup(x => x.IsRunningOnGitHubActions).Returns(true);

        await ExecutePipelineAsync();

        var logOutput = _stringBuilder.ToString();
        await Assert.That(logOutput).Contains("::add-mask::This is a secret value!");
        await Assert.That(logOutput).DoesNotContain("::add-mask::This is NOT a secret value!");
    }

    [Test]
    public async Task DoesNotMaskSecrets_WhenNotGitHubActions()
    {
        _buildSystemMock.Setup(x => x.IsRunningOnGitHubActions).Returns(false);

        await ExecutePipelineAsync();

        var logOutput = _stringBuilder.ToString();
        await Assert.That(logOutput).DoesNotContain("::add-mask::This is a secret value!");
        await Assert.That(logOutput).DoesNotContain("::add-mask::This is NOT a secret value!");
    }

    private Task<IPipelineHost> GetPipelineHost()
    {
        return TestPipelineHostBuilder.Create()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(_buildSystemMock.Object);
                services.Configure<MyModel>(context.Configuration);
                services.AddSingleton(_consoleWriterMock.Object);
            })
            .AddModule<GlobalDummyModule>()
            .BuildHostAsync();
    }

    private async Task ExecutePipelineAsync()
    {
        var pipelineHost = await GetPipelineHost();

        await pipelineHost.ExecutePipelineAsync();
    }
}