using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Scrapers;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class DockerDocumentationScraperTests
{
    [Test]
    [Arguments(2, "", "")]
    [Arguments(21, "", "")]
    [Arguments(2, "<", ">")]
    [Arguments(21, "<", ">")]
    [Arguments(2, "{", "}")]
    [Arguments(21, "{", "}")]
    public async Task Option_Table_Preserves_Choices_Inside_And_Outside_Enum_Limit(int count, string prefix, string suffix)
    {
        var values = Enumerable.Range(1, count).Select(index => $"mode{index}").ToArray();
        var hint = $"{prefix}{string.Join('|', values)}{suffix}";
        var html = $"""
            <html><body><h1>docker build</h1>
            <table><thead><tr><th>Option</th><th>Description</th><th>Default</th></tr></thead>
            <tbody><tr><td>--mode</td><td>Select mode.</td><td>{WebUtility.HtmlEncode(hint)}</td></tr></tbody></table>
            </body></html>
            """;
        using var handler = new DocumentationHandler(html);
        using var client = new HttpClient(handler);
        var scraper = new DockerDocumentationScraper(client, NullLogger<DockerDocumentationScraper>.Instance);

        var tool = await scraper.ScrapeAsync();
        var option = tool.Commands.Single().Options.Single();

        if (count == 2)
        {
            await Assert.That(option.EnumDefinition!.Values.Select(value => value.CliValue)).IsEquivalentTo(values);
            await Assert.That(option.Description).IsEqualTo("Select mode.");
        }
        else
        {
            await Assert.That(option.EnumDefinition).IsNull();
            await Assert.That(option.CSharpType).IsEqualTo("string?");
            await Assert.That(option.Description).IsEqualTo($"Select mode. [value type: {hint}]");
        }
    }

    private sealed class DocumentationHandler(string html) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.FromResult(request.RequestUri!.AbsolutePath == "/reference/cli/docker/build/"
                ? new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(html) }
                : new HttpResponseMessage(HttpStatusCode.NotFound));
    }
}
