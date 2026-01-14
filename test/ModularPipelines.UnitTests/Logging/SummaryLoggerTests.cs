using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Logging;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Logging;

public class SummaryLoggerTests
{
    #region Summary API Tests

    private class SummaryInfoLoggingModule : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            context.Summary.Info("Info message");
            await Task.CompletedTask;
            return true;
        }
    }

    private class SummarySuccessLoggingModule : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            context.Summary.Success("Success message");
            await Task.CompletedTask;
            return true;
        }
    }

    private class SummaryWarningLoggingModule : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            context.Summary.Warning("Warning message");
            await Task.CompletedTask;
            return true;
        }
    }

    private class SummaryErrorLoggingModule : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            context.Summary.Error("Error message");
            await Task.CompletedTask;
            return true;
        }
    }

    private class SummaryKeyValueLoggingModule : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            context.Summary.KeyValue("Version", "1.2.3");
            await Task.CompletedTask;
            return true;
        }
    }

    private class SummaryCategoryLoggingModule : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            context.Summary.Info("Build", "Build completed");
            context.Summary.Success("Build", "All tests passed");
            context.Summary.KeyValue("Artifacts", "Path", "/output/artifacts");
            await Task.CompletedTask;
            return true;
        }
    }

    [Test]
    public async Task SummaryApi_Info_LogsCorrectly()
    {
        var stringBuilder = new StringBuilder();
        var host = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) =>
            {
                collection.AddSingleton(stringBuilder);
                collection.AddSingleton(typeof(ILogger<>), typeof(StringLogger<>));
            })
            .AddModule<SummaryInfoLoggingModule>()
            .BuildHostAsync();

        await host.RunAsync();
        await host.DisposeAsync();

        await Assert.That(stringBuilder.ToString()).Contains("Info message");
    }

    [Test]
    public async Task SummaryApi_Success_LogsCorrectly()
    {
        var stringBuilder = new StringBuilder();
        var host = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) =>
            {
                collection.AddSingleton(stringBuilder);
                collection.AddSingleton(typeof(ILogger<>), typeof(StringLogger<>));
            })
            .AddModule<SummarySuccessLoggingModule>()
            .BuildHostAsync();

        await host.RunAsync();
        await host.DisposeAsync();

        await Assert.That(stringBuilder.ToString()).Contains("Success message");
    }

    [Test]
    public async Task SummaryApi_Warning_LogsCorrectly()
    {
        var stringBuilder = new StringBuilder();
        var host = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) =>
            {
                collection.AddSingleton(stringBuilder);
                collection.AddSingleton(typeof(ILogger<>), typeof(StringLogger<>));
            })
            .AddModule<SummaryWarningLoggingModule>()
            .BuildHostAsync();

        await host.RunAsync();
        await host.DisposeAsync();

        await Assert.That(stringBuilder.ToString()).Contains("Warning message");
    }

    [Test]
    public async Task SummaryApi_Error_LogsCorrectly()
    {
        var stringBuilder = new StringBuilder();
        var host = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) =>
            {
                collection.AddSingleton(stringBuilder);
                collection.AddSingleton(typeof(ILogger<>), typeof(StringLogger<>));
            })
            .AddModule<SummaryErrorLoggingModule>()
            .BuildHostAsync();

        await host.RunAsync();
        await host.DisposeAsync();

        await Assert.That(stringBuilder.ToString()).Contains("Error message");
    }

    [Test]
    public async Task SummaryApi_KeyValue_LogsCorrectly()
    {
        var stringBuilder = new StringBuilder();
        var host = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) =>
            {
                collection.AddSingleton(stringBuilder);
                collection.AddSingleton(typeof(ILogger<>), typeof(StringLogger<>));
            })
            .AddModule<SummaryKeyValueLoggingModule>()
            .BuildHostAsync();

        await host.RunAsync();
        await host.DisposeAsync();

        await Assert.That(stringBuilder.ToString()).Contains("Version: 1.2.3");
    }

    [Test]
    public async Task SummaryApi_Category_LogsCorrectly()
    {
        var stringBuilder = new StringBuilder();
        var host = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) =>
            {
                collection.AddSingleton(stringBuilder);
                collection.AddSingleton(typeof(ILogger<>), typeof(StringLogger<>));
            })
            .AddModule<SummaryCategoryLoggingModule>()
            .BuildHostAsync();

        await host.RunAsync();
        await host.DisposeAsync();

        var output = stringBuilder.ToString();
        await Assert.That(output).Contains("Build");
        await Assert.That(output).Contains("Build completed");
        await Assert.That(output).Contains("All tests passed");
        await Assert.That(output).Contains("Path: /output/artifacts");
    }

    #endregion

    #region GetEntries Tests

    [Test]
    public async Task GetEntries_ReturnsAllEntries()
    {
        var logger = new SummaryLogger(new NullLogger<SummaryLogger>());

        logger.Info("Message 1");
        logger.Success("Message 2");
        logger.Warning("Message 3");

        var entries = logger.GetEntries();

        await Assert.That(entries.Count).IsEqualTo(3);
    }

    [Test]
    public async Task GetEntries_WithCategory_FiltersCorrectly()
    {
        var logger = new SummaryLogger(new NullLogger<SummaryLogger>());

        logger.Info("Build", "Build message");
        logger.Info("Deploy", "Deploy message");
        logger.Info("Build", "Another build message");

        var buildEntries = logger.GetEntries("Build");
        var deployEntries = logger.GetEntries("Deploy");

        await Assert.That(buildEntries.Count).IsEqualTo(2);
        await Assert.That(deployEntries.Count).IsEqualTo(1);
    }

    [Test]
    public async Task GetOutput_FormatsCorrectly()
    {
        var logger = new SummaryLogger(new NullLogger<SummaryLogger>());

        logger.Info("Info message");
        logger.Success("Success message");
        logger.Warning("Warning message");
        logger.Error("Error message");

        var output = logger.GetOutput();

        await Assert.That(output).Contains("Info message");
        await Assert.That(output).Contains("[OK] Success message");
        await Assert.That(output).Contains("[WARN] Warning message");
        await Assert.That(output).Contains("[ERR] Error message");
    }

    [Test]
    public async Task GetOutput_GroupsByCategory()
    {
        var logger = new SummaryLogger(new NullLogger<SummaryLogger>());

        logger.Info("Category1", "Message in Category1");
        logger.Info("Category2", "Message in Category2");
        logger.Info("Uncategorized message");

        var output = logger.GetOutput();

        await Assert.That(output).Contains("[Category1]");
        await Assert.That(output).Contains("[Category2]");
        await Assert.That(output).Contains("Message in Category1");
        await Assert.That(output).Contains("Message in Category2");
        await Assert.That(output).Contains("Uncategorized message");
    }

    #endregion

    #region Log Level Tests

    [Test]
    public async Task Log_WithLevel_AddsCorrectEntry()
    {
        var logger = new SummaryLogger(new NullLogger<SummaryLogger>());

        logger.Log(SummaryLogLevel.Warning, "Test warning");

        var entries = logger.GetEntries();

        await Assert.That(entries.Count).IsEqualTo(1);
        await Assert.That(entries[0].Level).IsEqualTo(SummaryLogLevel.Warning);
        await Assert.That(entries[0].Message).IsEqualTo("Test warning");
    }

    [Test]
    public async Task Log_WithLevelAndCategory_AddsCorrectEntry()
    {
        var logger = new SummaryLogger(new NullLogger<SummaryLogger>());

        logger.Log(SummaryLogLevel.Error, "TestCategory", "Test error");

        var entries = logger.GetEntries();

        await Assert.That(entries.Count).IsEqualTo(1);
        await Assert.That(entries[0].Level).IsEqualTo(SummaryLogLevel.Error);
        await Assert.That(entries[0].Category).IsEqualTo("TestCategory");
        await Assert.That(entries[0].Message).IsEqualTo("Test error");
    }

    #endregion

    #region Thread Safety Tests

    [Test]
    public async Task ConcurrentLogging_IsThreadSafe()
    {
        var logger = new SummaryLogger(new NullLogger<SummaryLogger>());
        var tasks = new List<Task>();
        const int iterationsPerTask = 100;
        const int taskCount = 10;

        for (int t = 0; t < taskCount; t++)
        {
            var taskIndex = t;
            tasks.Add(Task.Run(() =>
            {
                for (int i = 0; i < iterationsPerTask; i++)
                {
                    logger.Info($"Task {taskIndex} message {i}");
                }
            }));
        }

        await Task.WhenAll(tasks);

        var entries = logger.GetEntries();
        await Assert.That(entries.Count).IsEqualTo(taskCount * iterationsPerTask);
    }

    #endregion

    private class NullLogger<T> : ILogger<T>
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
    }
}
