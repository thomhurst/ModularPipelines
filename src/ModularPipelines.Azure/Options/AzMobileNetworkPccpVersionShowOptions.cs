using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network", "pccp", "version", "show")]
public record AzMobileNetworkPccpVersionShowOptions(
[property: CommandSwitch("--version-name")] string VersionName
) : AzOptions
{
}

