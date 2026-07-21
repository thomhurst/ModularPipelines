using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class ShellcheckCliScraperTests
{
    [Test]
    public async Task ParseCommand_Models_All_V011_Options()
    {
        var scraper = new TestShellcheckCliScraper();

        var command = await scraper.ParseShellcheckCommand(HelpText);

        await Assert.That(command).IsNotNull();
        await Assert.That(command!.Options.Select(x => x.SwitchName)).IsEquivalentTo(
        [
            "--check-sourced",
            "--color",
            "--include",
            "--exclude",
            "--extended-analysis",
            "--format",
            "--list-optional",
            "--norc",
            "--rcfile",
            "--enable",
            "--source-path",
            "--shell",
            "--severity",
            "--version",
            "--wiki-link-count",
            "--external-sources",
            "--help",
        ]);

        var color = command.Options.Single(x => x.SwitchName == "--color");
        await Assert.That(color.ShortForm).IsEqualTo("-C");
        await Assert.That(color.CSharpType).IsEqualTo("ShellcheckColor?");
        await Assert.That(color.IsFlag).IsFalse();

        var extendedAnalysis = command.Options.Single(x => x.SwitchName == "--extended-analysis");
        await Assert.That(extendedAnalysis.CSharpType).IsEqualTo("bool?");
        await Assert.That(extendedAnalysis.IsFlag).IsFalse();

        await Assert.That(command.Options.Single(x => x.SwitchName == "--wiki-link-count").CSharpType).IsEqualTo("int?");
        await Assert.That(command.Options.Single(x => x.SwitchName == "--include").CSharpType).IsEqualTo("string?");
        await Assert.That(command.Options.Any(x => x.IsSecret)).IsFalse();

        var shell = command.Options.Single(x => x.SwitchName == "--shell").EnumDefinition;
        await Assert.That(shell).IsNotNull();
        await Assert.That(shell!.Values.Select(x => x.CliValue)).Contains("busybox");

        await Assert.That(command.PositionalArguments).Count().IsEqualTo(2);

        var file = command.PositionalArguments.Single(x => x.PropertyName == "File");
        await Assert.That(file.CSharpType).IsEqualTo("string");
        await Assert.That(file.IsRequired).IsTrue();
        await Assert.That(file.Placement).IsEqualTo(PositionalArgumentPosition.AfterOptions);

        var additionalFiles = command.PositionalArguments.Single(x => x.PropertyName == "AdditionalFiles");
        await Assert.That(additionalFiles.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(additionalFiles.IsRequired).IsFalse();
        await Assert.That(additionalFiles.Placement).IsEqualTo(PositionalArgumentPosition.AfterOptions);
    }

    private const string HelpText = """
        Usage: shellcheck [OPTIONS...] FILES...
          -a                  --check-sourced            Include warnings from sourced files
          -C[WHEN]            --color[=WHEN]             Use color (auto, always, never)
          -i CODE1,CODE2..    --include=CODE1,CODE2..    Consider only given types of warnings
          -e CODE1,CODE2..    --exclude=CODE1,CODE2..    Exclude types of warnings
                              --extended-analysis=bool   Perform dataflow analysis (default true)
          -f FORMAT           --format=FORMAT            Output format (checkstyle, diff, gcc, json, json1, quiet, tty)
                              --list-optional            List checks disabled by default
                              --norc                     Don't look for .shellcheckrc files
                              --rcfile=RCFILE            Prefer the specified configuration file over searching for one
          -o check1,check2..  --enable=check1,check2..   List of optional checks to enable (or 'all')
          -P SOURCEPATHS      --source-path=SOURCEPATHS  Specify path when looking for sourced files
          -s SHELLNAME        --shell=SHELLNAME          Specify dialect (sh, bash, dash, ksh, busybox)
          -S SEVERITY         --severity=SEVERITY        Minimum severity of errors to consider
          -V                  --version                  Print version information
          -W NUM              --wiki-link-count=NUM      The number of wiki links to show, when applicable
          -x                  --external-sources         Allow 'source' outside of FILES
                              --help                     Show this usage summary and exit
        """;

    private sealed class TestShellcheckCliScraper()
        : ShellcheckCliScraper(new StubExecutor(), new StubHelpTextCache(), NullLogger<ShellcheckCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> ParseShellcheckCommand(string helpText) =>
            ParseCommandAsync(["shellcheck"], helpText, CancellationToken.None);
    }

    private sealed class StubExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) => throw new NotSupportedException();

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(false);
    }

    private sealed class StubHelpTextCache : IHelpTextCache
    {
        public bool TryGet(string cacheKey, out string? helpText)
        {
            helpText = null;
            return false;
        }

        public void Set(string cacheKey, string helpText)
        {
        }

        public void Clear()
        {
        }

        public CacheStatistics GetStatistics() => new();
    }
}
