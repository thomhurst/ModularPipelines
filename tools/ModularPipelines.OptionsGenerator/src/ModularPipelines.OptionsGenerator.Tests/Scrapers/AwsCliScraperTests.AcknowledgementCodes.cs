using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public partial class AwsCliScraperTests
{
    [Test]
    public async Task Accept_Page_Generates_Secret_Metadata_Only_For_Acknowledgement_Value()
    {
        // The command help fixture uses descriptions and types retained in the
        // AWS CLI 2.38.0 snapshot (run 38503946817), preserving its numeric constraints.
        var path = Path.Combine(AppContext.BaseDirectory, "Fixtures", "AwsCli", "ssm-contacts-accept-page.txt");
        var scraper = new TestAwsCliScraper();
        var command = (await scraper.Parse(["aws", "ssm-contacts", "accept-page"], await File.ReadAllTextAsync(path)))!;
        var enhancer = new OptionTypeEnhancer(
            new OptionTypeDetectorPipeline([], NullLogger<OptionTypeDetectorPipeline>.Instance),
            NullLogger<OptionTypeEnhancer>.Instance);
        var tool = await enhancer.EnhanceManualOverridesAsync(
            scraper.CreateToolDefinition() with { Commands = [command] });
        var options = tool.Commands.Single().Options;
        var files = await new OptionsClassGenerator().GenerateAsync(tool);

        await Assert.That(options.Where(option => option.IsSecret).Select(option => option.PropertyName))
            .IsEquivalentTo(["AcceptCode"]);
        await Assert.That(options.Single(option => option.PropertyName == "AcceptCode").IsRequired).IsTrue();
        await Assert.That(files.Single().Content).Contains($"[SecretValue]{Environment.NewLine}    [CliOption(\"--accept-code\")]");
        await Assert.That(options.Single(option => option.PropertyName == "AcceptCodeValidation").EnumDefinition is not null).IsTrue();
    }
}
