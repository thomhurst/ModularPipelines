using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests;

public class ModuleTimeoutTests : TestBase
{
    private class Module : Module<string>
    {
        protected override TimeSpan Timeout { get; } = TimeSpan.FromSeconds(3);

        protected override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            return "Foo bar!";
        }
    }

    [Test]
    public void Throws_Timeout_Exception()
    {
        var exception = Assert.ThrowsAsync<ModuleFailedException>(RunModule<Module>);
        Assert.That(exception!.InnerException, Is.TypeOf<TaskCanceledException>());
    }
}
