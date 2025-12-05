using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "bare-metal", "clusters", "describe")]
public record GcloudContainerBareMetalClustersDescribeOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions;