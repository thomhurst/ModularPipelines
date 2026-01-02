using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.TestHelpers;

/// <summary>
/// Base class for test modules that throw an exception.
/// Includes <see cref="Task.Yield"/> to ensure proper async scheduling in the pipeline.
/// </summary>
/// <typeparam name="T">The type of result the module would return (used for type compatibility).</typeparam>
/// <remarks>
/// The <see cref="Task.Yield"/> call ensures that the async method truly yields control,
/// which is important for testing the pipeline's async scheduling behavior.
/// </remarks>
public abstract class ThrowingTestModule<T> : Module<T>
{
    /// <summary>
    /// Gets the exception to throw. Override to provide a custom exception.
    /// </summary>
    protected virtual Exception ExceptionToThrow => new Exception();

    /// <inheritdoc />
    public override async Task<T?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Yield();
        throw ExceptionToThrow;
    }
}

/// <summary>
/// A simple test module that throws a generic exception.
/// </summary>
public class ExceptionModule : ThrowingTestModule<bool>
{
}
