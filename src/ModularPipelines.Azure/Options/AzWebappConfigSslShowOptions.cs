using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "config", "ssl", "show")]
public record AzWebappConfigSslShowOptions(
[property: CommandSwitch("--certificate-name")] string CertificateName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}