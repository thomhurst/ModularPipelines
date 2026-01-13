using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Execution;

public class ModuleTimeoutTests : TestBase
{
    private class Module_UsingCancellationToken : Module<string>
    {
        private static readonly TaskCompletionSource<bool> _taskCompletionSource = new();

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithTimeout(TimeSpan.FromSeconds(1))
            .Build();

        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await _taskCompletionSource.Task.WaitAsync(cancellationToken);
            return TestConstants.TestString;
        }
    }

    private class Module_NotUsingCancellationToken : Module<string>
    {
        private static readonly TaskCompletionSource<bool> _taskCompletionSource = new();

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithTimeout(TimeSpan.FromSeconds(1))
            .Build();

        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            try
            {
                await _taskCompletionSource.Task.WaitAsync(TimeSpan.FromSeconds(5));
            }
            catch (TimeoutException)
            {
            }

            return TestConstants.TestString;
        }
    }

    private class NoTimeoutModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(10), cancellationToken);
            return TestConstants.TestString;
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

    [Test]
    public async Task Timeout_Exception_Contains_Configured_Timeout()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_UsingCancellationToken>);

        var timeoutException = exception.InnerException as ModuleTimeoutException;
        await Assert.That(timeoutException).IsNotNull();
        await Assert.That(timeoutException!.ConfiguredTimeout).IsEqualTo(TimeSpan.FromSeconds(1));
    }

    [Test]
    public async Task Timeout_Exception_Contains_Module_Type()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_UsingCancellationToken>);

        var timeoutException = exception.InnerException as ModuleTimeoutException;
        await Assert.That(timeoutException).IsNotNull();
        await Assert.That(timeoutException!.ModuleType).IsEqualTo(typeof(Module_UsingCancellationToken));
    }

    [Test]
    public async Task Timeout_Exception_Contains_Elapsed_Time()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_UsingCancellationToken>);

        var timeoutException = exception.InnerException as ModuleTimeoutException;
        await Assert.That(timeoutException).IsNotNull();

        // Elapsed time should be at least close to the configured timeout
        await Assert.That(timeoutException!.ElapsedTime).IsGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(900));
    }

    [Test]
    public async Task Timeout_Exception_Indicates_Token_Was_Respected_When_Module_Uses_Token()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_UsingCancellationToken>);

        var timeoutException = exception.InnerException as ModuleTimeoutException;
        await Assert.That(timeoutException).IsNotNull();
        await Assert.That(timeoutException!.WasCancellationTokenRespected).IsTrue();
    }

    [Test]
    public async Task Timeout_Exception_Indicates_Token_Was_Not_Respected_When_Module_Ignores_Token()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_NotUsingCancellationToken>);

        var timeoutException = exception.InnerException as ModuleTimeoutException;
        await Assert.That(timeoutException).IsNotNull();
        await Assert.That(timeoutException!.WasCancellationTokenRespected).IsFalse();
    }

    [Test]
    public async Task Timeout_Exception_Message_Includes_Warning_When_Token_Ignored()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_NotUsingCancellationToken>);

        var timeoutException = exception.InnerException as ModuleTimeoutException;
        await Assert.That(timeoutException).IsNotNull();
        await Assert.That(timeoutException!.Message).Contains("did not respond to the cancellation token");
    }
}
