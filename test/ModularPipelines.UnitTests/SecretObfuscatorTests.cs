using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Extensions;
using ModularPipelines.UnitTests.Models;
using Moq;

namespace ModularPipelines.UnitTests;

[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
[Parallelizable(ParallelScope.None)]
public class SecretObfuscatorTests
{
    private IHost _pipeline = null!;
    private Mock<IBuildSystemDetector> _buildSystemMock = null!;
   
    private static TextWriter? _console;
    [OneTimeSetUp]
    public static void SetUp()
    {
        _console = Console.Out;
    }

    [OneTimeTearDown]
    public static void TearDown()
    {
        Console.SetOut(_console!);
    }

    [SetUp]
    public async Task Setup()
    {
        _buildSystemMock = new Mock<IBuildSystemDetector>();

        _pipeline = await TestPipelineHostBuilder.Create()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(_buildSystemMock.Object);
                services.Configure<MyModel>(context.Configuration);
            })
            .AddModule<GlobalDummyModule>()
            .BuildHostAsync();
    }
    
    [Test]
    public async Task GitHubActions_MasksSecrets()
    {
        _buildSystemMock.Setup(x => x.IsRunningOnGitHubActions).Returns(true);

        await using var stringWriter = new StringWriter();
        
        Console.SetOut(stringWriter);
        
        await _pipeline.ExecuteAsync();
            
        var consoleOutput = stringWriter.ToString();
        
        Assert.That(consoleOutput, Contains.Substring("::add-mask::This is a secret value!"));
        Assert.That(consoleOutput, Does.Not.Contains("::add-mask::This is NOT a secret value!"));
    }
    
    [Test]
    public async Task DoesNotMaskSecrets_WhenNotGitHubActions()
    {
        _buildSystemMock.Setup(x => x.IsRunningOnGitHubActions).Returns(false);

        await using var stringWriter = new StringWriter();
        
        Console.SetOut(stringWriter);
        
        await _pipeline.ExecuteAsync();
            
        var consoleOutput = stringWriter.ToString();
        
        Assert.That(consoleOutput, Does.Not.Contains("::add-mask::This is a secret value!"));
        Assert.That(consoleOutput, Does.Not.Contains("::add-mask::This is NOT a secret value!"));
    }
}