using ModularPipelines.DotNet.Services;

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
}
