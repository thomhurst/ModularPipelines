using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Syft - SBOM (Software Bill of Materials) generator from Anchore.
/// Syft uses Cobra for its CLI (Go-based).
///
/// syft help format (syft --help):
/// Generate a packaged-based Software Bill Of Materials (SBOM) from container images and filesystems
///
/// Usage:
///   syft [flags] [SOURCE]
///   syft [command]
///
/// Available Commands:
///   attest      Generate an SBOM as an attestation for the given [SOURCE] container image
///   completion  Generate a shell completion script
///   convert     Convert between SBOM formats
///   help        Help about any command
///   login       Log in to a registry
///   packages    Generate a package SBOM
///   scan        Generate an SBOM
///   version     Show the version
///
/// Syft is a CLI tool and Go library for generating Software Bill of Materials (SBOM)
/// from container images and filesystems. It supports multiple SBOM formats including
/// SPDX, CycloneDX, and Syft's native JSON format.
/// </summary>
public partial class SyftCliScraper : CobraCliScraper
{
    public SyftCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<SyftCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "syft";

    public override string NamespacePrefix => "Syft";

    public override string TargetNamespace => "ModularPipelines.Syft";

    public override string OutputDirectory => "src/ModularPipelines.Syft";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version"
    };
}
