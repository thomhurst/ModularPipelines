using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "operations", "wait")]
public record GcloudEdgeCloudNetworkingOperationsWaitOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location
) : GcloudOptions;