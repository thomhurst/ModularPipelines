using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class CosignCliScraperTests
{
    [Test]
    public async Task Extracts_Unindented_Cosign_V3_Subcommands()
    {
        const string helpText = """
            A tool for Container Signing, Verification and Storage in an OCI registry

            Usage:
            cosign [command]

            Available Commands:
            attest                  Attest the supplied container image
            sign                    Sign the supplied container image
            verify                  Verify a signature on the supplied container image
            verify-blob-attestation Verify an attestation on the supplied blob

            Flags:
                -h, --help=false:
            """;
        var scraper = new TestCosignCliScraper();

        var subcommands = scraper.Extract(helpText);

        await Assert.That(subcommands).IsEquivalentTo(
            ["attest", "sign", "verify", "verify-blob-attestation"]);
    }

    [Test]
    public async Task Extracts_Cosign_V3_All_Word_Command_Table()
    {
        const string helpText = """
            Tools for interacting with a Sigstore protobuf bundle

            Usage:
            cosign bundle [command]

            Available Commands:
            create      Create a Sigstore protobuf bundle
            inspect     Inspect a Sigstore protobuf bundle
            upgrade     Upgrade a Sigstore protobuf bundle

            Flags:
                -h, --help=false:
            """;

        var subcommands = new TestCosignCliScraper().Extract(helpText);

        await Assert.That(subcommands).IsEquivalentTo(["create", "inspect", "upgrade"]);
    }

    [Test]
    public async Task Does_Not_Treat_Description_Ending_In_Commands_As_Command_Section()
    {
        const string helpText = """
            List installed extension commands

            USAGE
              cosign extension list [flags]

            LEARN MORE
              Use `cosign <command> <subcommand> --help` for more information.
            """;

        var subcommands = new TestCosignCliScraper().Extract(helpText);

        await Assert.That(subcommands).IsEmpty();
    }

    [Test]
    [Arguments("signing-config", "signing config")]
    [Arguments("trusted-root", "trusted root")]
    public async Task Extracts_Cosign_V3_Single_Row_All_Word_Command_Tables(
        string commandGroup,
        string description)
    {
        var helpText = $"""
            Tools for interacting with a Sigstore protobuf {description}

            Usage:
            cosign {commandGroup} [command]

            Available Commands:
            create      Create a Sigstore protobuf {description}

            Flags:
                -h, --help=false:
            """;

        var subcommands = new TestCosignCliScraper().Extract(helpText);

        await Assert.That(subcommands).IsEquivalentTo(["create"]);
    }

    [Test]
    public async Task Parses_Cosign_V3_Default_Value_Flag_Format()
    {
        const string helpText = """
            Sign the supplied container image.

            Usage:
            cosign sign [flags]

            Flags:
                --annotations=[]:
                        extra key=value pairs to sign

                -h, --help=false:
                        help for sign

                --registry-referrers-mode=:
                        mode for fetching references from the registry

                --upload=true:
                        whether to upload the signature
            """;
        var command = await new TestCosignCliScraper().Parse(["cosign", "sign"], helpText);

        await Assert.That(command).IsNotNull();
        await Assert.That(GetOption(command!, "--annotations").CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(GetOption(command, "--annotations").AcceptsMultipleValues).IsTrue();
        await Assert.That(GetOption(command, "--help").IsFlag).IsTrue();
        await Assert.That(GetOption(command, "--registry-referrers-mode").IsFlag).IsFalse();
        await Assert.That(GetOption(command, "--upload").CSharpType).IsEqualTo("bool?");
        await Assert.That(GetOption(command, "--upload").IsFlag).IsFalse();
        await Assert.That(GetOption(command, "--upload").ValueSeparator).IsEqualTo("=");
    }

    [Test]
    public async Task Adds_Omitted_Positionals_And_Marks_Hardware_Credentials()
    {
        var scraper = new TestCosignCliScraper();

        var signArguments = scraper.ApplyFix(["sign"], []);
        var loginArguments = scraper.ApplyFix(
            ["login"],
            [OptionalArgument("Options", 0), OptionalArgument("Server", 1)]);
        var downloadArguments = scraper.ApplyFix(["download", "attestation"], []);

        await Assert.That(signArguments).Count().IsEqualTo(1);
        await Assert.That(signArguments[0].PropertyName).IsEqualTo("Images");
        await Assert.That(signArguments[0].CSharpType).IsEqualTo("IEnumerable<string>");
        await Assert.That(signArguments[0].IsRequired).IsTrue();
        await Assert.That(loginArguments).Count().IsEqualTo(1);
        await Assert.That(loginArguments[0].PropertyName).IsEqualTo("Server");
        await Assert.That(downloadArguments.Single().PropertyName).IsEqualTo("Image");
        await Assert.That(scraper.IsSecret("NewPin", isFlag: false)).IsTrue();
        await Assert.That(scraper.IsSecret("OldKey", isFlag: false)).IsTrue();
        await Assert.That(scraper.IsSecret("IdentityToken", isFlag: false)).IsTrue();
        await Assert.That(scraper.IsSecret("OidcClientSecretFile", isFlag: false)).IsTrue();
        await Assert.That(scraper.IsSecret("NewPin", isFlag: true)).IsFalse();
        await Assert.That(scraper.IsSecret("IdentityToken", isFlag: true)).IsFalse();
    }

    [Test]
    public async Task Extracts_GitVersion_From_Version_Banner()
    {
        const string output = """
            ______   ______        _______. __    _______ .__   __.
            cosign: A tool for Container Signing, Verification and Storage in an OCI registry

            GitVersion:    v3.1.3
            GitCommit:     11926fa5bbbbde47e88fc006b625a17769
            """;

        var version = new TestCosignCliScraper().ParseVersion(output);

        await Assert.That(version).IsEqualTo("v3.1.3");
    }

    [Test]
    public async Task Keeps_Attestation_Predicate_Uri_As_String()
    {
        const string helpText = """
            Attest the supplied container image.

            Usage:
            cosign attest [flags]

            Flags:
                --type=:
                        specify a predicate type (slsaprovenance|spdx|custom) or an URI
            """;

        var command = await new TestCosignCliScraper().Parse(["cosign", "attest"], helpText);
        var option = GetOption(command!, "--type");

        await Assert.That(option.CSharpType).IsEqualTo("string?");
        await Assert.That(option.EnumDefinition).IsNull();
    }

    private static CliOptionDefinition GetOption(CliCommandDefinition command, string switchName) =>
        command.Options.Single(option => option.SwitchName == switchName);

    private static CliPositionalArgument OptionalArgument(string propertyName, int position) => new()
    {
        PropertyName = propertyName,
        CSharpType = "string?",
        IsRequired = false,
        PositionIndex = position,
    };

    private sealed class TestCosignCliScraper : CosignCliScraper
    {
        public TestCosignCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<CosignCliScraper>.Instance)
        {
        }

        public IReadOnlyList<string> Extract(string helpText) => ExtractSubcommands(helpText).ToList();

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }

        public IReadOnlyList<CliPositionalArgument> ApplyFix(
            string[] commandParts,
            IReadOnlyList<CliPositionalArgument> positionalArguments) =>
            ApplyPositionalArgumentFixes(commandParts, positionalArguments);

        public bool IsSecret(string propertyName, bool isFlag) => IsSecretOption(propertyName, isFlag, string.Empty);

        public string? ParseVersion(string standardOutput) => ParseVersionOutput(new CliCommandResult
        {
            StandardOutput = standardOutput,
            StandardError = string.Empty,
            ExitCode = 0,
        });
    }
}
