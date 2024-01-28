using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class ModuleTimeoutTests : TestBase
{
    private class Module_UsingCancellationToken : Module<string>
    {
        protected internal override TimeSpan Timeout { get; } = TimeSpan.FromSeconds(1);

        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            return "Foo bar!";
        }
    }
    
    private class Module_NotUsingCancellationToken : Module<string>
    {
        protected internal override TimeSpan Timeout { get; } = TimeSpan.FromSeconds(1);

        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), CancellationToken.None);
            return "Foo bar!";
        }
    }
    
    private class NoTimeoutModule : Module<string>
    {
        protected internal override TimeSpan Timeout { get; } = TimeSpan.Zero;

        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken);
            return "Foo bar!";
        }
    }

    [Test]
    public void Throws_TaskException_When_Using_CancellationToken()
    {
        var exception = Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_UsingCancellationToken>);

        Assert.That(exception!.InnerException, Is.TypeOf<ModuleTimeoutException>());
    }
    
    [Test]
    public void Throws_Timeout_Exception_When_Not_Using_CancellationToken()
    {
        var exception = Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module_NotUsingCancellationToken>);

        Assert.That(exception!.InnerException, Is.TypeOf<ModuleTimeoutException>());
    }
    
    [Test]
    public void No_Timeout_Does_Not_Throw_Exception()
    {
        Assert.DoesNotThrowAsync(RunModule<NoTimeoutModule>);
    }
}