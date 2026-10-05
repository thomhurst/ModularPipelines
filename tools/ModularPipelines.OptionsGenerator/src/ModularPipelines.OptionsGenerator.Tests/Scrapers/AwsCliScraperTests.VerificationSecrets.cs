using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public partial class AwsCliScraperTests
{
    [Test]
    [Arguments("The one-time passcode that the recipient submitted for validation.", true)]
    [Arguments("The one-time password that the recipient submitted for validation.", true)]
    [Arguments("The status code returned after validating a one-time passcode.", false)]
    [Arguments("The number of attempts to validate a one-time passcode.", false)]
    public async Task Verification_Code_Secrets_Follow_Value_Description(string description, bool expectedSecret)
    {
        var scraper = new TestAwsCliScraper();
        var command = (await scraper.Parse(["aws", "endusermessaging", "validate-notify-code-verification"], $$"""
            OPTIONS
                   --code (string)
                    {{description}}
            """))!;
        var enhancer = new OptionTypeEnhancer(
            new OptionTypeDetectorPipeline([], NullLogger<OptionTypeDetectorPipeline>.Instance),
            NullLogger<OptionTypeEnhancer>.Instance);
        var tool = await enhancer.EnhanceManualOverridesAsync(
            scraper.CreateToolDefinition() with { Commands = [command] });
        var files = await new OptionsClassGenerator().GenerateAsync(tool);

        await Assert.That(tool.Commands.Single().Options.Single().IsSecret).IsEqualTo(expectedSecret);
        await Assert.That(files.Single().Content.Contains("[SecretValue]", StringComparison.Ordinal)).IsEqualTo(expectedSecret);
    }
}
