using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public partial class AwsCliScraperTests
{
    [Test]
    [Arguments("A connection string in the form of a fully qualified domain name (FQDN), such as www.example.com . WARNING: After you create a connection string, it is always associated to your Amazon Web Services account. You cannot recreate the same connection string with a different account, even if you delete all instances of it from the original account. The connection string is globally reserved for your account. Constraints: o min: 1 o max: 255 o pattern: ^[.0-9a-zA-Z\\-]{1,255}$", false)]
    [Arguments("Storage account connection string.", true)]
    [Arguments("A connection string containing a password and a fully qualified domain name (FQDN).", true)]
    public async Task Connection_String_Classification_Preserves_Public_Domain_Aliases(
        string description, bool expectedSecret)
    {
        // The domain-alias description is captured in the generated WorkSpaces create-connection-alias options.
        var scraper = new TestAwsCliScraper();
        var command = (await scraper.Parse(["aws", "workspaces", "create-connection-alias"], $$"""
            OPTIONS
                   --connection-string (string)
                    {{description}}
            """))!;
        await Assert.That(command.Options.Single().IsSecret).IsEqualTo(expectedSecret);

        var enhancer = new OptionTypeEnhancer(
            new OptionTypeDetectorPipeline([], NullLogger<OptionTypeDetectorPipeline>.Instance),
            NullLogger<OptionTypeEnhancer>.Instance);
        var tool = await enhancer.EnhanceManualOverridesAsync(
            scraper.CreateToolDefinition() with { Commands = [command] });
        var files = await new OptionsClassGenerator().GenerateAsync(tool);

        using (Assert.Multiple())
        {
            await Assert.That(tool.Commands.Single().Options.Single().IsSecret).IsEqualTo(expectedSecret);
            await Assert.That(files.Single().Content.Contains("[SecretValue]", StringComparison.Ordinal)).IsEqualTo(expectedSecret);
            await Assert.That(files.Single().Content).Contains("[CliOption(\"--connection-string\")]");
        }
    }
}
