using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "config", "ssl", "show")]
public record AzFunctionappConfigSslShowOptions(
[property: CommandSwitch("--certificate-name")] string CertificateName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}