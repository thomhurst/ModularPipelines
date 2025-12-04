using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp", "config", "ssl", "show")]
public record AzWebappConfigSslShowOptions(
[property: CliOption("--certificate-name")] string CertificateName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;