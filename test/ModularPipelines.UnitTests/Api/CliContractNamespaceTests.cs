namespace ModularPipelines.UnitTests.Api;

public class CliContractNamespaceTests
{
    [Test]
    [Arguments("CommandLine")]
    [Arguments("KeyValue")]
    [Arguments("CliOptionValue")]
    [Arguments("CliValuePair")]
    [Arguments("CliValueGroup")]
    [Arguments("CliOptionValueRangeAttribute")]
    [Arguments("CliOptionValueRegularExpressionAttribute")]
    public async Task CliContractsAreOnlyExposedInTheRootNamespace(string name)
    {
        var assembly = typeof(Module<>).Assembly;

        await Assert.That(assembly.GetType($"ModularPipelines.{name}")?.IsPublic).IsTrue();
        await Assert.That(assembly.GetType($"ModularPipelines.Models.{name}")).IsNull();
    }

    [Test]
    public async Task ModelsNamespaceContainsOnlyInternalImplementationTypes()
    {
        var assembly = typeof(Module<>).Assembly;

        await Assert.That(assembly.ExportedTypes)
            .DoesNotContain(type => type.Namespace == "ModularPipelines.Models");
        // Generated imports still require Models to exist; this internal record anchors that namespace.
        await Assert.That(assembly.GetType("ModularPipelines.Models.DeclaredDependency")).IsNotNull();
    }
}
