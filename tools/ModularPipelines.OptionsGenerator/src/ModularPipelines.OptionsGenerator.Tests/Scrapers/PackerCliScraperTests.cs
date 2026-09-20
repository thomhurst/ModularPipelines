using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class PackerCliScraperTests
{
    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task Version_Excludes_Update_Advisory(bool versionOnStandardError, bool advisoryOnStandardError)
    {
        const string identity = "Packer v1.16.0\n";
        const string advisory = "\nYour version of Packer is out of date! The latest version\n"
            + "is 1.16.1. You can update by downloading from www.packer.io/downloads.html\n";
        var scraper = CreateVersionScraper(
            (versionOnStandardError ? string.Empty : identity) + (advisoryOnStandardError ? string.Empty : advisory),
            (versionOnStandardError ? identity : string.Empty) + (advisoryOnStandardError ? advisory : string.Empty));

        await Assert.That(await scraper.GetVersionAsync()).IsEqualTo("Packer v1.16.0");
    }

    [Test]
    public async Task Version_Preserves_Prerelease_And_Build_Identity()
    {
        var scraper = CreateVersionScraper("Packer v1.17.0-dev+build.42\r\n", string.Empty);

        await Assert.That(await scraper.GetVersionAsync()).IsEqualTo("Packer v1.17.0-dev+build.42");
    }

    [Test]
    public async Task Version_Rejects_Advisory_Without_Installed_Identity()
    {
        var scraper = CreateVersionScraper("The latest version is Packer v1.16.1.\n", string.Empty);

        await Assert.That(await scraper.GetVersionAsync()).IsNull();
    }

    private static PackerCliScraper CreateVersionScraper(string output, string error) => new(
        new VersionExecutor(output, error),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<PackerCliScraper>.Instance);

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Quoted_Value_Hint_Preserves_Same_Column_Repeatability_Description(bool nested)
    {
        var indentation = nested ? "      " : "  ";
        var helpText = "Usage: packer build [options] TEMPLATE\n\nOptions:\n"
            + (nested ? "  -color=false  Configure variables\n" : string.Empty)
            + $"{indentation}-var 'key=value'  \n"
            + $"{indentation}May be specified multiple times\n"
            + "  -force  Force a build.\n";

        var command = await new TestPackerCliScraper().Parse(["packer", "build"], helpText);
        var variable = command!.Options.Single(option => option.SwitchName == "--var");
        await Assert.That(variable.Description).IsEqualTo("May be specified multiple times");
        await Assert.That(variable.AcceptsMultipleValues).IsTrue();
        await Assert.That(variable.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--force").Description)
            .IsEqualTo("Force a build.");
        if (nested)
        {
            await Assert.That(command.Options.Single(option => option.SwitchName == "--color").Description)
                .IsEqualTo("Configure variables");
        }
    }

    [Test]
    public async Task Wrapped_Descriptions_That_Look_Like_Option_Rows_Stay_Prose()
    {
        // The wrapped "-only=foo,bar  to ..." line deliberately keeps two spaces so it satisfies
        // the option-row pattern; only its column keeps it inside the description.
        const string helpText = """
            Usage: packer build [options] TEMPLATE

            Options:
              -color=false                 Disable color output. (Default: color)
              -except=foo,bar,baz          Run all builds and post-processors other than these. Combine with
                                           -only=foo,bar  to narrow the selection further.
              -force                       Force a build to continue if artifacts exist, deletes existing artifacts.
            """;

        var command = await new TestPackerCliScraper().Parse(["packer", "build"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["--color", "--except", "--force"]);
            await Assert.That(command.Options.Single(option => option.SwitchName == "--except").Description)
                .IsEqualTo("Run all builds and post-processors other than these. Combine with -only=foo,bar  to narrow the selection further.");
            await Assert.That(command.Options.Single(option => option.SwitchName == "--force").Description)
                .IsEqualTo("Force a build to continue if artifacts exist, deletes existing artifacts.");
        }
    }

    private sealed class VersionExecutor(string output, string error) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) => Task.FromResult(new CliCommandResult
            {
                StandardOutput = output,
                StandardError = error,
                ExitCode = 0,
            });

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class TestPackerCliScraper()
        : PackerCliScraper(
            new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<PackerCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                UsageSynopsisParser.Parse(helpText, commandPath),
                CancellationToken.None);
    }
}
