using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public partial class GoCliScraperTests
{
    [Test]
    [Arguments("")]
    [Arguments("usage: go build [packages]\n\nThe build flags are:\n    -race\n        enable race detection")]
    [Arguments("usage: go build [packages]\n\nThe build flags are:\n    -C\n        Change directory.")]
    public async Task Missing_Directory_Declaration_Rejects_Root_Help(string buildHelp)
    {
        var scraper = CreateScraper(new Dictionary<string, string>
        {
            ["help"] = await ReadGlobalFixture(""),
            ["help build"] = buildHelp,
        });

        await Assert.That(async () => await scraper.LoadRootHelp())
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("go help build");
        await Assert.That(scraper.CreateToolDefinition().GetGlobalOptions()).IsEmpty();
    }

    [Test]
    public async Task Working_Directory_Is_Global_Without_Promoting_Build_Flags()
    {
        var scraper = await CreateGlobalOptionsScraper();
        var tool = scraper.CreateToolDefinition();
        var global = tool.GetGlobalOptions().Single();

        await Assert.That(global.SwitchName).IsEqualTo("-C");
        await Assert.That(global.PropertyName).IsEqualTo("WorkingDirectory");
        await Assert.That(global.CSharpType).IsEqualTo("string?");
        await Assert.That(global.ValueArity).IsEqualTo(CliOptionValueArity.Required);
        await Assert.That(global.Phase).IsEqualTo(CommandLinePhase.Normal);
        await Assert.That(global.IsFlag || global.IsSecret || global.AcceptsMultipleValues).IsFalse();
        await Assert.That(tool.GlobalOptionsBeforeSubcommands).IsTrue();
        var generated = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(generated).Contains("public virtual string? WorkingDirectory");
        await Assert.That(generated).Contains("[CliGlobalOptions]");
        await Assert.That(generated).DoesNotContain("CliFlag");
    }

    [Test]
    [Arguments("build")]
    [Arguments("mod edit")]
    [Arguments("version")]
    [Arguments("test")]
    public async Task Commands_Inherit_Working_Directory_Without_Local_Duplicates(string commandName)
    {
        var scraper = await CreateGlobalOptionsScraper();
        var command = (await scraper.Parse(["go", .. commandName.Split(' ')],
            await ReadGlobalFixture(commandName)))!;

        await Assert.That(command.Options.Any(option => option.SwitchName == "-C")).IsFalse();
        if (commandName == "build")
        {
            await Assert.That(command.Options.Any(option => option.SwitchName == "-race")).IsTrue();
        }
    }

    [Test]
    public async Task Root_Discovery_Resets_Global_Options_When_Build_Help_Becomes_Incomplete()
    {
        var scraper = await CreateGlobalOptionsScraper();
        await Assert.That(scraper.CreateToolDefinition().GetGlobalOptions().Count).IsEqualTo(1);
        scraper.SetCachedHelp("go build", string.Empty);

        await Assert.That(async () => await scraper.LoadRootHelp())
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("go help build");
        await Assert.That(scraper.CreateToolDefinition().GetGlobalOptions()).IsEmpty();
    }

    [Test]
    public async Task Root_Discovery_Clears_Global_Options_When_Root_Help_Is_Missing()
    {
        var scraper = await CreateGlobalOptionsScraper();
        scraper.SetCachedHelp("go", string.Empty);

        await Assert.That(await scraper.LoadRootHelp()).IsNullOrEmpty();
        await Assert.That(scraper.CreateToolDefinition().GetGlobalOptions()).IsEmpty();
    }

    [Test]
    public async Task Test_Fixtures_Preserve_Command_Flags_And_Operands_After_Global_Discovery()
    {
        var scraper = await CreateGlobalOptionsScraper();
        var command = (await scraper.Parse(["go", "test"],
            await ReadGlobalFixture("test") + "\n\n" + await ReadGlobalFixture("testflag")))!;

        await Assert.That(command.Options.Any(option => option.SwitchName == "-C")).IsFalse();
        await Assert.That(command.Options.Single(option => option.SwitchName == "-c").IsFlag).IsTrue();
        await Assert.That(command.Options.Any(option => option.SwitchName == "-run")).IsTrue();
        await Assert.That(command.PositionalArguments).IsNotEmpty();
    }

    private static async Task<TestGoCliScraper> CreateGlobalOptionsScraper()
    {
        var scraper = CreateScraper(new Dictionary<string, string>
        {
            ["help"] = await ReadGlobalFixture(""),
            ["help build"] = await ReadGlobalFixture("build"),
        });
        await scraper.LoadRootHelp();
        return scraper;
    }

    private static Task<string> ReadGlobalFixture(string topic) => File.ReadAllTextAsync(Path.Combine(
        AppContext.BaseDirectory, "Fixtures", "Go", "1.27.2",
        topic.Length == 0 ? "go-help.txt" : $"go-help-{topic.Replace(' ', '-')}.txt"));
}
