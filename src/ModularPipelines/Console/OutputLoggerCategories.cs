using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Console;

internal static class OutputLoggerCategories
{
    public const string Pipeline = "ModularPipelines.Output";

    private static readonly ConcurrentDictionary<Type, string> ModuleCategories = [];
    private static readonly CategoryNameLoggerFactory CategoryNameFactory = new();

    public static string ForModule(Type moduleType) =>
        moduleType == typeof(void)
            ? Pipeline
            : ModuleCategories.GetOrAdd(moduleType, GetCategoryName);

    private static string GetCategoryName(Type moduleType)
    {
        var logger = (CategoryNameLogger) CategoryNameFactory.CreateLogger(moduleType);
        return logger.CategoryName;
    }

    private sealed class CategoryNameLoggerFactory : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider)
        {
        }

        public ILogger CreateLogger(string categoryName) => new CategoryNameLogger(categoryName);

        public void Dispose()
        {
        }
    }

    private sealed class CategoryNameLogger(string categoryName) : ILogger
    {
        public string CategoryName { get; } = categoryName;

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => false;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
        }
    }
}
