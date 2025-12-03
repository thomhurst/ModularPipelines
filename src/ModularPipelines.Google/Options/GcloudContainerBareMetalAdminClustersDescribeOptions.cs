using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "bare-metal", "admin-clusters", "describe")]
public record GcloudContainerBareMetalAdminClustersDescribeOptions(
[property: CliArgument] string AdminCluster,
[property: CliArgument] string Location
) : GcloudOptions;