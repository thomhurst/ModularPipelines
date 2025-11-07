using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class ModuleTimeoutTests : TestBase
{
    // Using TaskCompletionSource allows us to test timeout behavior without actual delays
    // The task will wait indefinitely until the timeout mechanism cancels it
    private class Module_UsingCancellationToken : Module<string>
    {
        private static readonly TaskCompletionSource<bool> _taskCompletionSource = new();

        protected internal override TimeSpan Timeout { get; } = TimeSpan.FromSeconds(1);

        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            // This will wait indefinitely (or until cancellation) without blocking the thread
            // The timeout mechanism will cancel it after 1 second
            await _taskCompletionSource.Task.WaitAsync(cancellationToken);
            return "Foo bar!";
        }
    }

    private class Module_NotUsingCancellationToken : Module<string>
    {
        private static readonly TaskCompletionSource<bool> _taskCompletionSource = new();

        protected internal override TimeSpan Timeout { get; } = TimeSpan.FromSeconds(1);

        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            // This will wait indefinitely without honoring cancellation
            // The timeout mechanism should still handle it
            await _taskCompletionSource.Task;
            return "Foo bar!";
        }
    }

    private class NoTimeoutModule : Module<string>
    {
        protected internal override TimeSpan Timeout => TimeSpan.Zero;

        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            // Reduced delay from 500ms to 10ms for faster test execution
            await Task.Delay(TimeSpan.FromMilliseconds(10), cancellationToken);
            return "Foo bar!";
        }
    }

    [Test]
    public async Task Throws_TaskException_When_Using_CancellationToken()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_UsingCancellationToken>);

        var innerException = exception.InnerException;
        var isExpectedType = innerException is ModuleTimeoutException or TaskCanceledException;
        await Assert.That(isExpectedType).IsTrue();
    }

    [Test]
    public async Task Throws_Timeout_Exception_When_Not_Using_CancellationToken()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_NotUsingCancellationToken>);

        var innerException = exception.InnerException;
        var isExpectedType = innerException is ModuleTimeoutException or OperationCanceledException or TaskCanceledException;
        await Assert.That(isExpectedType).IsTrue();
    }

    [Test]
    public async Task No_Timeout_Does_Not_Throw_Exception()
    {
        await Assert.That(RunModule<NoTimeoutModule>).ThrowsNothing();
    }
}