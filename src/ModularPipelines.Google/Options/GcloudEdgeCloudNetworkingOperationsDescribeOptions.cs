using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "operations", "describe")]
public record GcloudEdgeCloudNetworkingOperationsDescribeOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Location
) : GcloudOptions;