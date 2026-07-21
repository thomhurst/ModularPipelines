using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers;

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

        DotNetCliCompatibility.NormalizeOptions(["build"], options);

        await Assert.That(options).Count().IsEqualTo(1);
        await Assert.That(options[0].SwitchName).IsEqualTo("--nologo");
        await Assert.That(options[0].ShortForm).IsNull();
        await Assert.That(options[0].PropertyName).IsEqualTo("NoLogo");
    }

    [Test]
    public async Task Build_Compatibility_Preserves_Removed_Public_Properties()
    {
        var properties = DotNetCliCompatibility.GetProperties(["build"]);

        await Assert.That(properties.Select(property => property.PropertyName))
            .IsEquivalentTo(["Nologo", "Debug"]);
        await Assert.That(properties.Single(property => property.PropertyName == "Nologo").ForwardToPropertyName)
            .IsEqualTo("NoLogo");
        await Assert.That(properties.Single(property => property.PropertyName == "Debug").ForwardToPropertyName)
            .IsNull();
    }

    [Test]
    public async Task Documentation_Fallback_Applies_Build_Compatibility()
    {
        const string html = """
                            <html><body><article>
                            <dl>
                              <dt>--no-logo</dt><dd>Do not display the startup banner.</dd>
                              <dt>--debug</dt><dd>Enable debug output.</dd>
                            </dl>
                            </article></body></html>
                            """;
        using var httpClient = new HttpClient(new StaticHtmlHandler(html));
        var scraper = new DotNetCliDocumentationScraper(
            httpClient,
            NullLogger<DotNetCliDocumentationScraper>.Instance);

        var tool = await scraper.ScrapeAsync();
        var build = tool.Commands.Single(command => command.CommandParts.SequenceEqual(["build"]));

        await Assert.That(build.Options).Count().IsEqualTo(1);
        await Assert.That(build.Options[0].SwitchName).IsEqualTo("--nologo");
        await Assert.That(build.Options[0].PropertyName).IsEqualTo("NoLogo");
        await Assert.That(build.CompatibilityProperties.Select(property => property.PropertyName))
            .IsEquivalentTo(["Nologo", "Debug"]);
    }

    private static CliOptionDefinition Flag(string switchName, string propertyName) => new()
    {
        SwitchName = switchName,
        ShortForm = "-nologo",
        PropertyName = propertyName,
        CSharpType = "bool?",
        IsFlag = true,
    };

    private sealed class StaticHtmlHandler(string html) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(html),
                RequestMessage = request,
            });
    }
}
