using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class ReturnNothingTests : TestBase
{
    private class ReturnNothingModule1 : Module<CommandResult>
    {
        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    private class ReturnNothingModule2 : Module<CommandResult>
    {
        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await NothingAsync();
        }
    }

    private class ReturnNothingModule3 : Module<CommandResult>
    {
        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return default;
        }
    }

    [Test]
    public async Task Module1_HasValue_False()
    {
        var module = await RunModule<ReturnNothingModule1>();

        var result = await module;

        Assert(result);
    }

    [Test]
    public async Task Module2_HasValue_False()
    {
        var module = await RunModule<ReturnNothingModule2>();

        var result = await module;

        Assert(result);
    }

    [Test]
    public async Task Module3_HasValue_False()
    {
        var module = await RunModule<ReturnNothingModule3>();

        var result = await module;

        Assert(result);
    }

    private static void Assert(ModuleResult<CommandResult> result)
    {
        TUnit.Assertions.Assert.Multiple(() =>
        {
            TUnit.Assertions.Assert.That(result.HasValue).Is.False();
            TUnit.Assertions.Assert.That(result.ModuleResultType).Is.EqualTo(ModuleResultType.Success);
            TUnit.Assertions.Assert.That(result.Value).Is.Null();
            TUnit.Assertions.Assert.That(result.Exception).Is.Null();
        });
    }
}