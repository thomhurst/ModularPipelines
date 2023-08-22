using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests;

public class ModuleTimeoutTests : TestBase
{
    private class Module : Module<string>
    {
        protected override TimeSpan Timeout { get; } = TimeSpan.FromSeconds(3);

        protected override async Task<ModuleResult<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            return "Foo bar!";
        }
    }

    [Test]
    public void Throws_Timeout_Exception()
    {
        Assert.ThrowsAsync<TaskCanceledException>(RunModule<Module>);
    }
}
