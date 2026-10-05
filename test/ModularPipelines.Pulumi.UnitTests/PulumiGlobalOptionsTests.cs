using System.Reflection;
using ModularPipelines.Pulumi.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Pulumi.UnitTests;

public class PulumiGlobalOptionsTests : TestBase
{
    [Test]
    public async Task Root_Settings_Precede_Nested_Command_And_Local_Options()
    {
        var command = await RenderCommand(new PulumiStackListOptions
        {
            Color = "never",
            Cwd = "infra project",
            Emoji = false,
            NonInteractive = true,
            Verbose = 0,
            Output = "json",
            Project = "production",
        });
        await Assert.That(command).IsEqualTo(
            "pulumi --color=never --cwd=infra project --emoji=false --non-interactive --verbose=0 stack list --output=json --project=production");
    }

    [Test]
    [Arguments(true, " --emoji=true")]
    [Arguments(false, " --emoji=false")]
    [Arguments(null, "")]
    public async Task Emoji_Preserves_Explicit_Boolean_Values(bool? emoji, string rendered)
    {
        var command = await RenderCommand(new PulumiStackListOptions { Emoji = emoji, NonInteractive = false });
        await Assert.That(command).IsEqualTo($"pulumi{rendered} stack list");
    }

    [Test]
    public async Task Environment_Scope_And_Passthrough_Remain_After_Command_Path()
    {
        var command = await RenderCommand(new PulumiEnvRunOptions("development", "echo")
        {
            Cwd = "infra",
            NonInteractive = true,
            Env = "team/development",
            Args = ["hello", "--color=raw"],
        });
        await Assert.That(command).IsEqualTo(
            "pulumi --cwd=infra --non-interactive env run development --env=team/development -- echo hello --color=raw");
    }

    [Test]
    public async Task Global_Values_Preserve_Token_Boundaries()
    {
        var arguments = OptionsRenderingTestHelper.BuildArguments(new PulumiStackListOptions
        {
            Cwd = "infra project",
            OtelTraces = "file://trace output.json",
        });
        await OptionsRenderingTestHelper.AssertArguments(arguments,
            ["--cwd=infra project", "--otel-traces=file://trace output.json"]);
    }

    [Test]
    public async Task Every_Command_Inherits_Exactly_One_Copy_Of_Each_Root_Setting()
    {
        string[] names =
        [
            "Color", "Cwd", "DisableIntegrityChecking", "Emoji", "FullyQualifyStackNames", "Logflow",
            "Logtostderr", "Memprofilerate", "NonInteractive", "OtelTraces", "Profiling", "Tracing", "Verbose",
        ];
        var types = typeof(PulumiOptions).Assembly.GetTypes()
            .Where(type => !type.IsAbstract && type.IsAssignableTo(typeof(PulumiOptions)));
        await Assert.That(types).IsNotEmpty();
        foreach (var type in types)
        {
            foreach (var name in names)
            {
                var properties = type.GetProperties().Where(property => property.Name == name).ToArray();
                await Assert.That(properties).Count().IsEqualTo(1);
                await Assert.That(properties.Single().DeclaringType).IsEqualTo(typeof(PulumiOptions));
                await Assert.That(type.GetProperty(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)).IsNull();
            }
        }
    }

    [Test]
    public async Task Overridden_Global_Is_Emitted_Once()
    {
        var command = await RenderCommand(new CustomListOptions { Emoji = false });
        await Assert.That(command).IsEqualTo("pulumi --emoji=false stack list");
    }

    private record CustomListOptions : PulumiStackListOptions
    {
        public override bool? Emoji { get; set; }
    }
}
