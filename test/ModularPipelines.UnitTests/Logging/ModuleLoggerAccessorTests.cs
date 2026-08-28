using ModularPipelines.Secrets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Console;
using ModularPipelines.Engine;
using ModularPipelines.Logging;
using Moq;

namespace ModularPipelines.UnitTests.Logging;

public class ModuleLoggerAccessorTests
{
    [Test]
    public async Task AccessorLifetimeEnd_DoesNotPreventAsyncLoggerFlush()
    {
        var buffer = new Mock<IModuleOutputBuffer>();
        var consoleCoordinator = new Mock<IConsoleCoordinator>();
        consoleCoordinator
            .Setup(x => x.GetModuleBuffer(typeof(TestModule)))
            .Returns(buffer.Object);
        var outputCoordinator = new Mock<IOutputCoordinator>();
        outputCoordinator
            .Setup(x => x.OnModuleCompletedAsync(
                buffer.Object,
                typeof(TestModule),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var services = new ServiceCollection()
            .AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory>(NullLoggerFactory.Instance)
            .AddSingleton(typeof(Microsoft.Extensions.Logging.ILogger<>), typeof(NullLogger<>))
            .AddSingleton(Mock.Of<ISecretObfuscator>())
            .AddSingleton(Mock.Of<IFormattedLogValuesObfuscator>())
            .AddSingleton(consoleCoordinator.Object)
            .AddSingleton(outputCoordinator.Object)
            .AddSingleton(Mock.Of<IStackTraceModuleDetector>())
            .AddScoped<ModuleLoggerAccessor>()
            .AddScoped(typeof(ModuleLogger<>));
        await using var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateAsyncScope();
        var previousLogger = ModuleLogger.Values.Value;

        try
        {
            var logger = scope.ServiceProvider.GetRequiredService<ModuleLogger<TestModule>>();
            var accessor = scope.ServiceProvider.GetRequiredService<ModuleLoggerAccessor>();
            ModuleLogger.Values.Value = logger;
            _ = accessor.Logger;
        }
        finally
        {
            ModuleLogger.Values.Value = previousLogger;
            await scope.DisposeAsync();
        }

        outputCoordinator.Verify(
            x => x.OnModuleCompletedAsync(
                buffer.Object,
                typeof(TestModule),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private sealed class TestModule;
}
