using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "config", "ssl", "delete")]
public record AzFunctionappConfigSslDeleteOptions(
[property: CommandSwitch("--certificate-thumbprint")] string CertificateThumbprint,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;