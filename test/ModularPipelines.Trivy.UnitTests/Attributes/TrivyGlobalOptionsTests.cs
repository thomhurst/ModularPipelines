using System.Reflection;
using ModularPipelines.TestHelpers;
using ModularPipelines.Trivy.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Trivy.UnitTests.Attributes;

public class TrivyGlobalOptionsTests : TestBase
{
    [Test]
    [Arguments(true, "trivy --debug --quiet --timeout=0s plugin install fixture-plugin")]
    [Arguments(false, "trivy --timeout=0s plugin install fixture-plugin")]
    [Arguments(null, "trivy --timeout=0s plugin install fixture-plugin")]
    public async Task Inherited_Flags_Precede_Nested_Command(bool? enabled, string expected)
    {
        var command = await RenderCommand(new TrivyPluginInstallOptions("fixture-plugin")
        {
            Debug = enabled,
            Quiet = enabled,
            Timeout = "0s",
        });
        await Assert.That(command).IsEqualTo(expected);
    }

    [Test]
    public async Task Shared_Paths_And_Repeated_Local_Values_Keep_Argument_Boundaries()
    {
        var arguments = BuildArguments(new TrivyRegistryLoginOptions("registry.example.com")
        {
            Config = "config directory/trivy.yaml",
            Cacert = "cert directory/root.pem",
            Password = ["first secret", "second secret"],
            Username = ["first", "second"],
        });
        await AssertArguments(arguments,
        [
            "registry.example.com",
            "--password=first secret",
            "--password=second secret",
            "--username=first",
            "--username=second",
            "--cacert=cert directory/root.pem",
            "--config=config directory/trivy.yaml",
        ]);
    }

    [Test]
    public async Task Every_Command_Inherits_Persistent_Settings_Exactly_Once()
    {
        string[] expected = ["Cacert", "CacheDir", "Config", "Debug", "GenerateDefaultConfig", "Insecure", "Quiet", "Timeout"];
        await Assert.That(typeof(TrivyOptions)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Select(property => property.Name))
            .IsEquivalentTo(expected);

        foreach (var type in typeof(TrivyOptions).Assembly.GetTypes()
                     .Where(type => !type.IsAbstract && typeof(TrivyOptions).IsAssignableFrom(type)))
        {
            foreach (var name in expected)
            {
                await Assert.That(type.GetProperties().Single(property => property.Name == name).DeclaringType)
                    .IsEqualTo(typeof(TrivyOptions));
            }
        }
    }
}
