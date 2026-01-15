using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.TestHelpers;

/// <summary>
/// Base class for test modules that simply return a value.
/// Includes <see cref="Task.Yield"/> to ensure proper async scheduling in the pipeline.
/// </summary>
/// <typeparam name="T">The type of result returned by the module.</typeparam>
/// <remarks>
/// The <see cref="Task.Yield"/> call ensures that the async method truly yields control,
/// which is important for testing the pipeline's async scheduling behavior.
/// </remarks>
public abstract class SimpleTestModule<T> : Module<T>
{
    /// <summary>
    /// Gets the result to return from the module.
    /// Override this property to provide a custom return value.
    /// </summary>
    protected abstract T? Result { get; }

    /// <inheritdoc />
    protected internal override async Task<T?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Yield();
        return Result;
    }
}

/// <summary>
/// A simple test module that returns true.
/// </summary>
public class TrueModule : SimpleTestModule<bool>
{
    /// <inheritdoc />
    protected override bool Result => true;
}

/// <summary>
/// A simple test module that returns null.
/// </summary>
public class NullModule : SimpleTestModule<object?>
{
    /// <inheritdoc />
    protected override object? Result => null;
}
