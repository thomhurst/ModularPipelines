using ModularPipelines.Models;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class ReturnNothingTests : TestBase
{
    private class ReturnNothingModule1 : SimpleTestModule<CommandResult>
    {
        protected override CommandResult? Result => null;
    }

    private class ReturnNothingModule2 : SimpleTestModule<CommandResult>
    {
        protected override CommandResult? Result => null;
    }

    private class ReturnNothingModule3 : SimpleTestModule<CommandResult>
    {
        protected override CommandResult? Result => default;
    }

    [Test]
    public async Task Module1_HasValue_False()
    {
        var result = await await RunModule<ReturnNothingModule1>();

        await Assert(result);
    }

    [Test]
    public async Task Module2_HasValue_False()
    {
        var result = await await RunModule<ReturnNothingModule2>();

        await Assert(result);
    }

    [Test]
    public async Task Module3_HasValue_False()
    {
        var result = await await RunModule<ReturnNothingModule3>();

        await Assert(result);
    }

    private static async Task Assert(ModuleResult<CommandResult?> result)
    {
        using (TUnit.Assertions.Assert.Multiple())
        {
            await TUnit.Assertions.Assert.That(result.IsSuccess).IsTrue();
            await TUnit.Assertions.Assert.That(result.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await TUnit.Assertions.Assert.That(result.ValueOrDefault).IsNull();
            await TUnit.Assertions.Assert.That(result.ExceptionOrDefault).IsNull();
        }
    }
}
