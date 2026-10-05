using ModularPipelines.OptionsGenerator.Generators;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public partial class GoCliScraperTests
{
    [Test]
    public async Task Working_Directory_Is_Global_Without_Promoting_Build_Flags()
    {
        var scraper = await CreateGlobalOptionsScraper();
        var tool = scraper.CreateToolDefinition();
        var global = tool.GetGlobalOptions().Single();

        await Assert.That(global.SwitchName).IsEqualTo("-C");
        await Assert.That(global.PropertyName).IsEqualTo("WorkingDirectory");
        await Assert.That(global.CSharpType).IsEqualTo("string?");
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
        AppContext.BaseDirectory, "Fixtures", "Go", "1.27.1",
        topic.Length == 0 ? "go-help.txt" : $"go-help-{topic.Replace(' ', '-')}.txt"));
}
