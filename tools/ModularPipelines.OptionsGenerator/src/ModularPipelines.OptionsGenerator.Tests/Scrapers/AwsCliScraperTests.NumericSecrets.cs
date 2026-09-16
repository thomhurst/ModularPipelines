using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public partial class AwsCliScraperTests
{
    public static IEnumerable<(string Command, string SwitchName, string PropertyName, string Description)> NumericSecretOptions()
    {
        // Numeric option metadata captured from AWS CLI 2.36.46 snapshot 5f94679375
        // (run 35058181945), including absent descriptions and line-wrap hyphenation.
        var path = Path.Combine(AppContext.BaseDirectory, "Fixtures", "AwsCli", "aws-2.36.46-numeric-secret-options.json");
        return JsonSerializer.Deserialize<Dictionary<string, string>[]>(File.ReadAllText(path))!
            .Select(entry => (entry["command"], entry["switchName"], entry["propertyName"], entry["description"]));
    }

    [Test]
    [MethodDataSource(nameof(NumericSecretOptions))]
    public async Task Numeric_Metadata_Does_Not_Generate_Secret_Attributes(
        string commandPath, string switchName, string propertyName, string description)
    {
        var scraper = new TestAwsCliScraper();
        var command = (await scraper.Parse(["aws", .. commandPath.Split(' ')], $$"""
            OPTIONS
                   {{switchName}} (integer)
                    {{description}}
            """))!;
        var enhancer = new OptionTypeEnhancer(
            new OptionTypeDetectorPipeline([], NullLogger<OptionTypeDetectorPipeline>.Instance),
            NullLogger<OptionTypeEnhancer>.Instance);
        var tool = await enhancer.EnhanceManualOverridesAsync(
            scraper.CreateToolDefinition() with { Commands = [command] });
        var option = tool.Commands.Single().Options.Single();
        var files = await new OptionsClassGenerator().GenerateAsync(tool);

        using (Assert.Multiple())
        {
            await Assert.That(command.FullCommand).IsEqualTo($"aws {commandPath}");
            await Assert.That(option.PropertyName).IsEqualTo(propertyName);
            await Assert.That(option.CSharpType).IsEqualTo("int?");
            await Assert.That(option.IsSecret).IsFalse();
            await Assert.That(files.Single().Content).DoesNotContain("[SecretValue]");
            await Assert.That(files.Single().Content).Contains($"[CliOption(\"{switchName}\")]");
        }
    }
}
