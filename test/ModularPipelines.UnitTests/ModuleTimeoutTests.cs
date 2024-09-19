using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using TUnit.Assertions.Extensions.Throws;

namespace ModularPipelines.UnitTests;

public class ModuleTimeoutTests : TestBase
{
    private class Module_UsingCancellationToken : Module<string>
    {
        protected internal override TimeSpan Timeout { get; } = TimeSpan.FromSeconds(1);

        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
            return "Foo bar!";
        }
    }

    private class Module_NotUsingCancellationToken : Module<string>
    {
        protected internal override TimeSpan Timeout { get; } = TimeSpan.FromSeconds(1);

        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(30), CancellationToken.None);
            return "Foo bar!";
        }
    }

    private class NoTimeoutModule : Module<string>
    {
        protected internal override TimeSpan Timeout => TimeSpan.Zero;

        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken);
            return "Foo bar!";
        }
    }

    [Test, Retry(3)]
    public async Task Throws_TaskException_When_Using_CancellationToken()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_UsingCancellationToken>);
        await Assert.That(exception.InnerException).IsTypeOf(typeof(ModuleTimeoutException)).Or.IsTypeOf(typeof(TaskCanceledException));
    }

    [Test]
    public async Task Throws_Timeout_Exception_When_Not_Using_CancellationToken()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_NotUsingCancellationToken>);
        await Assert.That(exception.InnerException).IsTypeOf(typeof(ModuleTimeoutException)).Or.IsTypeOf(typeof(OperationCanceledException)).Or.IsTypeOf(typeof(TaskCanceledException));
    }

    [Test]
    public async Task No_Timeout_Does_Not_Throw_Exception()
    {
        await Assert.That(RunModule<NoTimeoutModule>).ThrowsNothing();
    }
}