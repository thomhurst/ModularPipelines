using System.Reflection;
using System.Reflection.Emit;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;

namespace ModularPipelines.UnitTests.Plugins;

public class PluginVersionValidatorTests
{
    [Test]
    public async Task Validate_WithCurrentGeneratedMetadata_DoesNotThrow()
    {
        var assembly = typeof(PluginVersionValidatorTests).Assembly;

        await Assert.That(() => PluginVersionValidator.Validate(assembly, new Version(5, 0, 0)))
            .ThrowsNothing();
    }

    [Test]
    public async Task Validate_WithoutAttributeOrMetadata_DoesNotTreatAssemblyAsPlugin()
    {
        var assembly = typeof(object).Assembly;

        await Assert.That(() => PluginVersionValidator.Validate(assembly, new Version(5, 0, 0)))
            .ThrowsNothing();
    }

    [Test]
    public async Task IsCompatible_WithoutAttributeOrMetadata_ReturnsTrue()
    {
        var assembly = typeof(object).Assembly;

        var result = PluginVersionValidator.IsCompatible(assembly, new Version(5, 0, 0));

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Validate_WithStaleGeneratedMetadata_Throws()
    {
        var assembly = CreateAssemblyWithMetadataSchemas(1, 3);

        await Assert.That(() => PluginVersionValidator.Validate(assembly, new Version(5, 0, 0)))
            .Throws<PluginInitializationException>()
            .WithMessageContaining("runtime metadata schemas 1/3");
    }

    private static Assembly CreateAssemblyWithMetadataSchemas(
        int runtimeSchemaVersion,
        int commandSchemaVersion)
    {
        var assembly = AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName($"Plugin_{Guid.NewGuid():N}"),
            AssemblyBuilderAccess.Run);
        var module = assembly.DefineDynamicModule("Plugin");
        var registration = module.DefineType(
            "ModularPipelines.Generated.RuntimeMetadataRegistration",
            TypeAttributes.NotPublic | TypeAttributes.Abstract | TypeAttributes.Sealed);
        registration.DefineField(
                "SchemaVersion",
                typeof(int),
                FieldAttributes.Assembly | FieldAttributes.Static | FieldAttributes.Literal)
            .SetConstant(runtimeSchemaVersion);
        registration.DefineField(
                "CommandSchemaVersion",
                typeof(int),
                FieldAttributes.Assembly | FieldAttributes.Static | FieldAttributes.Literal)
            .SetConstant(commandSchemaVersion);
        _ = registration.CreateType();
        return assembly;
    }
}
