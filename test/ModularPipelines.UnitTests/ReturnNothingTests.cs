using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class ReturnNothingTests : TestBase
{
    private class ReturnNothingModule1 : Module<CommandResult>
    {
        public override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    private class ReturnNothingModule2 : Module<CommandResult>
    {
        public override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    private class ReturnNothingModule3 : Module<CommandResult>
    {
        public override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return default;
        }
    }

    [Test]
    public async Task Module1_HasValue_False()
    {
        var result = await RunModuleWithResult<ReturnNothingModule1, CommandResult>();

        await Assert(result);
    }

    [Test]
    public async Task Module2_HasValue_False()
    {
        var result = await RunModuleWithResult<ReturnNothingModule2, CommandResult>();

        await Assert(result);
    }

    [Test]
    public async Task Module3_HasValue_False()
    {
        var result = await RunModuleWithResult<ReturnNothingModule3, CommandResult>();

        await Assert(result);
    }

    private static async Task Assert(ModuleResult<CommandResult> result)
    {
        using (TUnit.Assertions.Assert.Multiple())
        {
            await TUnit.Assertions.Assert.That(result.HasValue).IsFalse();
            await TUnit.Assertions.Assert.That(result.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await TUnit.Assertions.Assert.That(result.Value).IsNull();
            await TUnit.Assertions.Assert.That(result.Exception).IsNull();
        }
    }
}
