using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network", "pccp", "version", "list")]
public record AzMobileNetworkPccpVersionListOptions(
[property: CommandSwitch("--version-name")] string VersionName
) : AzOptions
{
}