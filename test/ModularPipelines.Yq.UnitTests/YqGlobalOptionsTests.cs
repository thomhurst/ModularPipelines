using ModularPipelines.TestHelpers;
using ModularPipelines.Yq.Options;

namespace ModularPipelines.Yq.UnitTests;

public class YqGlobalOptionsTests : TestBase
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Globals_Render_Once_Before_The_Command_And_Operands(bool all)
    {
        YqOptions options = all
            ? new YqEvalAllOptions { ExpressionArgument = ".items[]", YamlFile1 = ["input.yaml"] }
            : new YqEvalOptions { ExpressionArgument = ".items[]", YamlFile1 = ["input.yaml"] };
        options.OutputFormat = "json";
        options.UnwrapScalar = false;
        options.Indent = 2;
        options.Colors = true;
        options.YamlCompactSeqIndent = true;

        var rendered = await RenderCommand(options);
        var command = all ? "eval-all" : "eval";
        var commandIndex = rendered.IndexOf($" {command} ", StringComparison.Ordinal);
        foreach (var setting in new[] { "--output-format=json", "--unwrapScalar=false", "--indent=2", "--colors", "--yaml-compact-seq-indent" })
        {
            var index = rendered.IndexOf(setting, StringComparison.Ordinal);
            await Assert.That(index).IsGreaterThanOrEqualTo(0);
            await Assert.That(index).IsLessThan(commandIndex);
            await Assert.That(rendered.LastIndexOf(setting, StringComparison.Ordinal)).IsEqualTo(index);
        }
        await Assert.That(rendered).EndsWith($"{command} .items[] input.yaml");
    }

    [Test]
    public async Task Dash_Expression_Gets_A_Terminator_After_Global_Settings()
    {
        var rendered = await RenderCommand(new YqEvalOptions
        {
            NullInput = true,
            ExpressionArgument = "-1",
        });
        await Assert.That(rendered).IsEqualTo("yq --null-input eval -- -1");
    }

    [Test]
    public async Task Overridden_Setting_Renders_Once()
    {
        var rendered = await RenderCommand(new JsonEvalOptions { ExpressionArgument = "." });
        await Assert.That(rendered).IsEqualTo("yq --output-format=json eval .");
    }

    private record JsonEvalOptions : YqEvalOptions
    {
        public override string? OutputFormat { get; set; } = "json";
    }
}
