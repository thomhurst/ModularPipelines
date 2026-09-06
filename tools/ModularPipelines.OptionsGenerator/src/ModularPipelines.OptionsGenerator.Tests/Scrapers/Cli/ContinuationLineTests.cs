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
    public async Task Repeatable_Phrase_On_A_Wrapped_Option_Looking_Line_Is_Found()
    {
        // --quiet sits above --env so its block must stop at the --env row.
        const string helpText = """
                  --quiet             Suppress output
                  --env stringArray   Set environment variables. Values from
                                      --env-file=PATH are merged; may be specified
                                      multiple times
            """;

        using (Assert.Multiple())
        {
            await Assert.That(CliScraperBase.HelpDeclaresRepeatableOption(helpText, "--env", string.Empty)).IsTrue();
            await Assert.That(CliScraperBase.HelpDeclaresRepeatableOption(helpText, "--quiet", string.Empty)).IsFalse();
        }
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
    public async Task Option_Looking_Rows_Without_A_Known_Description_Column_Start_The_Next_Option()
    {
        const string wrapped = "                           --tlsverify";

        await Assert.That(CliScraperBase.IsContinuationLine(wrapped, 6, null, looksLikeOptionRow: true)).IsFalse();
    }
}
