using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class GhGlobalOptionsAuditTests
{
    [Test]
    public async Task Root_Control_Actions_Do_Not_Become_Inherited_Settings()
    {
        await Assert.That(new AuditScraper().Globals(Fixture("root"))).IsEmpty();
    }

    [Test]
    public async Task Repository_Selection_Remains_On_The_Issue_Command()
    {
        var command = await new AuditScraper().Parse(["gh", "issue", "list"], Fixture("issue list"));
        var repository = command!.Options.Single(option => option.SwitchName == "--repo");
        await Assert.That(repository.ShortForm).IsEqualTo("-R");
        await Assert.That(repository.IsFlag).IsFalse();
        await Assert.That(repository.AcceptsMultipleValues).IsFalse();
        await Assert.That(repository.CSharpType).IsEqualTo("string?");
    }

    [Test]
    [Arguments("auth status")]
    [Arguments("config get")]
    [Arguments("api")]
    public async Task Unrelated_Commands_Do_Not_Gain_Repository_Selection(string name)
    {
        var command = await new AuditScraper().Parse(["gh", .. name.Split(' ')], Fixture(name));
        await Assert.That(command).IsNotNull();
        await Assert.That(command!.Options.Any(option => option.SwitchName == "--repo")).IsFalse();
    }

    private static string Fixture(string command) => File.ReadAllText(Path.Combine(
        AppContext.BaseDirectory, "Fixtures", "Gh", "2.98.0", command.Replace(' ', '-') + ".txt"));

    private sealed class AuditScraper() : GhCliScraper(
        new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<GhCliScraper>.Instance)
    {
        public IReadOnlyList<CliOptionDefinition> Globals(string help) => ParseGlobalOptions(help);

        public Task<CliCommandDefinition?> Parse(string[] path, string help) =>
            ParseCommandAsync(path, help, ParseUsageSynopsis(path, help), CancellationToken.None);
    }
}
