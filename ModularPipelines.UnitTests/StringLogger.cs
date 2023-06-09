﻿using System.Collections.Concurrent;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ModularPipelines.UnitTests;

public class StringLogger<T> : ILogger<T>
{
    private readonly StringBuilder _stringBuilder;

    public StringLogger(StringBuilder stringBuilder)
    {
        _stringBuilder = stringBuilder;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var log = formatter.Invoke(state, exception);
        _stringBuilder.AppendLine(log);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return new NoOpDisposable();
    }

    private class NoOpDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}