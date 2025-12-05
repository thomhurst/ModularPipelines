using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("looker", "instances", "describe")]
public record GcloudLookerInstancesDescribeOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Region
) : GcloudOptions;