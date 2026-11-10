namespace ModularPipelines.UnitTests.Helpers;

internal static class TaskFaultAssertions
{
    public static Task AssertNoUnobservedException(
        Exception expectedException,
        Func<WeakReference> createFaultedOwner) =>
        AssertNoUnobservedException(
            expectedException,
            () => Task.FromResult(createFaultedOwner()));

    public static async Task AssertNoUnobservedException(
        Exception expectedException,
        Func<Task<WeakReference>> createFaultedOwner)
    {
        var exceptionObservedByFinalizer = 0;

        void OnUnobservedTaskException(object? _, UnobservedTaskExceptionEventArgs args)
        {
            if (ContainsException(args.Exception, expectedException))
            {
                Interlocked.Exchange(ref exceptionObservedByFinalizer, 1);
            }
        }

        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        try
        {
            var ownerReference = await createFaultedOwner();

            for (var attempt = 0; attempt < 10 && ownerReference.IsAlive; attempt++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                await Task.Yield();
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
}
