using Microsoft.Extensions.Logging;
using ModularPipelines.Logging;

namespace ModularPipelines.UnitTests.Logging;

[TUnit.Core.NotInParallel]
public class ModuleOutputContextScopeTests
{
    private sealed class StubModuleLogger : IModuleLogger
    {
        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public void Dispose()
        {
        }
    }

    private sealed class FirstModule;

    private sealed class SecondModule;

    [Test]
    public async Task ScopeSetsOneConsistentContext()
    {
        var logger = new StubModuleLogger();

        using (new ModuleOutputContextScope(typeof(FirstModule), logger))
        {
            var context = AmbientModuleOutputContext.Current;

            using (Assert.Multiple())
            {
                await Assert.That(context).IsNotNull();
                await Assert.That(context!.ModuleType).IsEqualTo(typeof(FirstModule));
                await Assert.That(context.Logger).IsSameReferenceAs(logger);
            }
        }

        await Assert.That(AmbientModuleOutputContext.Current).IsNull();
    }

    [Test]
    public async Task ConstructionScopeSupportsTypeWithoutLogger()
    {
        using (new ModuleOutputContextScope(typeof(FirstModule)))
        {
            var context = AmbientModuleOutputContext.Current;

            using (Assert.Multiple())
            {
                await Assert.That(context).IsNotNull();
                await Assert.That(context!.ModuleType).IsEqualTo(typeof(FirstModule));
                await Assert.That(context.Logger).IsNull();
            }
        }
    }

    [Test]
    public async Task NestedScopesRestorePreviousContext()
    {
        var firstLogger = new StubModuleLogger();
        var secondLogger = new StubModuleLogger();

        using (new ModuleOutputContextScope(typeof(FirstModule), firstLogger))
        {
            var firstContext = AmbientModuleOutputContext.Current;

            using (new ModuleOutputContextScope(typeof(SecondModule), secondLogger))
            {
                await Assert.That(AmbientModuleOutputContext.Current!.ModuleType)
                    .IsEqualTo(typeof(SecondModule));
            }

            await Assert.That(AmbientModuleOutputContext.Current)
                .IsSameReferenceAs(firstContext);
        }
    }

    [Test]
    public async Task ContextFlowsAcrossAwaitAndTaskRun()
    {
        using (new ModuleOutputContextScope(typeof(FirstModule), new StubModuleLogger()))
        {
            await Task.Yield();
            var taskContext = await Task.Run(static () => AmbientModuleOutputContext.Current);

            await Assert.That(taskContext)
                .IsSameReferenceAs(AmbientModuleOutputContext.Current);
        }
    }

    [Test]
    public async Task SuppressedExecutionContextFlowIsUnattributed()
    {
        Task<ModuleOutputContext?> task;
        using (new ModuleOutputContextScope(typeof(FirstModule), new StubModuleLogger()))
        {
            using (ExecutionContext.SuppressFlow())
            {
                task = Task.Run(static () => AmbientModuleOutputContext.Current);
            }

            await Assert.That(await task).IsNull();
        }
    }

    [Test]
    public async Task FlowedContextBecomesInactiveAfterOwningScopeEnds()
    {
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        Task<ModuleOutputContext?> lateRead;

        using (new ModuleOutputContextScope(typeof(FirstModule), new StubModuleLogger()))
        {
            lateRead = Task.Run(async () =>
            {
                await release.Task.ConfigureAwait(false);
                return AmbientModuleOutputContext.Current;
            });
        }

        release.SetResult();

        await Assert.That(await lateRead).IsNull();
    }

    [Test]
    public async Task DisposeIsIdempotent()
    {
        var outerLogger = new StubModuleLogger();
        using (new ModuleOutputContextScope(typeof(FirstModule), outerLogger))
        {
            var scope = new ModuleOutputContextScope(typeof(SecondModule), new StubModuleLogger());
            scope.Dispose();

            using (new ModuleOutputContextScope(typeof(SecondModule)))
            {
                scope.Dispose();

                await Assert.That(AmbientModuleOutputContext.Current!.ModuleType)
                    .IsEqualTo(typeof(SecondModule));
            }

            await Assert.That(AmbientModuleOutputContext.Current!.Logger)
                .IsSameReferenceAs(outerLogger);
        }
    }
}
