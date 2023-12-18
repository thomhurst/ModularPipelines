using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "config", "ssl", "delete")]
public record AzWebappConfigSslDeleteOptions(
[property: CommandSwitch("--certificate-thumbprint")] string CertificateThumbprint,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;