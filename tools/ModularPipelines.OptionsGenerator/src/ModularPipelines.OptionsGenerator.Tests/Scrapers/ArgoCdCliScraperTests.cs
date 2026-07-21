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
        var scraper = new TestArgoCdCliScraper();
        var arguments = scraper.ParseArguments(
            "Usage:\n  argocd app rollback APPNAME [ID] [flags]");

        await Assert.That(arguments).Count().IsEqualTo(2);
        await Assert.That(arguments[0].PropertyName).IsEqualTo("Appname");
        await Assert.That(arguments[0].IsRequired).IsTrue();
        await Assert.That(arguments[1].PropertyName).IsEqualTo("Id");
        await Assert.That(arguments[1].IsRequired).IsFalse();
    }

    [Test]
    public async Task Shared_Cobra_Parser_Does_Not_Parse_Bare_Kubectl_Qualifier_Tokens()
    {
        var scraper = new TestArgoCdCliScraper();
        var arguments = scraper.ParseBaseArguments(
            "Usage:\n  kubectl get TYPE[.VERSION][.GROUP]/NAME [flags]");

        await Assert.That(arguments).IsEmpty();
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
    [Arguments("generate", "File", "string")]
    [Arguments("delete", "ApplicationSetNames", "IEnumerable<string>")]
    public async Task Appset_Commands_Add_Required_Arguments_Omitted_By_Help(
        string command,
        string propertyName,
        string csharpType)
    {
        var scraper = new TestArgoCdCliScraper();

        var arguments = scraper.ApplyFix(["appset", command], []);

        await Assert.That(arguments).Count().IsEqualTo(1);
        await Assert.That(arguments[0].PropertyName).IsEqualTo(propertyName);
        await Assert.That(arguments[0].CSharpType).IsEqualTo(csharpType);
        await Assert.That(arguments[0].IsRequired).IsTrue();
    }

    [Test]
    public async Task Positional_Argument_Compound_Names_Are_Normalized()
    {
        var scraper = new TestArgoCdCliScraper();
        var parsedArguments = scraper.ParseArguments(
            "Usage:\n  argocd app rollback APPNAME [ID] [flags]");

        var arguments = scraper.ApplyFix(["app", "rollback"], parsedArguments);

        await Assert.That(arguments[0].PropertyName).IsEqualTo("ApplicationName");
        await Assert.That(arguments[1].PropertyName).IsEqualTo("Id");
    }

    [Test]
    public async Task Slash_Separated_Metavariable_Is_One_Positional_Argument()
    {
        var scraper = new TestArgoCdCliScraper();
        var parsedArguments = scraper.ParseArguments(
            "Usage:\n  argocd admin settings rbac can ROLE/SUBJECT ACTION RESOURCE [SUB-RESOURCE] [flags]");
        var arguments = scraper.ApplyFix(["admin", "settings", "rbac", "can"], parsedArguments);

        await Assert.That(arguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(["RoleSubject", "Action", "Resource", "SubResource"]);
        await Assert.That(arguments.Take(3).All(argument => argument.IsRequired)).IsTrue();
        await Assert.That(arguments[3].IsRequired).IsFalse();
    }

    [Test]
    public async Task Default_True_Boolean_Is_Value_Option()
    {
        const string helpText = """
            Implement bulk project role update.

            Usage:
              argocd admin proj update-role-policy PROJECT-GLOB MODIFICATION ACTION [flags]

            Flags:
                  --dry-run   Dry run (default true)
            """;

        var command = await new TestArgoCdCliScraper().Parse(
            ["argocd", "admin", "proj", "update-role-policy"],
            helpText);
        var option = command!.Options.Single(item => item.SwitchName == "--dry-run");

        await Assert.That(option.CSharpType).IsEqualTo("bool?");
        await Assert.That(option.IsFlag).IsFalse();
        await Assert.That(option.ValueSeparator).IsEqualTo("=");
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

        public IReadOnlyList<CliPositionalArgument> ParseArguments(string helpText) =>
            ParsePositionalArguments(helpText);

        public IReadOnlyList<CliPositionalArgument> ParseBaseArguments(string helpText) =>
            base.ParsePositionalArguments(helpText);

        public IReadOnlyList<CliPositionalArgument> ApplyFix(
            string[] commandParts,
            IReadOnlyList<CliPositionalArgument> positionalArguments) =>
            ApplyPositionalArgumentFixes(commandParts, positionalArguments);

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(commandPath, helpText, CancellationToken.None);
    }
}
