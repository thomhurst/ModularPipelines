using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network", "pccp", "version", "show")]
public record AzMobileNetworkPccpVersionShowOptions(
[property: CommandSwitch("--version-name")] string VersionName
) : AzOptions;