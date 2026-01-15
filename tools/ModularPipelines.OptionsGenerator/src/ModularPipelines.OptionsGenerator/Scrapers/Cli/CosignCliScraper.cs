using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Cosign - container signing tool from Sigstore.
/// Cosign uses Cobra for its CLI (Go-based).
///
/// cosign help format (cosign --help):
/// A tool for Container Signing, Verification and Storage in an OCI registry.
///
/// Usage:
///   cosign [command]
///
/// Available Commands:
///   attach         Attach a signature or sbom to a container image
///   attest         Attest the supplied container image
///   clean          Remove all signatures from an image
///   completion     Generate completion script
///   copy           Copy the supplied container image and signatures
///   dockerfile     Verify a Dockerfile FROM reference image
///   download       Provides utilities for downloading artifacts and attached artifacts in a registry
///   env            Prints Cosign environment variables
///   generate       Generates (unsigned) signature payloads from the supplied container image
///   help           Help about any command
///   import-key-pair Imports a PEM-encoded RSA or EC private key
///   initialize     Initializes SigStore root to retrieve TUF targets
///   load           Load a signed image on disk to a remote registry
///   login          Log in to a registry
///   manifest       Provides utilities for discovering images in a registry
///   piv-tool       Provides utilities for working with PIV tokens
///   pkcs11-tool    Provides utilities for working with PKCS11 tokens
///   policy         Subcommand to manage policies
///   public-key     Gets a public key from the key-pair
///   save           Save the container image and associated signatures to disk
///   sign           Sign the supplied container image
///   sign-blob      Sign the supplied blob, outputting the base64-encoded signature to stdout
///   tree           Display supply chain security related artifacts for an image
///   triangulate    Find out where the signature lives
///   upload         Provides utilities for uploading artifacts to a registry
///   verify         Verify a signature on the supplied container image
///   verify-attestation Verify an attestation on the supplied container image
///   verify-blob    Verify a signature on the supplied blob
///   version        Prints the version
/// </summary>
public partial class CosignCliScraper : CobraCliScraper
{
    public CosignCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<CosignCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "cosign";

    public override string NamespacePrefix => "Cosign";

    public override string TargetNamespace => "ModularPipelines.Cosign";

    public override string OutputDirectory => "src/ModularPipelines.Cosign";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version", "env"
    };
}
