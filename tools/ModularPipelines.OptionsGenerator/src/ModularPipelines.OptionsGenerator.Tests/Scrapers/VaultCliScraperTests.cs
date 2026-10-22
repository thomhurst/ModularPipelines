using System.Reflection;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.Tests.Generators;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class VaultCliScraperTests
{
    private static readonly string[] TpmMetadataValues = ["owner=platform team", "region=west"];
    private static readonly string[] TpmPassthroughValues = ["-role-name=build team", "-mount-path=auth/custom"];

    [Test]
    [Arguments("print token", "2.2.0", "Prints the value of the Vault token that will be used for commands, after taking into account the configured token-helper and the environment.")]
    [Arguments("hcp connect", "2.2.0", "Authenticates users or machines to HCP using either provided arguments or retrieved token through browser login. A successful authentication results in an HCP token and an HCP Vault address being locally cached.")]
    [Arguments("tpm attest", "2.2.0", "Performs TPM attestation against a Vault tpm auth method mount. The command opens the local TPM device, runs the two-phase challenge/response workflow (begin + finish), and on success writes the issued certificate and associated key material to the state directory. Additionally, it stores the private key in the TPM:")]
    [Arguments("read", "2.1.1", "Reads data from Vault at the given path. This can be used to read secrets, generate dynamic credentials, get configuration details, and more.")]
    [Arguments("version", "2.1.1", "Prints the version of this Vault CLI. This does not print the target Vault server version.")]
    [Arguments("operator raft snapshot inspect", "2.1.1", "Inspects a snapshot file.")]
    public async Task Captured_Help_Uses_Complete_Description_Paragraph(
        string command, string version, string expectedDescription)
    {
        var parsed = await ParseFixture(command, version);

        await Assert.That(parsed.Description).IsEqualTo(expectedDescription);

        var files = await new OptionsClassGenerator().GenerateAsync(
            new TestVaultCliScraper().CreateToolDefinition() with { Commands = [parsed] });
        await Assert.That(files.Single().Content)
            .Contains($"/// {expectedDescription}");
    }

    [Test]
    public async Task Description_Preserves_Flags_And_Skips_Wrapped_Usage_And_Notes()
    {
        var command = await new TestVaultCliScraper().ParseGroup(["vault", "test"], """
            Usage: vault test [options]
                <PATH>

              Note: This is a prerequisite,
              with a wrapped continuation.

              Runs with --example and -other flags,
              preserving the full command description.

              An example:

                  $ vault test --example

            Options:
              -example  An example flag.
            """);

        await Assert.That(command!.Description)
            .IsEqualTo("Runs with --example and -other flags, preserving the full command description.");
    }

    [Test]
    [Arguments("Options:\n  -address=<string>\n      Address of the Vault server.")]
    [Arguments("Subcommands:\n    read    Read data from Vault")]
    [Arguments("  Examples:\n\n      $ vault test --example")]
    [Arguments("  Note: A prerequisite.\n\n      $ vault test --example")]
    public async Task Missing_Description_Does_Not_Select_Examples_Options_Or_Commands(string trailingHelp)
    {
        var command = await new TestVaultCliScraper().ParseGroup(
            ["vault", "test"], "Usage: vault test [options]\n\n" + trailingHelp);

        await Assert.That(command!.Description).IsNull();
    }

    [Test]
    public async Task Hcp_Connect_Example_Options_Preserve_String_Values_And_Secrets()
    {
        var help = File.ReadAllText(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Vault", "2.2.0", "hcp-connect.txt"));
        var command = (await new TestVaultCliScraper().ParseGroup(["vault", "hcp", "connect"], help))!;

        await Assert.That(command.Options.Select(option => option.SwitchName)).IsEquivalentTo(
            ["-client-id", "-secret-id", "-organization-id", "-project-id", "-cluster-id"]);
        foreach (var option in command.Options)
        {
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.CSharpType).IsEqualTo("string?");
            await Assert.That(option.ValueSeparator).IsEqualTo("=");
            await Assert.That(option.IsRequired).IsFalse();
            await Assert.That(option.IsSecret).IsEqualTo(option.SwitchName == "-secret-id");
        }

        await Assert.That(command.PositionalArguments).IsEmpty();
    }

    [Test]
    public async Task Hcp_Disconnect_Does_Not_Inherit_Http_Options()
    {
        var help = File.ReadAllText(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Vault", "2.2.0", "hcp-disconnect.txt"));
        var command = (await new TestVaultCliScraper().ParseGroup(["vault", "hcp", "disconnect"], help))!;

        await Assert.That(command.Options).IsEmpty();
        await Assert.That(command.PositionalArguments).IsEmpty();
    }

    [Test]
    public async Task Hcp_Example_Options_Ignore_Prose_Other_Commands_And_Quoted_Values()
    {
        var command = (await new TestVaultCliScraper().ParseGroup(["vault", "hcp", "connect"], """
            Usage: vault hcp connect [options]

            Mention -prose=value without advertising an invocation.
              $ vault hcp disconnect -other=value
              $ vault hcp connect-extra -different=value
              $ vault hcp connect child -child=value
              $ vault hcp connect -client-id="value -embedded=ignored" -secret-id='value -nested=ignored'
              $ vault hcp connect -client-id=duplicate
            """))!;

        await Assert.That(command.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["-client-id", "-secret-id"]);
    }

    [Test]
    public async Task Structured_Hcp_Options_Take_Precedence_Over_Examples()
    {
        var command = (await new TestVaultCliScraper().ParseGroup(["vault", "hcp", "connect"], """
            Usage: vault hcp connect [options]

              $ vault hcp connect -client-id=example -obsolete=value

            Options:
              -client-id=<string>    Client identifier from structured help.
            """))!;

        var option = command.Options.Single();
        await Assert.That(option.SwitchName).IsEqualTo("-client-id");
        await Assert.That(option.Description).IsEqualTo("Client identifier from structured help.");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Shared_Traversal_Preserves_Operands_After_Flags(bool alternativeSynopsis)
    {
        var helpText = """
            Usage: vault audit disable -tls-skip-verify <PATH>

            Options:
                -tls-skip-verify    Skip TLS verification
                -address=<string>    Address of the Vault server
            """;
        if (alternativeSynopsis)
        {
            helpText = "Usage: vault audit disable -address <ADDRESS>\n" + helpText;
        }

        var scraper = new VaultCliScraper(
            new VaultHelpExecutor(helpText),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<VaultCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var leaf = commands.Single(command => command.FullCommand == "vault audit disable");
        var path = leaf.PositionalArguments.Single();
        await Assert.That(path.PropertyName).IsEqualTo("Path");
        await Assert.That(path.IsRequired).IsEqualTo(!alternativeSynopsis);
        await Assert.That(path.AssociatedOptionSwitch).IsNull();
        await Assert.That(leaf.Options.Single(option => option.PropertyName == "TlsSkipVerify").SwitchName)
            .IsEqualTo("-tls-skip-verify");
    }

    [Test]
    public async Task Value_Option_Placeholders_Remain_Excluded_From_Positionals()
    {
        const string helpText = """
            Usage: vault read -address <ADDRESS> <PATH>

            Options:
                -address=<string>    Address of the Vault server
            """;
        var command = await new TestVaultCliScraper().ParseGroup(["vault", "read"], helpText);
        await Assert.That(command!.PositionalArguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(["Path"]);
        await Assert.That(command.Options.Single().SwitchName).IsEqualTo("-address");
    }

    [Test]
    public async Task Shared_Traversal_Preserves_Required_Subcommand()
    {
        var scraper = new VaultCliScraper(
            new VaultHelpExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<VaultCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var audit = commands.Single(command => command.FullCommand == "vault audit");
        using (Assert.Multiple())
        {
            await Assert.That(audit.PositionalArguments[0].PropertyName).IsEqualTo("Subcommand");
            await Assert.That(audit.PositionalArguments[0].IsRequired).IsTrue();
            await Assert.That(commands.Select(command => command.FullCommand))
                .Contains("vault audit disable");
        }
    }

    [Test]
    public async Task Command_Group_Preserves_Subcommand_And_Optional_Args()
    {
        const string helpText = """
            Usage: vault audit <subcommand> [options] [args]

              This command groups subcommands for interacting with Vault's audit devices.

            Subcommands:
                disable    Disables an audit device
                enable     Enables an audit device
                list       Lists enabled audit devices
            """;
        var command = await new TestVaultCliScraper().ParseGroup(
            ["vault", "audit"],
            helpText);

        var subcommand = command!.PositionalArguments[0];
        var argument = command.PositionalArguments[1];
        using (Assert.Multiple())
        {
            await Assert.That(subcommand.PropertyName).IsEqualTo("Subcommand");
            await Assert.That(subcommand.IsRequired).IsTrue();
            await Assert.That(argument.PropertyName).IsEqualTo("Args");
            await Assert.That(argument.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(argument.IsVariadic).IsTrue();
            await Assert.That(argument.IsRequired).IsFalse();
            await Assert.That(argument.Phase).IsEqualTo(CommandLinePhase.Passthrough);
        }
    }

    [Test]
    public async Task Captured_Help_Preserves_Http_Arity_Descriptions_And_Secrets()
    {
        var command = await ParseFixture("read");
        var header = command.Options.Single(option => option.SwitchName == "-header");
        await Assert.That(header.IsFlag).IsFalse();
        await Assert.That(header.AcceptsMultipleValues).IsTrue();
        await Assert.That(header.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(command.Options.Single(option => option.SwitchName == "-address").Description)
            .Contains("VAULT_ADDR");
        await Assert.That(command.Options.Single(option => option.SwitchName == "-namespace").ShortForm)
            .IsEqualTo("-ns");
        await Assert.That(command.Options.Single(option => option.SwitchName == "-mfa").IsSecret).IsTrue();
        await Assert.That(command.Options.Single(option => option.SwitchName == "-client-key").IsSecret).IsFalse();
        await Assert.That(command.Options.Single(option => option.SwitchName == "-unlock-key").IsSecret).IsTrue();
    }

    [Test]
    public async Task Token_Create_Id_Is_A_Credential_Value()
    {
        var command = await ParseFixture("token create");
        var id = command.Options.Single(option => option.SwitchName == "-id");

        await Assert.That(id.IsSecret).IsTrue();
        await Assert.That(id.IsFlag).IsFalse();
        await Assert.That(id.CSharpType).IsEqualTo("string?");
        await Assert.That(id.ValueSeparator).IsEqualTo("=");
        await Assert.That(command.Options.Single(option => option.SwitchName == "-display-name").IsSecret).IsFalse();
    }

    [Test]
    public async Task Other_Command_Ids_Are_Not_Classified_As_Token_Credentials()
    {
        var command = await new TestVaultCliScraper().ParseGroup(["vault", "other"], """
            Usage: vault other [options]

            Options:
              -id=<string>  Identifier of the resource.
            """);

        await Assert.That(command!.Options.Single(option => option.SwitchName == "-id").IsSecret).IsFalse();
    }

    [Test]
    public async Task Captured_Help_Preserves_Command_Specific_Applicability()
    {
        var server = await ParseFixture("server");
        var print = await ParseFixture("print token");
        await Assert.That(server.Options.Any(option => option.SwitchName == "-address")).IsTrue();
        await Assert.That(server.Options.Any(option => option.SwitchName == "-format")).IsFalse();
        await Assert.That(server.Options.Single(option => option.SwitchName == "-dev-root-token-id").IsSecret).IsTrue();
        await Assert.That(print.Options).IsEmpty();
        await Assert.That(new TestVaultCliScraper().ParseGlobals(Fixture("root"))).IsEmpty();
    }

    [Test]
    public async Task Captured_Help_Preserves_Public_Key_Value_And_Masks_Root_Credentials()
    {
        var command = await ParseFixture("operator generate-root");
        var key = command.Options.Single(option => option.SwitchName == "-pgp-key");
        await Assert.That(key.IsFlag).IsFalse();
        await Assert.That(key.IsSecret).IsFalse();
        await Assert.That(command.Options.Single(option => option.SwitchName == "-otp").IsSecret).IsTrue();
        await Assert.That(command.Options.Single(option => option.SwitchName == "-decode").IsSecret).IsTrue();
    }

    [Test]
    public async Task Login_Authentication_Arguments_Are_Secret()
    {
        var command = await ParseFixture("login");
        await Assert.That(command.PositionalArguments.Single().IsSecret).IsTrue();
    }

    [Test]
    public async Task Tpm_Enrollment_Metadata_Is_Repeatable_Without_Repeatability_Prose()
    {
        var helpText = File.ReadAllText(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Vault", "2.2.0", "tpm-enroll.txt"));
        var command = await new TestVaultCliScraper().ParseGroup(["vault", "tpm", "enroll"], helpText);
        var metadata = command!.Options.Single(option => option.SwitchName == "-metadata");

        await Assert.That(metadata.AcceptsMultipleValues).IsTrue();
        await Assert.That(metadata.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(metadata.ValueSeparator).IsEqualTo("=");
        await Assert.That(metadata.IsFlag).IsFalse();
        await Assert.That(command.Options.Single(option => option.SwitchName == "-name").CSharpType)
            .IsEqualTo("string?");

        var tool = new CliToolDefinition
        {
            ToolName = "vault",
            NamespacePrefix = "Vault",
            TargetNamespace = "ModularPipelines.Vault",
            OutputDirectory = "src/ModularPipelines.Vault",
            Commands = [command],
        };
        var files = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool))
            .Concat(await new OptionsClassGenerator().GenerateAsync(tool));
        var assembly = PackageManagerGlobalRenderingTests.Compile(files.Select(file => file.Content));
        var type = assembly.GetType("ModularPipelines.Vault.Options.VaultTpmEnrollOptions")!;
        var instance = Activator.CreateInstance(type)!;
        var property = type.GetProperty("Metadata")!;
        property.SetValue(instance, TpmMetadataValues);
        var part = new OptionPart(property.Name, property.GetValue, property.GetCustomAttribute<CliOptionAttribute>()!);

        await Assert.That(new CommandArgumentBuilder().BuildArguments([part], instance))
            .IsEquivalentTo(["-metadata=owner=platform team", "-metadata=region=west"],
                TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Tpm_Attestation_Requires_Role_Without_Requiring_Conditional_Options()
    {
        var help = File.ReadAllText(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Vault", "2.2.0", "tpm-attest.txt"));
        var command = (await new TestVaultCliScraper().ParseGroup(["vault", "tpm", "attest"], help))!;
        await Assert.That(command.Options.Where(option => option.IsRequired).Select(option => option.SwitchName))
            .IsEquivalentTo(["-role-name"]);

        var tool = new CliToolDefinition
        {
            ToolName = "vault",
            NamespacePrefix = "Vault",
            TargetNamespace = "ModularPipelines.Vault",
            OutputDirectory = "src/ModularPipelines.Vault",
            Commands = [command],
        };
        var files = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool))
            .Concat(await new OptionsClassGenerator().GenerateAsync(tool));
        var assembly = PackageManagerGlobalRenderingTests.Compile(files.Select(file => file.Content));
        var type = assembly.GetType("ModularPipelines.Vault.Options.VaultTpmAttestOptions")!;
        await Assert.That(type.GetConstructor(Type.EmptyTypes)).IsNull();
        var constructor = type.GetConstructor([typeof(string)])!;
        var instance = constructor.Invoke(["build"]);
        var property = type.GetProperty("RoleName")!;
        var part = new OptionPart(property.Name, property.GetValue, property.GetCustomAttribute<CliOptionAttribute>()!);
        await Assert.That(new CommandArgumentBuilder().BuildArguments([part], instance))
            .IsEquivalentTo(["-role-name=build"]);

        var serviceInterface = (await new ServiceInterfaceGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(serviceInterface).Contains("TpmAttestAsync(VaultTpmAttestOptions options,");
        var service = (await new ServiceImplementationGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(service).DoesNotContain("new VaultTpmAttestOptions()");
    }

    [Test]
    public async Task Tpm_Group_Renders_Multiple_Trailing_Arguments_As_Separate_Tokens()
    {
        var help = File.ReadAllText(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Vault", "2.2.0", "tpm.txt"));
        var command = (await new TestVaultCliScraper().ParseGroup(["vault", "tpm"], help))!;
        var subcommand = command.PositionalArguments.Single(argument => argument.PropertyName == "Subcommand");
        await Assert.That(subcommand.IsRequired).IsTrue();
        await Assert.That(subcommand.IsVariadic).IsFalse();
        await Assert.That(subcommand.CSharpType).IsEqualTo("string");
        var args = command.PositionalArguments.Single(argument => argument.PropertyName == "Args");
        await Assert.That(args.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(args.IsRequired).IsFalse();
        await Assert.That(args.IsVariadic).IsTrue();
        await Assert.That(args.Phase).IsEqualTo(CommandLinePhase.Passthrough);

        var tool = new CliToolDefinition
        {
            ToolName = "vault",
            NamespacePrefix = "Vault",
            TargetNamespace = "ModularPipelines.Vault",
            OutputDirectory = "src/ModularPipelines.Vault",
            Commands = [command],
        };
        var files = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool))
            .Concat(await new OptionsClassGenerator().GenerateAsync(tool));
        var assembly = PackageManagerGlobalRenderingTests.Compile(files.Select(file => file.Content));
        var type = assembly.GetType("ModularPipelines.Vault.Options.VaultTpmOptions")!;
        var instance = Activator.CreateInstance(type, "attest")!;
        var parts = new List<PropertyCommandLinePart>();
        foreach (var argument in command.PositionalArguments)
        {
            var property = type.GetProperty(argument.PropertyName)!;
            var attribute = property.GetCustomAttribute<CliArgumentAttribute>()!;
            parts.Add(new ArgumentPart(property.Name, property.GetValue, attribute) { HasExplicitPosition = true });
        }

        await Assert.That(new CommandArgumentBuilder().BuildArguments(parts, instance))
            .IsEquivalentTo(["attest"]);
        type.GetProperty("Args")!.SetValue(instance, TpmPassthroughValues);
        await Assert.That(new CommandArgumentBuilder().BuildArguments(parts, instance))
            .IsEquivalentTo(["attest", "-role-name=build team", "-mount-path=auth/custom"],
                TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    private static string Fixture(string command, string version = "2.1.1") => File.ReadAllText(Path.Combine(
        AppContext.BaseDirectory, "Fixtures", "Vault", version, command.Replace(' ', '-') + ".txt"));

    [Test]
    [Arguments("print token", true)]
    [Arguments("token create", false)]
    [Arguments("read", false)]
    public async Task Only_Print_Token_Suppresses_Output_Logging_By_Default(string commandName, bool expected)
    {
        var command = await ParseFixture(commandName);
        await Assert.That(command.SuppressOutputLoggingByDefault).IsEqualTo(expected);
    }

    private static async Task<CliCommandDefinition> ParseFixture(string command, string version = "2.1.1") =>
        (await new TestVaultCliScraper().ParseGroup(["vault", .. command.Split(' ')], Fixture(command, version)))!;

    private sealed class TestVaultCliScraper : VaultCliScraper
    {
        public TestVaultCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<VaultCliScraper>.Instance)
        {
        }

        public IReadOnlyList<CliOptionDefinition> ParseGlobals(string helpText) => ParseGlobalOptions(helpText);

        public Task<CliCommandDefinition?> ParseGroup(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }

    private sealed class VaultHelpExecutor(string? leafHelp = null) : ICliCommandExecutor
    {
        private static readonly Dictionary<string, string> Responses =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["--help"] = """
                    Usage: vault <command> [args]

                    Other commands:
                        audit       Interact with audit devices
                    """,
                ["audit --help"] = """
                    Usage: vault audit <subcommand> [options] [args]

                    This command groups subcommands for interacting with Vault's audit devices.

                    Subcommands:
                        disable    Disables an audit device
                    """,
                ["audit disable --help"] = """
                    Usage: vault audit disable [options]

                    Options:
                        -address=<string>    Address of the Vault server
                    """,
            };

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            string? workingDirectory = null,
            CancellationToken cancellationToken = default)
        {
            if (!Responses.TryGetValue(arguments, out var response))
            {
                throw new InvalidOperationException($"Unexpected invocation: {command} {arguments}");
            }

            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = arguments == "audit disable --help" && leafHelp is not null ? leafHelp : response,
                StandardError = string.Empty,
                ExitCode = 0,
            });
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
