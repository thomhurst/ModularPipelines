using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp", "config", "ssl", "show")]
public record AzFunctionappConfigSslShowOptions(
[property: CliOption("--certificate-name")] string CertificateName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;