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
