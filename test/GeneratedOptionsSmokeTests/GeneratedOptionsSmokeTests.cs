using System.Reflection;
using ModularPipelines.TestHelpers;
using ReflectionAssembly = System.Reflection.Assembly;

namespace ModularPipelines.GeneratedOptions.UnitTests;

public class GeneratedOptionsSmokeTests
{
    [Test]
    public async Task Every_Options_Record_Renders_Expected_Arguments()
    {
        var assemblyName = ReflectionAssembly.GetExecutingAssembly()
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(attribute => attribute.Key == "GeneratedOptionsAssembly")
            .Value!;
        var result = GeneratedOptionsSmokeTestHarness.ValidateAssembly(ReflectionAssembly.Load(assemblyName));

        await Assert.That(result.OptionsTypesTested).IsGreaterThan(0);
    }
}
