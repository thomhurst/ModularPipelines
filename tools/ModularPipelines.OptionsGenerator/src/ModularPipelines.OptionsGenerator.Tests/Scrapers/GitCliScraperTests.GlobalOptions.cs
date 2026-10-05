using ModularPipelines.Attributes;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public partial class GitCliScraperTests
{
    [Test]
    public async Task Root_Settings_Exclude_Reporting_Actions_And_Internal_Arguments()
    {
        using var scraper = new TestGitCliScraper();
        var options = scraper.ParseGlobals(File.ReadAllText(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Git", "2.56.0", "root-help.txt")));

        await Assert.That(options.Select(option => option.SwitchName)).IsEquivalentTo([
            "-C", "-c", "--config-env", "--exec-path", "--git-dir", "--work-tree", "--namespace",
            "--bare", "--paginate", "--no-pager", "--no-replace-objects", "--no-lazy-fetch",
            "--no-optional-locks", "--no-advice",
        ]);
        await Assert.That(scraper.ParseGlobals("usage: git [--version] [--html-path] [--super-prefix=<path>] <command>"))
            .IsEmpty();
    }

    [Test]
    public async Task Root_Values_Retain_Arity_Repetition_Ordering_And_Secrets()
    {
        using var scraper = new TestGitCliScraper();
        var options = scraper.ParseGlobals("usage: git [-C <path>] [-c <name>=<value>] [--git-dir=<path>] [--config-env=<name>=<envvar>] <command>");
        var directory = options.Single(option => option.SwitchName == "-C");
        var configuration = options.Single(option => option.SwitchName == "-c");
        await Assert.That(directory.CSharpType).IsEqualTo("string[]?");
        await Assert.That(directory.Phase).IsEqualTo(CommandLinePhase.EarlyOperand);
        await Assert.That(configuration.AcceptsMultipleValues).IsTrue();
        await Assert.That(configuration.IsSecret).IsTrue();
        await Assert.That(configuration.ValueSeparator).IsEqualTo(" ");
        await Assert.That(options.Single(option => option.SwitchName == "--git-dir").ValueSeparator).IsEqualTo("=");
        await Assert.That(options.Single(option => option.SwitchName == "--config-env").CSharpType).IsEqualTo("string[]?");
        await Assert.That(options.All(option => !option.IsFlag && option.ValueArity == CliOptionValueArity.Required)).IsTrue();
    }

    [Test]
    public async Task Root_Parsing_Does_Not_Consume_Command_Local_Short_Options()
    {
        using var scraper = new TestGitCliScraper();
        scraper.ParseGlobals("usage: git [-C <path>] [-c <name>=<value>] <command>");
        var command = await scraper.Parse(["git", "apply"], File.ReadAllText(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Git", "2.56.0", "apply-help.txt")));
        var context = command!.Options.Single(option => option.SwitchName == "-C");
        await Assert.That(context.CSharpType).IsEqualTo("int?");
        await Assert.That(context.PropertyName).IsNotEqualTo("ChangeDirectories");
        await Assert.That(scraper.CreateToolDefinition().GenerateCode).IsFalse();
    }
}
