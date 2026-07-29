using Microsoft.Extensions.Logging;
using ModularPipelines.Console;

namespace ModularPipelines.UnitTests.Console;

public class OutputLoggerCategoriesTests
{
    [Test]
    public async Task Module_Category_Matches_ILogger_Type_Category()
    {
        var provider = new CategoryRecordingLoggerProvider();
        using var loggerFactory = new LoggerFactory([provider]);
        var moduleType = typeof(NestedGenericModule<string>);

        _ = loggerFactory.CreateLogger(moduleType);

        await Assert.That(OutputLoggerCategories.ForModule(moduleType))
            .IsEqualTo(provider.CategoryName);
    }

    private sealed class NestedGenericModule<T>;

    private sealed class CategoryRecordingLoggerProvider : ILoggerProvider
    {
        public string? CategoryName { get; private set; }

        public ILogger CreateLogger(string categoryName)
        {
            CategoryName = categoryName;
            return Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
        }

        public void Dispose()
        {
        }
    }
}
