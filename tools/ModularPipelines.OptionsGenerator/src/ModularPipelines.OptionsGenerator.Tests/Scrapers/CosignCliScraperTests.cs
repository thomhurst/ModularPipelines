using Microsoft.Extensions.Logging.Abstractions;
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

            Flags:
                -h, --help=false:
            """;
        var scraper = new TestCosignCliScraper();

        var subcommands = scraper.Extract(helpText);

        await Assert.That(subcommands).IsEquivalentTo(["attest", "sign", "verify"]);
    }

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
    }
}
