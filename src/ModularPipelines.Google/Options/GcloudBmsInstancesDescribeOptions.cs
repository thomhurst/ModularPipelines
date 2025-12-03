using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "instances", "describe")]
public record GcloudBmsInstancesDescribeOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Region
) : GcloudOptions;