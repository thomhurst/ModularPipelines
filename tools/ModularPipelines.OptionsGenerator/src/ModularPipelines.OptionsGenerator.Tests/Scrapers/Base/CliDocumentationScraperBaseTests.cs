using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Base;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Base;

public class CliDocumentationScraperBaseTests
{
    [Test]
    [Arguments("json|yaml default=json")]
    [Arguments("json|yaml (default json)")]
    [Arguments("<json|yaml default=json>")]
    [Arguments("{json|yaml default=json}")]
    [Arguments("json||yaml")]
    [Arguments("|json|yaml")]
    [Arguments("json|yaml|")]
    [Arguments("[json|yaml")]
    [Arguments("json|yaml]")]
    [Arguments("[json|yaml>")]
    [Arguments("[<json|yaml>]")]
    [Arguments("(json|yaml")]
    [Arguments("json|yaml)")]
    [Arguments("(json|yaml]")]
    public async Task DetectEnumValues_Rejects_Annotated_Or_Incomplete_Pipe_Hints(string hint)
    {
        await Assert.That(TestDocumentationScraper.DetectEnumValues(hint)).IsNull();
    }

    [Test]
    [Arguments("<0|1>")]
    [Arguments("{0, 1}")]
    [Arguments("1.5|2.5")]
    public async Task DetectEnumValues_Does_Not_Infer_Enums_From_Numeric_Hints(string hint)
    {
        await Assert.That(TestDocumentationScraper.DetectEnumValues(hint)).IsNull();
    }

    [Test]
    [Arguments("<json|yaml>")]
    [Arguments("{json|yaml}")]
    [Arguments("[json|yaml]")]
    [Arguments("(json|yaml)")]
    [Arguments(" ( json|yaml ) ")]
    [Arguments(" [ json|yaml ] ")]
    [Arguments(" < json|yaml > ")]
    [Arguments(" { json|yaml } ")]
    public async Task DetectEnumValues_Strips_Placeholder_Wrappers_From_Pipe_Choices(string valueType)
    {
        var result = TestDocumentationScraper.DetectEnumValues(valueType);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Values.Select(value => value.CliValue)).IsEquivalentTo(["json", "yaml"]);
    }

    [Test]
    [Arguments("foo-bar|foo_bar|FOO_BAR|foo-bar")]
    [Arguments("{foo-bar, foo_bar, FOO_BAR, foo-bar}")]
    public async Task DetectEnumValues_Preserves_Distinct_Values_With_Unique_Members(string valueType)
    {
        var result = TestDocumentationScraper.DetectEnumValues(valueType);

        await Assert.That(result!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["foo-bar", "foo_bar", "FOO_BAR"]);
        await Assert.That(result.Values.Select(value => value.MemberName).Distinct().Count())
            .IsEqualTo(result.Values.Count);
    }

    [Test]
    [Arguments("net8.0|net9.0|1st")]
    [Arguments("{net8.0, net9.0, 1st}")]
    public async Task DetectEnumValues_Preserves_Punctuation_And_Leading_Digits(string valueType)
    {
        var result = TestDocumentationScraper.DetectEnumValues(valueType);

        await Assert.That(result!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["net8.0", "net9.0", "1st"]);
        await Assert.That(result.Values.All(value => Microsoft.CodeAnalysis.CSharp.SyntaxFacts.IsValidIdentifier(value.MemberName)))
            .IsTrue();
    }

    [Test]
    [Arguments(1, false)]
    [Arguments(2, true)]
    [Arguments(20, true)]
    [Arguments(21, false)]
    public async Task DetectEnumValues_Uses_Shared_Member_Limits(int count, bool expectEnum)
    {
        var values = Enumerable.Range(1, count).Select(index => $"value{index}").ToArray();
        var result = TestDocumentationScraper.DetectEnumValues(string.Join('|', values));

        await Assert.That(result is not null).IsEqualTo(expectEnum);
        if (expectEnum)
        {
            await Assert.That(result!.Values.Select(value => value.CliValue)).IsEquivalentTo(values);
        }
    }

    [Test]
    [Arguments("ms|s|m|h")]
    [Arguments("ns|us|ms|s|m|h")]
    [Arguments("<ms|s|m|h>")]
    [Arguments("{ms|s|m|h}")]
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
