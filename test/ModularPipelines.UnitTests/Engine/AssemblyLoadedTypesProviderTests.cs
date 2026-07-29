using ModularPipelines.Engine;

namespace ModularPipelines.UnitTests.Engine;

public class AssemblyLoadedTypesProviderTests
{
    [Test]
    public async Task ReferencesModularPipelines_IncludesCoreAndReferencingAssemblies()
    {
        await Assert.That(AssemblyLoadedTypesProvider.ReferencesModularPipelines(typeof(PipelineBuilder).Assembly))
            .IsTrue();
        await Assert.That(AssemblyLoadedTypesProvider.ReferencesModularPipelines(GetType().Assembly))
            .IsTrue();
    }

    [Test]
    public async Task ReferencesModularPipelines_ExcludesUnrelatedAssemblies()
    {
        await Assert.That(AssemblyLoadedTypesProvider.ReferencesModularPipelines(typeof(string).Assembly))
            .IsFalse();
    }
}
