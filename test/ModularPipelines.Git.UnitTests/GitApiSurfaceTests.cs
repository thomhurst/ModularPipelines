namespace ModularPipelines.Git.UnitTests;

public class GitApiSurfaceTests
{
    [Test]
    public async Task Package_Exports_Only_The_Grouped_Git_Facade()
    {
        var exportedGitInterfaces = typeof(IGit).Assembly
            .GetExportedTypes()
            .Where(type => type.Name == nameof(IGit));

        await Assert.That(exportedGitInterfaces).IsEquivalentTo([typeof(IGit)]);
    }
}
