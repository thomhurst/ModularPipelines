using System.Reflection;
using ModularPipelines.Eksctl.Options;
using ModularPipelines.TestHelpers;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Eksctl.UnitTests.Attributes;

public class EksctlGlobalOptionsTests : TestBase
{
    [Test]
    [Arguments(true, "eksctl --color=false --dumpLogs --verbose=0 get cluster --region=eu-west-2 --profile=production")]
    [Arguments(false, "eksctl --color=false --verbose=0 get cluster --region=eu-west-2 --profile=production")]
    [Arguments(null, "eksctl --color=false --verbose=0 get cluster --region=eu-west-2 --profile=production")]
    public async Task Inherited_Settings_Precede_Command_And_Aws_Settings_Remain_Local(bool? dumpLogs, string expected)
    {
        var command = await RenderCommand(new EksctlGetClusterOptions
        {
            Color = "false",
            Dumplogs = dumpLogs,
            Verbose = 0,
            Region = "eu-west-2",
            Profile = "production",
        });
        await Assert.That(command).IsEqualTo(expected);
    }

    [Test]
    public async Task Repeated_Local_Values_Keep_Their_Argument_Boundaries()
    {
        var arguments = BuildArguments(new EksctlCreateClusterOptions
        {
            Color = "fabulous",
            Verbose = 4,
            Name = "test-cluster",
            Zones = ["eu-west-2a", "eu-west-2b"],
        });
        await AssertArguments(arguments,
        [
            "--name=test-cluster", "--zones=eu-west-2a", "--zones=eu-west-2b",
            "--color=fabulous", "--verbose=4",
        ]);
    }

    [Test]
    public async Task Every_Command_Inherits_Persistent_Settings_Exactly_Once()
    {
        string[] expected = ["Color", "Dumplogs", "Verbose"];
        await Assert.That(typeof(EksctlOptions)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Select(property => property.Name))
            .IsEquivalentTo(expected);
        foreach (var type in typeof(EksctlOptions).Assembly.GetTypes()
                     .Where(type => !type.IsAbstract && typeof(EksctlOptions).IsAssignableFrom(type)))
        {
            foreach (var name in expected)
            {
                await Assert.That(type.GetProperties().Single(property => property.Name == name).DeclaringType)
                    .IsEqualTo(typeof(EksctlOptions));
            }
        }
    }
}
