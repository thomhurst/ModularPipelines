using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mobile-network", "pccp", "version", "list")]
public record AzMobileNetworkPccpVersionListOptions : AzOptions;