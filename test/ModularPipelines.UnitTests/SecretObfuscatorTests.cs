using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Extensions;
using ModularPipelines.Host;
using ModularPipelines.UnitTests.Extensions;
using ModularPipelines.UnitTests.Models;
using Moq;

namespace ModularPipelines.UnitTests;

[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class SecretObfuscatorTests
{
    private readonly Lazy<Task<IPipelineHost>> _pipeline;
    private readonly Mock<IBuildSystemDetector> _buildSystemMock;
    
    private readonly StringBuilder _stringBuilder = new();
    
    public SecretObfuscatorTests()
    {
        _buildSystemMock = new Mock<IBuildSystemDetector>();

        var consoleWriterMock = new Mock<IConsoleWriter>();
        consoleWriterMock.Setup(x => x.WriteLine(It.IsAny<string>()))
            .Callback<string>(value => _stringBuilder.AppendLine(value));
        
        _pipeline = new Lazy<Task<IPipelineHost>>(TestPipelineHostBuilder.Create()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(_buildSystemMock.Object);
                services.Configure<MyModel>(context.Configuration);
                services.AddSingleton(consoleWriterMock.Object);
            })
            .AddModule<GlobalDummyModule>()
            .BuildHostAsync());
    }
    
    [Test]
    public async Task GitHubActions_MasksSecrets()
    {
        _buildSystemMock.Setup(x => x.IsRunningOnGitHubActions).Returns(true);
        
        await (await _pipeline.Value).ExecutePipelineAsync();

        var logOutput = _stringBuilder.ToString();
        
        Assert.That(logOutput, Contains.Substring("::add-mask::This is a secret value!"));
        Assert.That(logOutput, Does.Not.Contains("::add-mask::This is NOT a secret value!"));
    }
    
    [Test]
    public async Task DoesNotMaskSecrets_WhenNotGitHubActions()
    {
        _buildSystemMock.Setup(x => x.IsRunningOnGitHubActions).Returns(false);
        
        await (await _pipeline.Value).ExecutePipelineAsync();
            
        var logOutput = _stringBuilder.ToString();
        
        Assert.That(logOutput, Does.Not.Contains("::add-mask::This is a secret value!"));
        Assert.That(logOutput, Does.Not.Contains("::add-mask::This is NOT a secret value!"));
    }
}