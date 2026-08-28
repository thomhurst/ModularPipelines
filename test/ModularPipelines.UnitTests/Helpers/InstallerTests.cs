using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class InstallerTests : TestBase
{
    [Test]
    public async Task Public_Command_Methods_Are_Asynchronous()
    {
        var commandMethods = typeof(IModuleContext).Assembly.ExportedTypes
            .Where(type => type.IsInterface)
            .SelectMany(type => type.GetMethods())
            .Where(method => method.ReturnType == typeof(Task<CommandResult>))
            .ToList();

        await Assert.That(commandMethods).IsNotEmpty();
        await Assert.That(commandMethods.All(method =>
            method.Name.EndsWith("Async", StringComparison.Ordinal))).IsTrue();
    }
}
