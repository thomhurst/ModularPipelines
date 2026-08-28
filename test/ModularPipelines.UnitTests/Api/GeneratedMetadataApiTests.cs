using System.ComponentModel;
using System.Reflection;
using ModularPipelines.Generated;

namespace ModularPipelines.UnitTests.Api;

public class GeneratedMetadataApiTests
{
    [Test]
    public async Task GeneratorContractsShareHiddenGeneratedNamespace()
    {
        var contractTypes = new[]
        {
            typeof(GeneratedModuleMetadata),
            typeof(GeneratedModuleRegistration),
            typeof(ModuleDependencyMetadata),
            typeof(GeneratedSecretMetadata),
            typeof(SecretPropertyAccessor),
            typeof(GeneratedModuleEventMetadata),
            typeof(GeneratedCommandMetadata),
            typeof(PropertyCommandLinePart),
            typeof(ArgumentPart),
            typeof(FlagPart),
            typeof(OptionPart),
            typeof(RuntimeMetadataRegistry),
            typeof(IncompleteRuntimeMetadataAttribute),
        };
        var generatedTypes = typeof(RuntimeMetadataRegistry).Assembly.ExportedTypes
            .Where(type => type.Namespace == "ModularPipelines.Generated")
            .ToArray();

        await Assert.That(generatedTypes).IsEquivalentTo(contractTypes);

        foreach (var type in generatedTypes)
        {
            var editorBrowsable = type.GetCustomAttribute<EditorBrowsableAttribute>();

            await Assert.That(type.Namespace).IsEqualTo("ModularPipelines.Generated");
            await Assert.That(editorBrowsable).IsNotNull();
            await Assert.That(editorBrowsable!.State).IsEqualTo(EditorBrowsableState.Never);
        }
    }

    [Test]
    public async Task CommandProviderInterfacesAreInternal()
    {
        var assembly = typeof(RuntimeMetadataRegistry).Assembly;
        var interfaceNames = new[]
        {
            "ModularPipelines.Helpers.Internal.ICommandArgumentBuilder",
            "ModularPipelines.Helpers.Internal.ICommandModelProvider",
            "ModularPipelines.Helpers.Internal.ICommandPartsProvider",
        };

        foreach (var interfaceName in interfaceNames)
        {
            var type = assembly.GetType(interfaceName);
            await Assert.That(type).IsNotNull();
            await Assert.That(type!.IsNotPublic).IsTrue();
        }

        await Assert.That(assembly.ExportedTypes)
            .DoesNotContain(type => type.Namespace == "ModularPipelines.Helpers.Internal");
    }
}
