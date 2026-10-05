using System.Reflection;
using ModularPipelines.Grype.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Grype.UnitTests;

public class GlobalOptionsTests : TestBase
{
    [Test]
    public async Task Persistent_Settings_Precede_Nested_Command_And_Keep_Value_Boundaries()
    {
        var command = await RenderCommand(new GrypeDbStatusOptions
        {
            Config = ["base config.yaml", "production.yaml"],
            Profile = ["one", "two,three"],
            Quiet = true,
            Verbose = 2,
            Output = "json",
        });

        await Assert.That(command).IsEqualTo(
            "grype --config=base config.yaml --config=production.yaml --profile=one --profile=two,three --quiet --verbose=2 db status --output=json");
    }

    [Test]
    [Arguments(false)]
    [Arguments(null)]
    public async Task Unset_And_False_Flags_Are_Omitted_But_Zero_Count_Is_Preserved(bool? quiet)
    {
        var command = await RenderCommand(new GrypeDbStatusOptions { Quiet = quiet, Verbose = 0 });
        await Assert.That(command).IsEqualTo("grype --verbose=0 db status");
    }

    [Test]
    public async Task Globals_Precede_Operands_And_Local_Options()
    {
        var command = await RenderCommand(new GrypeDbSearchVulnOptions(["CVE-2026-1234", "CVE-2026-5678"])
        {
            Quiet = true,
            Limit = 0,
            Output = "json",
        });
        await Assert.That(command)
            .IsEqualTo("grype --quiet db search vuln CVE-2026-1234 CVE-2026-5678 --limit=0 --output=json");
    }

    [Test]
    public async Task Every_Command_Inherits_One_Copy_Of_Each_Setting()
    {
        string[] names = ["Config", "Profile", "Quiet", "Verbose"];
        var types = typeof(GrypeOptions).Assembly.GetTypes()
            .Where(type => !type.IsAbstract && type.IsAssignableTo(typeof(GrypeOptions))).ToArray();
        await Assert.That(types).IsNotEmpty();
        foreach (var type in types)
        {
            foreach (var name in names)
            {
                var property = type.GetProperties().Single(property => property.Name == name);
                await Assert.That(property.DeclaringType).IsEqualTo(typeof(GrypeOptions));
                await Assert.That(type.GetProperty(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)).IsNull();
            }
        }
    }

    [Test]
    public async Task Overridden_Global_Is_Emitted_Once()
    {
        var command = await RenderCommand(new CustomStatusOptions { Quiet = true });
        await Assert.That(command).IsEqualTo("grype --quiet db status");
    }

    [Test]
    public async Task Repeated_Values_Preserve_Token_Boundaries()
    {
        var arguments = OptionsRenderingTestHelper.BuildArguments(new GrypeDbStatusOptions
        {
            Config = ["base config.yaml", "production.yaml"],
            Profile = ["one", "two,three"],
        });

        await Assert.That(arguments).IsEquivalentTo([
            "--config=base config.yaml", "--config=production.yaml", "--profile=one", "--profile=two,three"]);
    }

    private record CustomStatusOptions : GrypeDbStatusOptions
    {
        public override bool? Quiet { get; set; }
    }
}
