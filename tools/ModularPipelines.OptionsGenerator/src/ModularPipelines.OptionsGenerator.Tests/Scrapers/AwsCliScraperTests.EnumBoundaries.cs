using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public partial class AwsCliScraperTests
{
    [Test]
    [Arguments("This attribute has two possible values: o Topic The scope of message deduplication is across the entire topic.")]
    [Arguments("The name of an attribute.\n    o FifoThroughputScope\n        Possible values: Topic MessageGroup")]
    [Arguments("The name of an attribute.\n    o FifoThroughputScope. Possible values: Topic MessageGroup")]
    [Arguments("The name of an attribute.\no FifoThroughputScope. Possible values: Topic MessageGroup")]
    public async Task Enum_Detection_Rejects_Nested_Attribute_Values(string description)
    {
        await Assert.That(AwsCliScraper.TryDetectEnum("AttributeName", "AwsSnsSetTopicAttributesOptions", description)).IsNull();
    }

    [Test]
    [Arguments("o", "PRE_APPROVED")]
    [Arguments("*", "PRE_APPROVED")]
    [Arguments("o", "ipsec.1")]
    public async Task Single_Bullet_Value_Uses_Shared_Single_Member_Fallback(string bullet, string value)
    {
        await Assert.That(AwsCliScraper.TryDetectEnum("AuthorizationBehavior", "AwsAcmCreateAcmeEndpointOptions",
            $"Possible values: {bullet} {value}")).IsNull();
    }

    [Test]
    [Arguments("The option mode. Possible values: active inactive")]
    [Arguments("The option mode.\nPossible values: active inactive")]
    [Arguments("        The option mode.\n\n        Possible values: active inactive")]
    public async Task Option_Level_Enum_Headings_Support_Inline_And_Separate_Paragraphs(string description)
    {
        var definition = AwsCliScraper.TryDetectEnum("Mode", "AwsExampleOptions", description);
        await Assert.That(definition).IsNotNull();
        await Assert.That(definition!.Values.Select(value => value.CliValue)).IsEquivalentTo(["active", "inactive"]);
    }

    [Test]
    [Arguments("o")]
    [Arguments("*")]
    public async Task Top_Level_Bullet_Values_Preserve_All_Choices(string bullet)
    {
        var definition = AwsCliScraper.TryDetectEnum("Contact", "AwsAcmCreateAcmeEndpointOptions",
            $"        Specifies whether contact information is required.\n        Possible values:\n\n        {bullet} REQUIRED\n\n        {bullet} NOT_REQUIRED");
        await Assert.That(definition).IsNotNull();
        await Assert.That(definition!.Values.Select(value => value.CliValue)).IsEquivalentTo(["REQUIRED", "NOT_REQUIRED"]);
    }

    [Test]
    public async Task Sns_Attribute_Name_Remains_A_Required_String()
    {
        // Excerpt from `aws sns set-topic-attributes help` (2.32.28), also present
        // in the 2.36.46 snapshot reviewed in #5163. Indentation matches man output.
        const string help = """
            SYNOPSIS
                   set-topic-attributes
                   --topic-arn <value>
                   --attribute-name <value>
                   [--attribute-value <value>]
            OPTIONS
                   --topic-arn (string)
                      The ARN of the topic to modify.
                   --attribute-name (string)
                      The name of the attribute you want to set.

                      o FifoThroughputScope Enables higher throughput for your FIFO
                        topic by adjusting the scope of deduplication. This attribute has
                        two possible values:

                        o Topic The scope of message deduplication is across the
                          entire topic. This is the default value.

                        o MessageGroup The scope of deduplication is within each
                          individual message group.
                   --attribute-value (string)
                      The new value for the attribute.
            """;
        var scraper = new TestAwsCliScraper();
        var command = (await scraper.Parse(["aws", "sns", "set-topic-attributes"], help))!;
        var attributeName = command.Options.Single(option => option.SwitchName == "--attribute-name");
        await Assert.That(attributeName.IsRequired).IsTrue();
        await Assert.That(attributeName.EnumDefinition).IsNull();
        await Assert.That(attributeName.CSharpType).IsEqualTo("string?");
        var output = await new OptionsClassGenerator().GenerateAsync(scraper.CreateToolDefinition() with { Commands = [command] });
        var content = output.Single(file => file.RelativePath.EndsWith("Options.Generated.cs", StringComparison.Ordinal)).Content;
        await Assert.That(content).Contains("string AttributeName");
        await Assert.That(content).DoesNotContain("AwsSnsSetTopicAttributesAttributeName");
    }

    [Test]
    public async Task Acm_Option_Enums_Exclude_Structural_Bullets()
    {
        // Option excerpts: https://docs.aws.amazon.com/cli/latest/reference/acm/create-acme-endpoint.html
        // The 2.36.46 man renderer uses `o` for each bullet, including a one-item list.
        const string help = """
            SYNOPSIS
                   create-acme-endpoint
                   --authorization-behavior <value>
                   [--contact <value>]
            OPTIONS
                   --authorization-behavior (string)
                      The authorization behavior for the ACME endpoint.

                      Possible values:

                      o PRE_APPROVED
                   --contact (string)
                      Specifies whether ACME clients must provide contact information during account registration.

                      Possible values:

                      o REQUIRED

                      o NOT_REQUIRED
            """;
        var scraper = new TestAwsCliScraper();
        var command = (await scraper.Parse(["aws", "acm", "create-acme-endpoint"], help))!;
        var authorization = command.Options.Single(option => option.SwitchName == "--authorization-behavior");
        await Assert.That(authorization.EnumDefinition).IsNull();
        await Assert.That(authorization.IsRequired).IsTrue();
        await Assert.That(authorization.CSharpType).IsEqualTo("string?");
        var contact = command.Options.Single(option => option.SwitchName == "--contact");
        await Assert.That(contact.EnumDefinition!.Values.Select(value => value.CliValue)).IsEquivalentTo(["REQUIRED", "NOT_REQUIRED"]);
        var tool = scraper.CreateToolDefinition() with { Commands = [command] };
        var enumFiles = await new EnumGenerator().GenerateAsync(tool);
        await Assert.That(enumFiles.Count).IsEqualTo(1);
        await Assert.That(enumFiles.Single().Content).Contains("EnumValue(\"REQUIRED\")");
        await Assert.That(enumFiles.Single().Content).Contains("EnumValue(\"NOT_REQUIRED\")");
        await Assert.That(enumFiles.Single().Content).DoesNotContain("EnumValue(\"o\")");
    }
}
