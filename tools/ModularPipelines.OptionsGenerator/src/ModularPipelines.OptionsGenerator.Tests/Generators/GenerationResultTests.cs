using ModularPipelines.OptionsGenerator.Generators;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class GenerationResultTests
{
    [Test]
    public async Task ChangedPaths_Preserves_Both_Sides_Of_Case_Only_Renames()
    {
        var result = new GenerationResult();
        result.FilesGenerated.Add("src/Fake/Options/FakeChangeSetOptions.Generated.cs");
        result.FilesDeleted.Add("src/Fake/Options/FakeChangesetOptions.Generated.cs");

        await Assert.That(result.ChangedPaths).IsEquivalentTo([
            "src/Fake/Options/FakeChangeSetOptions.Generated.cs",
            "src/Fake/Options/FakeChangesetOptions.Generated.cs",
        ]);
    }
}
