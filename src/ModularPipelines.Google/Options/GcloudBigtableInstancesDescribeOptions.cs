using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "instances", "describe")]
public record GcloudBigtableInstancesDescribeOptions(
[property: PositionalArgument] string Instance
) : GcloudOptions;