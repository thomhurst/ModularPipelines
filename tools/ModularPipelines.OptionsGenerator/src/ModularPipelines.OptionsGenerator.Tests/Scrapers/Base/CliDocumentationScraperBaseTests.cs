using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Base;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Base;

public class CliDocumentationScraperBaseTests
{
    [Test]
    [Arguments("ms|s|m|h")]
    [Arguments("ns|us|ms|s|m|h")]
    public async Task DetectEnumValues_Does_Not_Treat_Duration_Units_As_Complete_Values(string valueType)
    {
        var result = TestDocumentationScraper.DetectEnumValues(valueType);

        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task DetectEnumValues_Still_Recognizes_Actual_Enum_Values()
    {
        var result = TestDocumentationScraper.DetectEnumValues("plain|json|tty");

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["plain", "json", "tty"]);
    }

    private sealed class TestDocumentationScraper()
        : CliDocumentationScraperBase(new HttpClient(), NullLogger.Instance)
    {
        public override string ToolName => "test";
        public override string NamespacePrefix => "Test";
        public override string TargetNamespace => "ModularPipelines.Test";
        public override string OutputDirectory => "src/ModularPipelines.Test";

        public static CliEnumDefinition? DetectEnumValues(string valueType)
        {
            return DetectEnumValues("Value", "TestOptions", valueType, null);
        }

        public override Task<CliToolDefinition> ScrapeAsync(CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }
    }
}
