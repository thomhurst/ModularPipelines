using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "peerings", "delete")]
public record GcloudComputeNetworksPeeringsDeleteOptions(
[property: CliArgument] string Name,
[property: CliOption("--network")] string Network
) : GcloudOptions;