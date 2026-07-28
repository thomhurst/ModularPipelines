using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Console;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Scheduling;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

[TUnit.Core.NotInParallel]
public class TaskCompletionSourceTests
{
    [Test]
    public async Task ModuleState_CompletionSource_RunsContinuationsAsynchronously()
    {
        var state = new ModuleState(Mock.Of<IModule>(), typeof(IModule));

        await Assert.That(RunsContinuationsAsynchronously(state.CompletionSource.Task)).IsTrue();
    }

    [Test]
    public async Task ModuleResultRegistry_CompletionSource_RunsContinuationsAsynchronously()
    {
        var registry = new ModuleResultRegistry();
        registry.RegisterModule(typeof(IModule));

        await Assert.That(RunsContinuationsAsynchronously(registry.GetCompletionTask(typeof(IModule))!)).IsTrue();
    }

    [Test]
    public async Task ProgressSession_CompletionSource_RunsContinuationsAsynchronously()
    {
        var session = new ProgressSession(
            null!,
            new OrganizedModules([], []),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            NullLoggerFactory.Instance,
            CancellationToken.None);
        var field = typeof(ProgressSession).GetField("_progressCompleted", BindingFlags.Instance | BindingFlags.NonPublic);
        var completionSource = (TaskCompletionSource) field!.GetValue(session)!;

        await Assert.That(RunsContinuationsAsynchronously(completionSource.Task)).IsTrue();
    }

    [Test]
    public async Task ModuleState_Failure_DoesNotRaiseUnobservedTaskException()
    {
        var expectedException = new InvalidOperationException(Guid.NewGuid().ToString());

        await AssertNoUnobservedTaskException(
            expectedException,
            () => CreateFaultedModuleCompletion(expectedException));
    }

    [Test]
    public async Task SubModuleTracker_Failure_DoesNotRaiseUnobservedTaskException()
    {
        var expectedException = new InvalidOperationException(Guid.NewGuid().ToString());

        await AssertNoUnobservedTaskException(
            expectedException,
            () => CreateFaultedSubModuleCompletion(expectedException));
    }

    private static bool RunsContinuationsAsynchronously(Task task) =>
        task.CreationOptions.HasFlag(TaskCreationOptions.RunContinuationsAsynchronously);

    private static async Task AssertNoUnobservedTaskException(
        Exception expectedException,
        Func<WeakReference> createFaultedOwner)
    {
        var exceptionObservedByFinalizer = 0;

        void OnUnobservedTaskException(object? _, UnobservedTaskExceptionEventArgs args)
        {
            if (ContainsException(args.Exception, expectedException))
            {
                Interlocked.Exchange(ref exceptionObservedByFinalizer, 1);
                args.SetObserved();
            }
        }

        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        try
        {
            var ownerReference = createFaultedOwner();

            for (var attempt = 0; attempt < 10 && ownerReference.IsAlive; attempt++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            await Assert.That(ownerReference.IsAlive).IsFalse();
            await Assert.That(Volatile.Read(ref exceptionObservedByFinalizer)).IsEqualTo(0);
        }
        finally
        {
            TaskScheduler.UnobservedTaskException -= OnUnobservedTaskException;
        }
    }

    private static bool ContainsException(Exception candidate, Exception expected)
    {
        if (ReferenceEquals(candidate, expected))
        {
            return true;
        }

        return candidate switch
        {
            AggregateException aggregate => aggregate.InnerExceptions.Any(x => ContainsException(x, expected)),
            { InnerException: not null } => ContainsException(candidate.InnerException, expected),
            _ => false,
        };
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference CreateFaultedModuleCompletion(Exception exception)
    {
        var module = Mock.Of<IModule>();
        var state = new ModuleState(module, module.GetType());
        var moduleStates = new ConcurrentDictionary<Type, ModuleState>();
        moduleStates[state.ModuleType] = state;
        var tracker = new ModuleStateTracker(
            NullLogger.Instance,
            TimeProvider.System,
            Mock.Of<IMetricsCollector>(),
            Mock.Of<IModuleConstraintEvaluator>(),
            moduleStates,
            [],
            [],
            new ModuleStateQueries(moduleStates),
            new ReaderWriterLockSlim(),
            new SemaphoreSlim(0),
            () => false);

        tracker.MarkModuleCompleted(state.ModuleType, success: false, exception);
        return new WeakReference(state.CompletionSource.Task);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference CreateFaultedSubModuleCompletion(Exception exception)
    {
        var tracker = new SubModuleTracker("test", typeof(IModule));
        var executionTask = tracker.ExecuteAsync<int>(() => Task.FromException<int>(exception));

        try
        {
            executionTask.GetAwaiter().GetResult();
        }
        catch (InvalidOperationException caught) when (ReferenceEquals(caught, exception))
        {
            // The caller-observable failure was handled.
        }

        return new WeakReference(tracker);
    }
}
