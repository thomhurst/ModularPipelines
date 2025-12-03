using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "subnets", "list-usable")]
public record GcloudComputeNetworksSubnetsListUsableOptions : GcloudOptions;