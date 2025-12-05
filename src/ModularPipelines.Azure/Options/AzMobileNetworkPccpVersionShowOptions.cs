using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mobile-network", "pccp", "version", "show")]
public record AzMobileNetworkPccpVersionShowOptions(
[property: CliOption("--version-name")] string VersionName
) : AzOptions;