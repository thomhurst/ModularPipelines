using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class AzGlobalOptionsTests
{
    private const string Globals = """
        Global Arguments
            --debug            : Increase logging verbosity to show all debug logs.
            --help -h          : Show this help message and exit.
            --only-show-errors : Only show errors, suppressing warnings.
            --output -o        : Output format. Allowed values: json, table, tsv.
            --query            : JMESPath query string.
            --verbose          : Increase logging verbosity.
        """;

    [Test]
    public async Task Scrape_Inherits_Globals_And_Removes_Only_Identical_Local_Options()
    {
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new AzCliScraper(new Executor(), cache, NullLogger<AzCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var tool = scraper.CreateToolDefinition() with { Commands = commands };
        var globals = tool.GetGlobalOptions();
        await Assert.That(globals.Select(option => option.SwitchName)).IsEquivalentTo(
            ["--acquire-policy-token", "--change-reference", "--debug", "--only-show-errors",
                "--output", "--query", "--verbose"]);
        await Assert.That(tool.GlobalOptionsBeforeSubcommands).IsFalse();
        var subscription = commands.Single(command => command.FullCommand == "az group show").Options
            .Single(option => option.SwitchName == "--subscription");
        await Assert.That(subscription.ShortForm).IsEqualTo("-s");
        await Assert.That(subscription.HasVerifiedValueShape).IsTrue();
        await Assert.That(subscription.PropertyType).IsEqualTo("string?");
        await Assert.That(globals.Single(option => option.SwitchName == "--output").ShortForm).IsEqualTo("-o");
        await Assert.That(globals.Single(option => option.SwitchName == "--debug").IsFlag).IsTrue();
        await Assert.That(commands.Single(command => command.FullCommand == "az group show").Options
            .Select(option => option.SwitchName)).IsEquivalentTo(["--name", "--subscription"]);
        await Assert.That(commands.Single(command => command.FullCommand == "az group clear").Options).IsEmpty();

        var set = commands.Single(command => command.FullCommand == "az group set");
        var local = set.Options.Single(option => option.SwitchName == "--subscription");
        await Assert.That(local.IsRequired).IsTrue();
        await Assert.That(local.PropertyType).IsEqualTo("IEnumerable<CliOptionValue>?");
        await Assert.That(local.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
        await Assert.That(local.GroupValues).IsTrue();
        await Assert.That(set.Options.Single(option => option.SwitchName == "--token").IsSecret).IsTrue();
        var resolved = InheritedPropertyCollisionResolver.Resolve(tool);
        var shadow = resolved.Commands.Single(command => command.FullCommand == set.FullCommand).Options
            .Single(option => option.SwitchName == "--subscription");
        await Assert.That(shadow.ShadowsGlobalOption).IsFalse();
        await Assert.That(shadow.PropertyName).IsEqualTo("Subscription");
        var generated = (await new GlobalOptionsBaseGenerator().GenerateAsync(resolved)).Single().Content;
        await Assert.That(generated).DoesNotContain(" Subscription");
        await Assert.That(generated).Contains("public virtual string? Output");
        await Assert.That(generated).DoesNotContain("[CliGlobalOptions]");
    }

    [Test]
    public async Task Installed_Parser_Globals_And_Local_Subscription_Aliases_Are_Preserved()
    {
        var directory = Path.Combine(AppContext.BaseDirectory, "Fixtures", "AzCli");
        var scraper = new TestScraper();
        var globals = scraper.ParseGlobals(await File.ReadAllTextAsync(Path.Combine(directory, "az-2.90.0-globals.txt")));
        await Assert.That(globals).Count().IsEqualTo(5);
        await Assert.That(globals.Any(option => option.SwitchName == "--subscription")).IsFalse();
        await Assert.That(globals.Single(option => option.SwitchName == "--output").ShortForm).IsEqualTo("-o");
        var command = await scraper.Parse(await File.ReadAllTextAsync(Path.Combine(directory, "az-2.90.0-account-set.txt")));
        var local = command!.Options.Single(option => option.SwitchName == "--name");
        await Assert.That(local.SwitchName).IsEqualTo("--name");
        await Assert.That(local.ShortForm).IsEqualTo("-n");
        await Assert.That(local.Aliases).IsEquivalentTo(["--subscription", "-s"]);
        var resolved = InheritedPropertyCollisionResolver.Resolve(scraper.CreateToolDefinition() with
        {
            GlobalOptions = globals,
            Commands = [command],
        });
        var replacement = resolved.Commands.Single().Options.Single(option => option.SwitchName == "--name");
        await Assert.That(replacement.PropertyName).IsEqualTo("Name");
        await Assert.That(replacement.SwitchName).IsEqualTo("--name");
        await Assert.That(replacement.ShortForm).IsEqualTo("-n");
        await Assert.That(replacement.ShadowsGlobalOption).IsFalse();
        await Assert.That(replacement.IsRequired).IsTrue();
    }

    [Test]
    [Arguments("value", "Plain text secret value.", true)]
    [Arguments("certificate-object", "The base64-encoded contents of a PEM file, which includes both the bucket server's certificate and private key.", true)]
    [Arguments("content-type", "Content type of the secret.", false)]
    public async Task Secret_Contents_Keep_Masking_While_Secret_Metadata_Stays_Visible(string name, string description, bool secret)
    {
        var command = await new TestScraper().Parse($"Command\n    az account set : Set.\nArguments\n    --{name} : {description}");
        await Assert.That(command!.Options.Single().IsSecret).IsEqualTo(secret);
    }

    [Test]
    public async Task Global_Arguments_Require_Complete_Parser_Metadata()
    {
        var scraper = new TestScraper();
        await Assert.That(() => scraper.ParseGlobals(Globals + "\n" + AzCliMetadataExecutor.MetadataMarker + "{}"))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Subscription_In_Command_Global_Arguments_Remains_Command_Specific()
    {
        var scraper = new TestScraper();
        var supported = await scraper.Parse("Command\n    az account set : Set.\n"
            + Globals + "\n    --subscription -s : Name or ID of subscription.\n" + Metadata());
        var unsupported = await scraper.Parse("Command\n    az account set : Set.\n" + Globals + "\n" + Metadata());
        await Assert.That(supported!.Options.Single(option => option.SwitchName == "--subscription").HasVerifiedValueShape).IsTrue();
        await Assert.That(unsupported!.Options.Any(option => option.SwitchName == "--subscription")).IsFalse();
    }

    [Test]
    [Arguments("az-2.90.0-group-show.txt", true)]
    [Arguments("az-2.90.0-extension-list.txt", false)]
    public async Task Captured_Command_Parsers_Control_Subscription_Support(string fixture, bool supported)
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "AzCli", fixture));
        var command = await new TestScraper().Parse(help);
        await Assert.That(command!.Options.Any(option => option.SwitchName == "--subscription")).IsEqualTo(supported);
    }

    [Test]
    [Arguments("no-passwords", "Do not generate passwords. Allowed values: false, true.")]
    [Arguments("secret", "Whether the variable is a secret. Allowed values: false, true.")]
    [Arguments("enable-access-token-issuance", "Enable access tokens. Allowed values: false, true.")]
    [Arguments("allow-credentials", "Allow credentials. Allowed values: false, true.")]
    public async Task Optional_Boolean_Values_Are_Not_Secrets(string name, string description)
    {
        var help = $"Command\n    az account set : Set.\nArguments\n    --{name} : {description}\n"
            + AzCliMetadataExecutor.MetadataMarker
            + $"{{\"--{name}\":{{\"IsInteger\":false,\"IsRepeated\":false,\"Nargs\":\"?\"}}}}";
        var option = (await new TestScraper().Parse(help))!.Options.Single();
        await Assert.That(option.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
        await Assert.That(option.PropertyType).IsEqualTo("CliOptionValue?");
        await Assert.That(option.IsSecret).IsFalse();
    }

    private sealed class TestScraper() : AzCliScraper(new Executor(),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<AzCliScraper>.Instance)
    {
        public IReadOnlyList<CliOptionDefinition> ParseGlobals(string help) => ParseGlobalOptions(help);
        public Task<CliCommandDefinition?> Parse(string help) => ParseCommandAsync(["az", "account", "set"], help, CancellationToken.None);
    }

    private static string Metadata(string subscriptionArity = "1") =>
        AzCliMetadataExecutor.MetadataMarker + $$$"""
        {"--debug":{"IsInteger":false,"IsRepeated":false,"Nargs":"0"},"--help":{"IsInteger":false,"IsRepeated":false,"Nargs":"0"},"--only-show-errors":{"IsInteger":false,"IsRepeated":false,"Nargs":"0"},
        "--output":{"IsInteger":false,"IsRepeated":false,"Nargs":"1"},"--query":{"IsInteger":false,"IsRepeated":false,"Nargs":"1"},"--subscription":{"IsInteger":false,"IsRepeated":false,"Nargs":"{{{subscriptionArity}}}"},
        "--verbose":{"IsInteger":false,"IsRepeated":false,"Nargs":"0"},"--name":{"IsInteger":false,"IsRepeated":false,"Nargs":"1"},"--token":{"IsInteger":false,"IsRepeated":false,"Nargs":"1"}}
        """;

    private sealed class Executor : ICliCommandExecutor
    {
        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);

        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            string? workingDirectory = null, CancellationToken cancellationToken = default) => Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardError = string.Empty,
                StandardOutput = arguments switch
                {
                    "--help" => "Group\n    az : Azure.\nSubgroups:\n    group : Groups.\n\n" + Globals + "\n" + Metadata(),
                    "group --help" => "Group\n    az group : Groups.\nCommands:\n    show : Show.\n    set : Set.\n    clear : Clear.\n",
                    "group show --help" => "Command\n    az group show : Show.\nArguments\n    --name -n : Name.\n    --subscription -s : Name or ID of subscription.\n\n" + Globals + "\n" + Metadata(),
                    "group set --help" => "Command\n    az group set : Set.\nRequired Arguments\n    --subscription -s [Required] : Subscription values.\nOptional Arguments\n    --token : Access token.\n\n" + Globals + "\n" + Metadata("*"),
                    "group clear --help" => "Command\n    az group clear : Clear.\n\n" + Globals + "\n" + Metadata(),
                    _ => throw new InvalidOperationException(arguments),
                },
            });
    }
}
