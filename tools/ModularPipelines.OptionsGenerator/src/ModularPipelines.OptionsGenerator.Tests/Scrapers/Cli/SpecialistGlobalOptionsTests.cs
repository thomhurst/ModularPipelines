using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class SpecialistGlobalOptionsTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Flyway_Root_Configuration_Is_Inherited_By_Commands(bool windowsNewlines)
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Flyway", "10.20.1", "flyway-help.txt"));
        help = help.Replace("\r\n", "\n");
        if (windowsNewlines)
        {
            help = help.Replace("\n", "\r\n");
        }

        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new FlywayCliScraper(new FixtureExecutor(help, "Configuration\n-url=  : Jdbc url\n-custom=  : Command-specific setting\n"), cache, NullLogger<FlywayCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var tool = scraper.CreateToolDefinition();
        var globals = tool.GetGlobalOptions();
        var url = globals.Single(option => option.SwitchName == "-url");
        await Assert.That(url.ValueSeparator).IsEqualTo("=");
        await Assert.That(url.IsFlag).IsFalse();
        await Assert.That(globals.Single(option => option.SwitchName == "-password").IsSecret).IsTrue();
        await Assert.That(globals.Single(option => option.SwitchName == "-X").IsFlag).IsTrue();
        await Assert.That(globals.Single(option => option.SwitchName == "-q").IsFlag).IsTrue();
        await Assert.That(globals.Single(option => option.SwitchName == "-n").IsFlag).IsTrue();
        await Assert.That(globals.Any(option => option.SwitchName == "-flyway")).IsFalse();
        var placeholders = globals.Single(option => option.PropertyName == "Placeholders");
        await Assert.That(placeholders.SwitchName).IsEqualTo("-placeholders.");
        await Assert.That(placeholders.CSharpType).IsEqualTo("IReadOnlyList<KeyValue>?");
        await Assert.That(placeholders.IsKeyValue).IsTrue();
        await Assert.That(placeholders.ValueSeparator).IsEqualTo(string.Empty);
        var jdbcProperties = globals.Single(option => option.PropertyName == "JdbcProperties");
        await Assert.That(jdbcProperties.SwitchName).IsEqualTo("-jdbcProperties.");
        await Assert.That(jdbcProperties.IsSecret).IsTrue();
        var generated = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(generated).Contains("public virtual string? Url { get; set; }");
        await Assert.That(generated).Contains("OptionFormat.EqualsSeparated");
        var migrate = commands.Single(command => command.FullCommand == "flyway migrate");
        await Assert.That(migrate.Options.Select(option => option.SwitchName)).IsEquivalentTo(["-custom"]);
    }

    [Test]
    public async Task Snyk_Root_Debug_Is_Global_Without_Promoting_Example_Options()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Snyk", "1.1302.0", "snyk-help.txt"));
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new SnykCliScraper(new FixtureExecutor(help, "Usage\n  snyk test [<OPTIONS>]\n\nOptions\n  --json\n    Output JSON.\n\nDebug\n  Use the -d option to output the debug logs.\n"), cache, NullLogger<SnykCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var global = scraper.CreateToolDefinition().GetGlobalOptions().Single();
        await Assert.That(global.SwitchName).IsEqualTo("-d");
        await Assert.That(global.PropertyName).IsEqualTo("Debug");
        await Assert.That(global.IsFlag).IsTrue();
        await Assert.That(commands.Single(command => command.FullCommand == "snyk test").Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--json"]);
    }

    private sealed class FixtureExecutor(string rootHelp, string commandHelp) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments, CancellationToken cancellationToken = default, string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                StandardOutput = arguments == "--help" ? rootHelp : commandHelp,
                StandardError = string.Empty,
                ExitCode = 0,
            });

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }
}
