using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "releases", "describe")]
public record GcloudDeployReleasesDescribeOptions(
[property: CliArgument] string Release,
[property: CliArgument] string DeliveryPipeline,
[property: CliArgument] string Region
) : GcloudOptions;