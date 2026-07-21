using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class ArgoCdCliScraperTests
{
    [Test]
    public async Task Appset_Uses_ApplicationSet_Identifier()
    {
        var scraper = new TestArgoCdCliScraper();

        await Assert.That(scraper.NormalizeIdentifier("appset")).IsEqualTo("ApplicationSet");
        await Assert.That(scraper.NormalizeIdentifier("app")).IsEqualTo("App");
    }

    [Test]
    public async Task Cobra_Usage_Parses_Bare_Required_And_Bracketed_Optional_Arguments()
    {
        var arguments = TestArgoCdCliScraper.ParseArguments(
            "Usage:\n  argocd app rollback APPNAME [ID] [flags]");

        await Assert.That(arguments).Count().IsEqualTo(2);
        await Assert.That(arguments[0].PropertyName).IsEqualTo("Appname");
        await Assert.That(arguments[0].IsRequired).IsTrue();
        await Assert.That(arguments[1].PropertyName).IsEqualTo("Id");
        await Assert.That(arguments[1].IsRequired).IsFalse();
    }

    [Test]
    public async Task Appset_Create_Adds_Required_File_Arguments_Omitted_By_Help()
    {
        var scraper = new TestArgoCdCliScraper();

        var arguments = scraper.ApplyFix(["appset", "create"], []);

        await Assert.That(arguments).Count().IsEqualTo(1);
        await Assert.That(arguments[0].PropertyName).IsEqualTo("Files");
        await Assert.That(arguments[0].CSharpType).IsEqualTo("IEnumerable<string>");
        await Assert.That(arguments[0].IsRequired).IsTrue();
    }

    [Test]
    public async Task Positional_Argument_Compound_Names_Are_Normalized()
    {
        var scraper = new TestArgoCdCliScraper();
        var parsedArguments = TestArgoCdCliScraper.ParseArguments(
            "Usage:\n  argocd app rollback APPNAME [ID] [flags]");

        var arguments = scraper.ApplyFix(["app", "rollback"], parsedArguments);

        await Assert.That(arguments[0].PropertyName).IsEqualTo("ApplicationName");
        await Assert.That(arguments[1].PropertyName).IsEqualTo("Id");
    }

    private sealed class TestArgoCdCliScraper : ArgoCdCliScraper
    {
        public TestArgoCdCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<ArgoCdCliScraper>.Instance)
        {
        }

        public string NormalizeIdentifier(string commandPart) => NormalizeCommandIdentifier(commandPart);

        public static IReadOnlyList<CliPositionalArgument> ParseArguments(string helpText) =>
            ParsePositionalArguments(helpText);

        public IReadOnlyList<CliPositionalArgument> ApplyFix(
            string[] commandParts,
            IReadOnlyList<CliPositionalArgument> positionalArguments) =>
            ApplyPositionalArgumentFixes(commandParts, positionalArguments);
    }
}
