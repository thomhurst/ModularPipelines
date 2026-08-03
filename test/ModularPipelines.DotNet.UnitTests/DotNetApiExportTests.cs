using ModularPipelines.DotNet.Services;
using ModularPipelines.Models;

namespace ModularPipelines.DotNet.UnitTests;

public class DotNetApiExportTests
{
    [Test]
    public async Task Assembly_ExportsOnlyActiveIDotNetInterface()
    {
        var exportedInterfaces = typeof(IDotNet).Assembly
            .GetExportedTypes()
            .Where(type => type.Name == nameof(IDotNet))
            .ToArray();

        await Assert.That(exportedInterfaces).IsEquivalentTo([typeof(IDotNet)]);
    }

    [Test]
    public async Task Assemblies_DoNotExportRemovedCommandBuilderTypes()
    {
        var coreAssembly = typeof(CommandResult).Assembly;
        var dotNetAssembly = typeof(IDotNet).Assembly;
        var removedTypes = new[]
        {
            coreAssembly.GetType("ModularPipelines.Builders.ICommandBuilder`2"),
            coreAssembly.GetType("ModularPipelines.Builders.CommandBuilderBase`2"),
            dotNetAssembly.GetType("ModularPipelines.DotNet.Builders.IDotNetBuildBuilder"),
            dotNetAssembly.GetType("ModularPipelines.DotNet.Builders.DotNetBuildBuilder"),
        };

        await Assert.That(removedTypes.Where(static type => type is not null)).IsEmpty();
    }
}
