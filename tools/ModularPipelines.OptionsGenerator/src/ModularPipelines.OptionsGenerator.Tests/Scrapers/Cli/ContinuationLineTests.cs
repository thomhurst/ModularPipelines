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
    [Arguments("  --quiet             Suppress output", "Suppress")]
    [Arguments("  --env  stringArray   Set environment variables", "Set")]
    [Arguments("-i CODE1,CODE2..    --include=CODE1,CODE2..    Consider only given types", "Consider")]
    [Arguments("\t--env stringArray\tSet environment variables", "Set")]
    [Arguments("  --verbose    Verbose", "Verbose")]
    [Arguments("  --tls  Use TLS  (implies --tlsverify)", "Use")]
    public async Task Inline_Description_Column_Skips_Switches_And_Value_Hints(string line, string descriptionStart)
    {
        var expected = CliScraperBase.GetColumn(line, line.IndexOf(descriptionStart, StringComparison.Ordinal));

        await Assert.That(CliScraperBase.GetInlineDescriptionColumn(line)).IsEqualTo(expected);
    }

    [Test]
    [Arguments("  --quiet")]
    [Arguments("  -f, --file string")]
    [Arguments("  --env  stringArray")]
    [Arguments("  --output  <path>")]
    [Arguments("  --level  DEBUG")]
    [Arguments("   ")]
    public async Task Rows_Without_Prose_Have_No_Inline_Description_Column(string line)
    {
        await Assert.That(CliScraperBase.GetInlineDescriptionColumn(line)).IsNull();
    }

    [Test]
    public async Task Repeatable_Lookahead_Does_Not_Anchor_On_A_Terminal_Value_Hint()
    {
        // --env's description starts on the next line; the padded hint must not become the
        // continuation threshold, and the prose column inferred from that line must end the
        // block at the shallower nested --env-file row.
        const string helpText = """
              --env  stringArray
                                 Set environment variables
                  --env-file=PATH   Read variables from a file; may be specified multiple times
            """;

        using (Assert.Multiple())
        {
            await Assert.That(CliScraperBase.HelpDeclaresRepeatableOption(helpText, "--env", string.Empty)).IsFalse();
            await Assert.That(CliScraperBase.HelpDeclaresRepeatableOption(helpText, "--env-file", string.Empty)).IsTrue();
        }
    }

    [Test]
    public async Task Repeatable_Lookahead_Anchors_On_The_Prose_Column_Not_A_Padded_Value_Hint()
    {
        // The nested --env-file row sits deeper than the padded value hint but shallower than
        // the prose column, so it must end the --env block instead of lending it "multiple times".
        const string helpText = """
              --env  stringArray   Set environment variables
                        --env-file=PATH   Read variables from a file; may be specified multiple times
            """;

        using (Assert.Multiple())
        {
            await Assert.That(CliScraperBase.HelpDeclaresRepeatableOption(helpText, "--env", string.Empty)).IsFalse();
            await Assert.That(CliScraperBase.HelpDeclaresRepeatableOption(helpText, "--env-file", string.Empty)).IsTrue();
        }
    }

    [Test]
    public async Task Repeatable_Lookahead_Accepts_A_Single_Tab_As_The_Description_Separator()
    {
        const string helpText =
            "\t--env stringArray\tSet environment variables. Values from\n"
            + "\t\t\t\t--env-file=PATH are merged; may be specified\n"
            + "\t\t\t\tmultiple times\n"
            + "\t--quiet\tSuppress output";

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
