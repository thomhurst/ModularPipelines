using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("functionapp", "config", "ssl", "delete")]
public record AzFunctionappConfigSslDeleteOptions(
[property: CliOption("--certificate-thumbprint")] string CertificateThumbprint,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;