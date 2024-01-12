using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "container", "machines", "describe")]
public record GcloudEdgeCloudContainerMachinesDescribeOptions(
[property: PositionalArgument] string Machine,
[property: PositionalArgument] string Location
) : GcloudOptions;