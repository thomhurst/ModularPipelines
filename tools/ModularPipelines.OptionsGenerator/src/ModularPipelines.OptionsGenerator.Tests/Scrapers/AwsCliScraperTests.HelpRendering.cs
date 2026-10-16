using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public partial class AwsCliScraperTests
{
    [Test]
    [Arguments("       o describe-local-gateway-route-table-virtual-interface-group-associa-\n         tions")]
    [Arguments("       * describe-local-gateway-route-table-virtual-interface-group-\n         associations")]
    [Arguments("       describe-local-gateway-route-table-virtual-interface-group-associa-")]
    public async Task Wrapped_Command_Names_Stop_Generation_Instead_Of_Publishing_Fragments(string commands)
    {
        var scraper = new AwsHelpRenderingScraper();
        await Assert.That(() => scraper.Subcommands($"AVAILABLE COMMANDS\n{commands}\n"))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Complete_Long_Command_Preserves_Tokens_And_Generated_Metadata()
    {
        const string name = "describe-local-gateway-route-table-virtual-interface-group-associations";
        var scraper = new AwsHelpRenderingScraper();
        await Assert.That(scraper.Subcommands($"AVAILABLE COMMANDS\n       o {name}\n       o describe-instances\n"))
            .IsEquivalentTo([name, "describe-instances"]);

        var parser = new TestAwsCliScraper();
        var command = (await parser.Parse(["aws", "ec2", name], "DESCRIPTION\n        Describes associations.\n\nOPTIONS\n"))!;
        var output = await new OptionsClassGenerator().GenerateAsync(parser.CreateToolDefinition() with { Commands = [command] });
        await Assert.That(command.DocumentationUrl).Contains($"ec2/{name}/index.html");
        await Assert.That(output.Single().Content).Contains($"[CliSubCommand(\"ec2\", \"{name}\")]");
        await Assert.That(output.Single().Content).Contains("AwsEc2DescribeLocalGatewayRouteTableVirtualInterfaceGroupAssociationsOptions");
    }

    [Test]
    public async Task Global_Descriptions_Render_Structural_Bullets_Without_Changing_Enum_Values()
    {
        const string help = """
            GLOBAL OPTIONS
                   --cli-binary-format (string)
                   The formatting style for binary blobs. Possible values:

                   o base64

                   o raw-in-base64-out

                   Overrides compatibility settings for direct-ConnectGatewayId.
            """;
        var option = new AwsHelpRenderingScraper().Globals(help).Single();
        await Assert.That(option.Description).Contains("\u2022 base64 \u2022 raw-in-base64-out");
        await Assert.That(option.Description).Contains("direct-ConnectGatewayId");
        await Assert.That(option.EnumDefinition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["base64", "raw-in-base64-out"]);
        var tool = new AwsHelpRenderingScraper().CreateToolDefinition() with
        {
            GlobalOptions = [option],
            DocumentationOutputDirectory = "docs",
        };
        var documentation = await new MarkdownDocumentationGenerator().GenerateAsync(tool);
        await Assert.That(documentation.Single().Content).Contains("\u2022 base64 \u2022 raw-in-base64-out");
    }

    private sealed class AwsHelpRenderingScraper()
        : AwsCliScraper(new AwsFixtureExecutor(string.Empty),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<AwsCliScraper>.Instance)
    {
        public IEnumerable<string> Subcommands(string help) => ExtractSubcommands(help);

        public IReadOnlyList<CliOptionDefinition> Globals(string help) =>
            ParseGlobalOptions(help);
    }
}
