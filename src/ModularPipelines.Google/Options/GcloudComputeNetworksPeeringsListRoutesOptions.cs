using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "peerings", "list-routes")]
public record GcloudComputeNetworksPeeringsListRoutesOptions(
[property: CliArgument] string Name,
[property: CliOption("--direction")] string Direction,
[property: CliOption("--network")] string Network,
[property: CliOption("--region")] string Region
) : GcloudOptions;