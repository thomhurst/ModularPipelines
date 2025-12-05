using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "container", "machines", "describe")]
public record GcloudEdgeCloudContainerMachinesDescribeOptions(
[property: CliArgument] string Machine,
[property: CliArgument] string Location
) : GcloudOptions;