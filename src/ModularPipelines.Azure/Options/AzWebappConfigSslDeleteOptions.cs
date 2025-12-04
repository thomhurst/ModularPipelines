using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp", "config", "ssl", "delete")]
public record AzWebappConfigSslDeleteOptions(
[property: CliOption("--certificate-thumbprint")] string CertificateThumbprint,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;