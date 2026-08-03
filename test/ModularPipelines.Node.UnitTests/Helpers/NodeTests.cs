using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Node.Extensions;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;

namespace ModularPipelines.Node.UnitTests.Helpers;

public class NodeTests : TestBase
{
    private class NodeVersionModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Node().VersionAsync(cancellationToken: cancellationToken);
        }
    }

    [Test]
    [RequiresTool("node")]
    public async Task Has_Not_Errored()
    {
        var moduleResult = await await RunModule<NodeVersionModule>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
    }

    [Test]
    [RequiresTool("node")]
    public async Task Standard_Output_Is_Version_Number()
    {
        var moduleResult = await await RunModule<NodeVersionModule>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ValueOrDefault!.StandardError).IsNull().Or.IsEmpty();
            await Assert.That(moduleResult.ValueOrDefault.StandardOutput).Matches(@"v\d+");
        }
    }

    [Test]
    public async Task Command_Methods_Are_Asynchronous()
    {
        var commandMethods = new[] { typeof(INode), typeof(INpm), typeof(INpx), typeof(INvm) }
            .SelectMany(type => type.GetMethods())
            .Where(method => method.ReturnType == typeof(Task<CommandResult>))
            .ToList();

        await Assert.That(commandMethods).IsNotEmpty();
        await Assert.That(commandMethods.All(method =>
            method.Name.EndsWith("Async", StringComparison.Ordinal))).IsTrue();
    }
}
