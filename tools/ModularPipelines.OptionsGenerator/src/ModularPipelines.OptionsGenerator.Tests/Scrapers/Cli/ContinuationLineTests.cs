using ModularPipelines.OptionsGenerator.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class ContinuationLineTests
{
    [Test]
    [Arguments("", 0)]
    [Arguments("--flag", 0)]
    [Arguments("    prose", 4)]
    [Arguments("\tprose", 8)]
    [Arguments("  \tprose", 8)]
    [Arguments("\t\tprose", 16)]
    [Arguments("        \tprose", 16)]
    public async Task GetIndentation_Counts_Columns_With_Eight_Column_Tab_Stops(string line, int expected)
    {
        await Assert.That(CliScraperBase.GetIndentation(line)).IsEqualTo(expected);
    }

    [Test]
    [Arguments("  -f, --file string   Path", 22, 22)]
    [Arguments("\t--file\tPath", 8, 16)]
    [Arguments("abc", 10, 3)]
    public async Task GetColumn_Expands_Tabs_Before_The_Index(string line, int index, int expected)
    {
        await Assert.That(CliScraperBase.GetColumn(line, index)).IsEqualTo(expected);
    }

    [Test]
    public async Task Blank_Lines_Never_Continue_A_Description()
    {
        await Assert.That(CliScraperBase.IsContinuationLine("   ", 2, 20, looksLikeOptionRow: false)).IsFalse();
    }

    [Test]
    public async Task Prose_Continues_Only_When_Deeper_Than_The_Declaration()
    {
        using (Assert.Multiple())
        {
            await Assert.That(CliScraperBase.IsContinuationLine("      wrapped prose", 2, null, looksLikeOptionRow: false)).IsTrue();
            await Assert.That(CliScraperBase.IsContinuationLine("  sibling text", 2, null, looksLikeOptionRow: false)).IsFalse();
            await Assert.That(CliScraperBase.IsContinuationLine("Section:", 2, null, looksLikeOptionRow: false)).IsFalse();
        }
    }

    [Test]
    public async Task Option_Rows_Before_The_Description_Column_Start_The_Next_Option()
    {
        const string nextOption = "      --tlsverify          Use TLS and verify the remote";

        await Assert.That(CliScraperBase.IsContinuationLine(nextOption, 6, 27, looksLikeOptionRow: true)).IsFalse();
    }

    [Test]
    public async Task Option_Looking_Rows_At_The_Description_Column_Are_Wrapped_Prose()
    {
        const string wrapped = "                           --tlsverify";

        using (Assert.Multiple())
        {
            await Assert.That(CliScraperBase.IsContinuationLine(wrapped, 6, 27, looksLikeOptionRow: true)).IsTrue();
            await Assert.That(CliScraperBase.IsContinuationLine(wrapped, 6, 20, looksLikeOptionRow: true)).IsTrue();
        }
    }

    [Test]
    public async Task Option_Looking_Rows_Deeper_Than_The_Declaration_Continue_Until_A_Column_Is_Known()
    {
        const string wrapped = "        --tlsverify is implied by this flag.";

        using (Assert.Multiple())
        {
            await Assert.That(CliScraperBase.IsContinuationLine(wrapped, 6, null, looksLikeOptionRow: true)).IsTrue();
            await Assert.That(CliScraperBase.IsContinuationLine(wrapped, 8, null, looksLikeOptionRow: true)).IsFalse();
        }
    }

    [Test]
    public async Task First_Wrapped_Line_That_Looks_Like_An_Option_Row_Still_Establishes_The_Column()
    {
        string[] lines =
        [
            "    --all-namespaces",
            "        --namespace is ignored when this flag is set. Lists the requested",
            "        objects across every namespace instead.",
            "      --namespace  Scope the listing to one namespace.",
        ];
        var index = 0;

        var description = CliScraperBase.AccumulateWrappedDescription(
            lines,
            ref index,
            inlineDescription: null,
            static line => line.TrimStart().StartsWith("--", StringComparison.Ordinal));

        using (Assert.Multiple())
        {
            await Assert.That(description)
                .IsEqualTo("--namespace is ignored when this flag is set. Lists the requested objects across every namespace instead.");
            await Assert.That(index).IsEqualTo(2);
        }
    }

    [Test]
    public async Task Rows_Without_Inline_Prose_Infer_The_Description_Column_From_The_First_Wrapped_Line()
    {
        string[] lines =
        [
            "    --all-namespaces",
            "        If present, list the requested objects across all namespaces. Pair with",
            "        --namespace to scope the listing instead.",
            "    --allow-missing-template-keys",
            "        If true, ignore any errors in templates.",
        ];
        var index = 0;

        var description = CliScraperBase.AccumulateWrappedDescription(
            lines,
            ref index,
            inlineDescription: null,
            static line => line.TrimStart().StartsWith("--", StringComparison.Ordinal));

        using (Assert.Multiple())
        {
            await Assert.That(description)
                .IsEqualTo("If present, list the requested objects across all namespaces. Pair with --namespace to scope the listing instead.");
            await Assert.That(index).IsEqualTo(2);
        }
    }

    [Test]
    public async Task Option_Rows_Shallower_Than_The_Inferred_Column_Still_Start_The_Next_Option()
    {
        string[] lines =
        [
            "    --all-namespaces",
            "        If present, list the requested objects across all namespaces.",
            "      --namespace  Scope the listing to one namespace.",
        ];
        var index = 0;

        var description = CliScraperBase.AccumulateWrappedDescription(
            lines,
            ref index,
            inlineDescription: null,
            static line => line.TrimStart().StartsWith("--", StringComparison.Ordinal));

        using (Assert.Multiple())
        {
            await Assert.That(description).IsEqualTo("If present, list the requested objects across all namespaces.");
            await Assert.That(index).IsEqualTo(1);
        }
    }
}
