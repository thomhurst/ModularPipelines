using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.TestHelpers;

/// <summary>
/// Base class for test modules that use the synchronous <see cref="SyncModule{T}"/> pattern.
/// </summary>
/// <typeparam name="T">The type of result returned by the module.</typeparam>
public abstract class SimpleSyncTestModule<T> : SyncModule<T>
{
    /// <summary>
    /// Gets the result to return from the module.
    /// Override this property to provide a custom return value.
    /// </summary>
    protected abstract T? Result { get; }

    /// <inheritdoc />
    protected override T? Execute(IModuleContext context, CancellationToken cancellationToken)
    {
        return Result;
    }
}

/// <summary>
/// A simple synchronous test module that returns true.
/// </summary>
public class SyncTrueModule : SimpleSyncTestModule<bool>
{
    /// <inheritdoc />
    protected override bool Result => true;
}

/// <summary>
/// A simple synchronous test module that returns null.
/// </summary>
public class SyncNullModule : SimpleSyncTestModule<object?>
{
    /// <inheritdoc />
    protected override object? Result => null;
}

/// <summary>
/// A synchronous test module that throws an exception.
/// </summary>
public class ThrowingSyncTestModule<T> : SyncModule<T>
{
    /// <inheritdoc />
    protected override T? Execute(IModuleContext context, CancellationToken cancellationToken)
    {
        throw new InvalidOperationException("Test exception from synchronous module");
    }
}
