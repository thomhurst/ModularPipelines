using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "app", "append-loaded-public-certificate")]
public record AzSpringAppAppendLoadedPublicCertificateOptions(
[property: CliOption("--certificate-name")] string CertificateName,
[property: CliFlag("--load-trust-store")] bool LoadTrustStore,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions;