using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "instances", "describe")]
public record GcloudBigtableInstancesDescribeOptions(
[property: CliArgument] string Instance
) : GcloudOptions;