using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using ModularPipelines.Modules.Behaviors;

namespace ModularPipelines.UnitTests;

public class ModuleTimeoutTests : TestBase
{
    private class Module_UsingCancellationToken : Module<string>, IModuleTimeout
    {
        private static readonly TaskCompletionSource<bool> _taskCompletionSource = new();

        public TimeSpan GetTimeout() => TimeSpan.FromSeconds(1);

        public override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await _taskCompletionSource.Task.WaitAsync(cancellationToken);
            return "Foo bar!";
        }
    }

    private class Module_NotUsingCancellationToken : Module<string>, IModuleTimeout
    {
        private static readonly TaskCompletionSource<bool> _taskCompletionSource = new();

        public TimeSpan GetTimeout() => TimeSpan.FromSeconds(1);

        public override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            try
            {
                await _taskCompletionSource.Task.WaitAsync(TimeSpan.FromSeconds(5));
            }
            catch (TimeoutException)
            {
            }
            return "Foo bar!";
        }
    }

    private class NoTimeoutModule : Module<string>, IModuleTimeout
    {
        public TimeSpan GetTimeout() => TimeSpan.Zero;

        public override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
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