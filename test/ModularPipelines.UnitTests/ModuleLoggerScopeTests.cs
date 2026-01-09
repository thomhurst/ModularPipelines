using Microsoft.Extensions.Logging;
using ModularPipelines.Logging;

namespace ModularPipelines.UnitTests;

public class ModuleLoggerScopeTests
{
    // Simple mock implementation of IModuleLogger for testing
    private class MockModuleLogger : IModuleLogger
    {
        public string Name { get; }

        public MockModuleLogger(string name)
        {
            Name = name;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public void Dispose()
        {
        }

        void IModuleLogger.SetException(Exception exception)
        {
        }
    }

    // Dummy types to use as module type identifiers for testing
    private class TestModuleA
    {
    }

    private class TestModuleB
    {
    }

    private class TestModuleC
    {
    }

    [Before(Test)]
    public void Setup()
    {
        // Ensure clean state before each test
        ModuleLogger.Values.Value = null;
        ModuleLogger.CurrentModuleType.Value = null;
    }

    [After(Test)]
    public void Cleanup()
    {
        // Ensure clean state after each test
        ModuleLogger.Values.Value = null;
        ModuleLogger.CurrentModuleType.Value = null;
    }

    [Test]
    public async Task ModuleLoggerScope_SetsContext_WhenCreated()
    {
        var mockLogger = new MockModuleLogger("TestLogger");
        var moduleType = typeof(TestModuleA);

        await using (new ModuleLoggerScope(mockLogger, moduleType))
        {
            await Assert.That(ModuleLogger.Values.Value).IsSameReferenceAs(mockLogger);
            await Assert.That(ModuleLogger.CurrentModuleType.Value).IsEqualTo(moduleType);
        }
    }

    [Test]
    public async Task ModuleLoggerScope_RestoresNullContext_AfterDispose()
    {
        // Initial state: null
        ModuleLogger.Values.Value = null;
        ModuleLogger.CurrentModuleType.Value = null;

        var mockLogger = new MockModuleLogger("TestLogger");
        var moduleType = typeof(TestModuleA);

        await using (new ModuleLoggerScope(mockLogger, moduleType))
        {
            // Inside scope - should have new values
            await Assert.That(ModuleLogger.Values.Value).IsSameReferenceAs(mockLogger);
            await Assert.That(ModuleLogger.CurrentModuleType.Value).IsEqualTo(moduleType);
        }

        // After scope - should be restored to null
        await Assert.That(ModuleLogger.Values.Value).IsNull();
        await Assert.That(ModuleLogger.CurrentModuleType.Value).IsNull();
    }

    [Test]
    public async Task ModuleLoggerScope_RestoresPreviousContext_AfterDispose()
    {
        var previousLogger = new MockModuleLogger("PreviousLogger");
        var previousType = typeof(TestModuleA);

        // Set up initial context
        ModuleLogger.Values.Value = previousLogger;
        ModuleLogger.CurrentModuleType.Value = previousType;

        var newLogger = new MockModuleLogger("NewLogger");
        var newType = typeof(TestModuleB);

        await using (new ModuleLoggerScope(newLogger, newType))
        {
            // Inside scope - should have new values
            await Assert.That(ModuleLogger.Values.Value).IsSameReferenceAs(newLogger);
            await Assert.That(ModuleLogger.CurrentModuleType.Value).IsEqualTo(newType);
        }

        // After scope - should be restored to previous values
        await Assert.That(ModuleLogger.Values.Value).IsSameReferenceAs(previousLogger);
        await Assert.That(ModuleLogger.CurrentModuleType.Value).IsEqualTo(previousType);
    }

    [Test]
    public async Task ModuleLoggerScope_NestedScopes_RestoreCorrectly()
    {
        var loggerA = new MockModuleLogger("LoggerA");
        var typeA = typeof(TestModuleA);

        var loggerB = new MockModuleLogger("LoggerB");
        var typeB = typeof(TestModuleB);

        var loggerC = new MockModuleLogger("LoggerC");
        var typeC = typeof(TestModuleC);

        // Initial state
        await Assert.That(ModuleLogger.Values.Value).IsNull();
        await Assert.That(ModuleLogger.CurrentModuleType.Value).IsNull();

        await using (var scopeA = new ModuleLoggerScope(loggerA, typeA))
        {
            // First scope
            await Assert.That(ModuleLogger.Values.Value).IsSameReferenceAs(loggerA);
            await Assert.That(ModuleLogger.CurrentModuleType.Value).IsEqualTo(typeA);

            await using (var scopeB = new ModuleLoggerScope(loggerB, typeB))
            {
                // Second scope (nested)
                await Assert.That(ModuleLogger.Values.Value).IsSameReferenceAs(loggerB);
                await Assert.That(ModuleLogger.CurrentModuleType.Value).IsEqualTo(typeB);

                await using (var scopeC = new ModuleLoggerScope(loggerC, typeC))
                {
                    // Third scope (deeply nested)
                    await Assert.That(ModuleLogger.Values.Value).IsSameReferenceAs(loggerC);
                    await Assert.That(ModuleLogger.CurrentModuleType.Value).IsEqualTo(typeC);
                }

                // After third scope - back to second
                await Assert.That(ModuleLogger.Values.Value).IsSameReferenceAs(loggerB);
                await Assert.That(ModuleLogger.CurrentModuleType.Value).IsEqualTo(typeB);
            }

            // After second scope - back to first
            await Assert.That(ModuleLogger.Values.Value).IsSameReferenceAs(loggerA);
            await Assert.That(ModuleLogger.CurrentModuleType.Value).IsEqualTo(typeA);
        }

        // After all scopes - back to null
        await Assert.That(ModuleLogger.Values.Value).IsNull();
        await Assert.That(ModuleLogger.CurrentModuleType.Value).IsNull();
    }

    [Test]
    public async Task ModuleLoggerScope_RestoresContext_EvenAfterException()
    {
        var previousLogger = new MockModuleLogger("PreviousLogger");
        var previousType = typeof(TestModuleA);

        // Set up initial context
        ModuleLogger.Values.Value = previousLogger;
        ModuleLogger.CurrentModuleType.Value = previousType;

        var newLogger = new MockModuleLogger("NewLogger");
        var newType = typeof(TestModuleB);

        try
        {
            await using (new ModuleLoggerScope(newLogger, newType))
            {
                // Inside scope - should have new values
                await Assert.That(ModuleLogger.Values.Value).IsSameReferenceAs(newLogger);
                await Assert.That(ModuleLogger.CurrentModuleType.Value).IsEqualTo(newType);

                // Simulate an exception during module execution
                throw new InvalidOperationException("Test exception");
            }
        }
        catch (InvalidOperationException)
        {
            // Expected exception
        }

        // Context should still be restored to previous values
        await Assert.That(ModuleLogger.Values.Value).IsSameReferenceAs(previousLogger);
        await Assert.That(ModuleLogger.CurrentModuleType.Value).IsEqualTo(previousType);
    }

    [Test]
    public async Task ModuleLoggerScope_DoubleDispose_IsIdempotent()
    {
        var previousLogger = new MockModuleLogger("PreviousLogger");
        var previousType = typeof(TestModuleA);

        // Set up initial context
        ModuleLogger.Values.Value = previousLogger;
        ModuleLogger.CurrentModuleType.Value = previousType;

        var newLogger = new MockModuleLogger("NewLogger");
        var newType = typeof(TestModuleB);

        var scope = new ModuleLoggerScope(newLogger, newType);

        // First dispose
        await scope.DisposeAsync();

        // Context should be restored
        await Assert.That(ModuleLogger.Values.Value).IsSameReferenceAs(previousLogger);
        await Assert.That(ModuleLogger.CurrentModuleType.Value).IsEqualTo(previousType);

        // Change context again to verify double dispose doesn't affect anything
        var anotherLogger = new MockModuleLogger("AnotherLogger");
        var anotherType = typeof(TestModuleC);
        ModuleLogger.Values.Value = anotherLogger;
        ModuleLogger.CurrentModuleType.Value = anotherType;

        // Second dispose - should be no-op
        await scope.DisposeAsync();

        // Context should NOT be changed by the second dispose
        await Assert.That(ModuleLogger.Values.Value).IsSameReferenceAs(anotherLogger);
        await Assert.That(ModuleLogger.CurrentModuleType.Value).IsEqualTo(anotherType);
    }

    [Test]
    public async Task ModuleLoggerScope_AsyncExecution_MaintainsContext()
    {
        var mockLogger = new MockModuleLogger("TestLogger");
        var moduleType = typeof(TestModuleA);

        await using (new ModuleLoggerScope(mockLogger, moduleType))
        {
            // Verify context survives async operations
            await Task.Delay(10);

            await Assert.That(ModuleLogger.Values.Value).IsSameReferenceAs(mockLogger);
            await Assert.That(ModuleLogger.CurrentModuleType.Value).IsEqualTo(moduleType);

            // Verify context survives Task.Yield
            await Task.Yield();

            await Assert.That(ModuleLogger.Values.Value).IsSameReferenceAs(mockLogger);
            await Assert.That(ModuleLogger.CurrentModuleType.Value).IsEqualTo(moduleType);
        }
    }
}
