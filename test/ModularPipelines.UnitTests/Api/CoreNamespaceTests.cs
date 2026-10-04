namespace ModularPipelines.UnitTests.Api;

public class CoreNamespaceTests
{
    [Test]
    [Arguments(typeof(ModularPipelines.NotInParallelAttribute), "ModularPipelines")]
    [Arguments(typeof(ModularPipelines.PriorityAttribute), "ModularPipelines")]
    [Arguments(typeof(ModularPipelines.ExecutionHintAttribute), "ModularPipelines")]
    [Arguments(typeof(ModularPipelines.ExecutionHint), "ModularPipelines")]
    [Arguments(typeof(ModularPipelines.BuildSystem), "ModularPipelines")]
    [Arguments(typeof(ModularPipelines.PipelineSummary), "ModularPipelines")]
    [Arguments(typeof(ModularPipelines.IModuleResult), "ModularPipelines")]
    [Arguments(typeof(ModularPipelines.IParallelLimit), "ModularPipelines")]
    [Arguments(typeof(ModularPipelines.CommandExtensions), "ModularPipelines")]
    [Arguments(typeof(ModularPipelines.EnumerableExtensions), "ModularPipelines")]
    [Arguments(typeof(ModularPipelines.FileSystem.FileExtensions), "ModularPipelines.FileSystem")]
    [Arguments(typeof(ModularPipelines.FileSystem.FolderExtensions), "ModularPipelines.FileSystem")]
    [Arguments(typeof(ModularPipelines.Reporting.DependencyGraphFormat), "ModularPipelines.Reporting")]
    public async Task Public_Types_Use_Module_Or_Feature_Namespaces(Type type, string expectedNamespace)
    {
        await Assert.That(type.Namespace).IsEqualTo(expectedNamespace);
    }

    [Test]
    [Arguments(typeof(ModularPipelines.NotInParallelAttribute))]
    [Arguments(typeof(ModularPipelines.PriorityAttribute))]
    [Arguments(typeof(ModularPipelines.ExecutionHintAttribute))]
    public async Task Module_Attributes_Are_Sealed(Type type)
    {
        await Assert.That(type.IsSealed).IsTrue();
    }
}
