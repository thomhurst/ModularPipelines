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
    [Arguments("sync")]
    [Arguments("wait")]
    public async Task App_Commands_Parse_Optional_Application_Name_Alternatives(string command)
    {
        var scraper = new TestArgoCdCliScraper();
        var parsedArguments = scraper.ParseArguments(
            $"Usage:\n  argocd app {command} [APPNAME... | -l selector] [flags]");

        var arguments = scraper.ApplyFix(["app", command], parsedArguments);

        await Assert.That(arguments).Count().IsEqualTo(1);
        await Assert.That(arguments[0].PropertyName).IsEqualTo("ApplicationNames");
        await Assert.That(arguments[0].CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(arguments[0].IsRequired).IsFalse();
    }

    [Test]
    [Arguments("cluster", "get", "Usage:\n  argocd cluster get SERVER/NAME [flags]", "ServerOrName", null)]
    [Arguments("cluster", "rm", "Usage:\n  argocd cluster rm SERVER/NAME [flags]", "ServerOrName", null)]
    [Arguments("proj", "add-destination", "Usage:\n  argocd proj add-destination PROJECT SERVER/NAME NAMESPACE [flags]", "Project", "Namespace")]
    public async Task Server_Name_Alternatives_Are_One_Operand(
        string parentCommand,
        string command,
        string helpText,
        string firstProperty,
        string? lastProperty)
    {
        var scraper = new TestArgoCdCliScraper();
        var parsedArguments = scraper.ParseArguments(helpText);

        var arguments = scraper.ApplyFix([parentCommand, command], parsedArguments);

        await Assert.That(arguments.Select(argument => argument.PropertyName)).Contains(firstProperty);
        await Assert.That(arguments.Select(argument => argument.PropertyName)).Contains("ServerOrName");
        if (lastProperty is not null)
        {
            await Assert.That(arguments.Select(argument => argument.PropertyName)).Contains(lastProperty);
        }

        await Assert.That(arguments.Select(argument => argument.PropertyName)).DoesNotContain("Server");
        await Assert.That(arguments.Select(argument => argument.PropertyName)).DoesNotContain("Name");
    }

    [Test]
    [Arguments("repo", "add", "REPOURL", "RepositoryUrl")]
    [Arguments("repocreds", "rm", "CREDSURL", "CredentialsUrl")]
    public async Task Url_Placeholders_Use_Public_Api_Casing(
        string parentCommand,
        string command,
        string placeholder,
        string propertyName)
    {
        var scraper = new TestArgoCdCliScraper();
        var parsedArguments = scraper.ParseArguments(
            $"Usage:\n  argocd {parentCommand} {command} {placeholder} [flags]");

        var arguments = scraper.ApplyFix([parentCommand, command], parsedArguments);

        await Assert.That(arguments.Single().PropertyName).IsEqualTo(propertyName);
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

    [Test]
    public async Task Prompts_Enabled_Is_Value_Option()
    {
        const string helpText = """
            Update global configuration.

            Usage:
              argocd configure [flags]

            Flags:
                  --prompts-enabled   Enable (or disable) optional interactive prompts
            """;

        var command = await new TestArgoCdCliScraper().Parse(["argocd", "configure"], helpText);
        var option = command!.Options.Single(item => item.SwitchName == "--prompts-enabled");

        await Assert.That(option.CSharpType).IsEqualTo("bool?");
        await Assert.That(option.IsFlag).IsFalse();
        await Assert.That(option.ValueSeparator).IsEqualTo("=");
    }

    [Test]
    [Arguments(@"C:\Users\alice/.config/argocd/config")]
    [Arguments("/home/alice/.config/argocd/config")]
    public async Task Home_Directory_Is_Redacted_From_Config_Description(string configPath)
    {
        var helpText = $$"""
            Get an application.

            Usage:
              argocd app get APPNAME [flags]

            Flags:
                  --config string   Path to Argo CD config (default "{{configPath}}")
            """;

        var command = await new TestArgoCdCliScraper().Parse(["argocd", "app", "get"], helpText);
        var option = command!.Options.Single(item => item.SwitchName == "--config");

        await Assert.That(option.Description).IsEqualTo("Path to Argo CD config (default \"<home>/.config/argocd/config\")");
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
