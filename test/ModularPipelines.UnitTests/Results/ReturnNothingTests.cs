using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Results;

public class ReturnNothingTests : TestBase
{
    private class ReturnNothingModule1 : SimpleTestModule<None>
    {
        protected override None Result => None.Value;
    }

    private class ReturnNothingModule2 : SimpleTestModule<None>
    {
        protected override None Result => None.Value;
    }

    private class ReturnNothingModule3 : SimpleTestModule<None>
    {
        protected override None Result => default;
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

    private static async Task Assert(ModuleResult<None> result)
    {
        using (TUnit.Assertions.Assert.Multiple())
        {
            await TUnit.Assertions.Assert.That(result.ModuleStatus).IsEqualTo(Status.Successful);
            await TUnit.Assertions.Assert.That(result.Value).IsEqualTo(None.Value);
            await TUnit.Assertions.Assert.That(result.ExceptionOrDefault).IsNull();
        }
    }
}
