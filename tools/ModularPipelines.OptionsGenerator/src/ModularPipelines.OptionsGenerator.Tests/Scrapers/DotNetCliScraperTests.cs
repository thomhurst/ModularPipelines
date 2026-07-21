using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class DotNetCliScraperTests
{
    [Test]
    [Arguments("--nologo", "Nologo")]
    [Arguments("--no-logo", "NoLogo")]
    public async Task Build_Options_Are_Stable_Across_Sdk_Help_Formats(
        string scrapedSwitch,
        string scrapedPropertyName)
    {
        var options = new List<CliOptionDefinition>
        {
            Flag(scrapedSwitch, scrapedPropertyName),
            Flag("--debug", "Debug"),
        };

        DotNetCliScraper.ApplyBuildOptionCompatibility(["build"], options);

        await Assert.That(options).Count().IsEqualTo(1);
        await Assert.That(options[0].SwitchName).IsEqualTo("--nologo");
        await Assert.That(options[0].ShortForm).IsNull();
        await Assert.That(options[0].PropertyName).IsEqualTo("NoLogo");
    }

    [Test]
    public async Task Build_Compatibility_Preserves_Removed_Public_Properties()
    {
        var properties = DotNetCliScraper.GetCompatibilityProperties(["build"]);

        await Assert.That(properties.Select(property => property.PropertyName))
            .IsEquivalentTo(["Nologo", "Debug"]);
        await Assert.That(properties.Single(property => property.PropertyName == "Nologo").ForwardToPropertyName)
            .IsEqualTo("NoLogo");
        await Assert.That(properties.Single(property => property.PropertyName == "Debug").ForwardToPropertyName)
            .IsNull();
    }

    private static CliOptionDefinition Flag(string switchName, string propertyName) => new()
    {
        SwitchName = switchName,
        ShortForm = "-nologo",
        PropertyName = propertyName,
        CSharpType = "bool?",
        IsFlag = true,
    };
}
