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
        await Assert.That(scraper.IsSecret("NewPin", isFlag: true)).IsFalse();
    }

    private static CliOptionDefinition GetOption(CliCommandDefinition command, string switchName) =>
        command.Options.Single(option => option.SwitchName == switchName);

    private static CliPositionalArgument OptionalArgument(string propertyName, int position) => new()
    {
        PropertyName = propertyName,
        PlaceholderName = propertyName.ToUpperInvariant(),
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

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(commandPath, helpText, CancellationToken.None);

        public IReadOnlyList<CliPositionalArgument> ApplyFix(
            string[] commandParts,
            IReadOnlyList<CliPositionalArgument> positionalArguments) =>
            ApplyPositionalArgumentFixes(commandParts, positionalArguments);

        public bool IsSecret(string propertyName, bool isFlag) => IsSecretOption(propertyName, isFlag);
    }
}
