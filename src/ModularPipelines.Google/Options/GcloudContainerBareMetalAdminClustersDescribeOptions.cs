using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "bare-metal", "admin-clusters", "describe")]
public record GcloudContainerBareMetalAdminClustersDescribeOptions(
[property: PositionalArgument] string AdminCluster,
[property: PositionalArgument] string Location
) : GcloudOptions;