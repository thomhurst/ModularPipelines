using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class CargoCliScraperTests
{
    [Test]
    public async Task Wrapped_Descriptions_That_Look_Like_Option_Rows_Stay_Prose()
    {
        const string helpText = """
            Compile a local package and all of its dependencies

            Usage: cargo build [OPTIONS]

            Options:
                  --ignore-rust-version
                                      Ignore `rust-version` specification in packages
                  --keep-going        Do not abort the build as soon as there is an error. Implies
                                      --jobs
                  --timings           Output information how long each compilation takes
              -h, --help              Print help
            """;

        var command = await new TestCargoCliScraper().Parse(["cargo", "build"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["--ignore-rust-version", "--keep-going", "--timings", "--help"]);
            await Assert.That(GetOption(command, "--ignore-rust-version").Description)
                .IsEqualTo("Ignore `rust-version` specification in packages");
            await Assert.That(GetOption(command, "--keep-going").Description)
                .IsEqualTo("Do not abort the build as soon as there is an error. Implies --jobs");
            await Assert.That(GetOption(command, "--timings").Description)
                .IsEqualTo("Output information how long each compilation takes");
        }
    }

    [Test]
    public async Task Options_Under_Custom_Clap_Headings_Are_Parsed()
    {
        // cargo add groups its dependency-source and section switches under "Source:" and
        // "Section:" rather than an "Options" heading; only Arguments/Commands are skipped.
        // (-p, --package [<SPEC>] uses clap's optional-value form, which this scraper does not
        // read yet; see #4712.)
        const string helpText = """
            Add dependencies to a Cargo.toml manifest file

            Usage: cargo add [OPTIONS] <DEP>[@<VERSION>] ...
                   cargo add [OPTIONS] --path <PATH> ...
                   cargo add [OPTIONS] --git <URL> ...

            Arguments:
              [DEP_ID]...
                      Reference to a package to add as a dependency

            Options:
                  --no-default-features
                      Disable the default features
              -h, --help
                      Print help (see a summary with '-h')

            Manifest Options:
                  --manifest-path <PATH>
                      Path to Cargo.toml

            Package Selection:
              -p, --package [<SPEC>]
                      Package to modify

            Source:
                  --path <PATH>
                      Filesystem path to local crate to add
                  --git <URI>
                      Git repository location
                  --branch <BRANCH>
                      Git branch to download the crate from

            Section:
                  --dev
                      Add as development dependency
                  --target <TARGET>
                      Add as dependency to the given target platform
            """;

        var command = await new TestCargoCliScraper().Parse(["cargo", "add"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo([
                    "--no-default-features", "--help", "--manifest-path",
                    "--path", "--git", "--branch", "--dev", "--target",
                ]);
            await Assert.That(GetOption(command, "--path").Description)
                .IsEqualTo("Filesystem path to local crate to add");
            await Assert.That(GetOption(command, "--dev").IsFlag).IsTrue();
        }
    }

    private static CliOptionDefinition GetOption(CliCommandDefinition command, string switchName) =>
        command.Options.Single(option => option.SwitchName == switchName);

    private sealed class TestCargoCliScraper()
        : CargoCliScraper(
            new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<CargoCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                UsageSynopsisParser.Parse(helpText, commandPath),
                CancellationToken.None);
    }
}
