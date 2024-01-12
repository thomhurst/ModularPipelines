using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "instances", "describe")]
public record GcloudBmsInstancesDescribeOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Region
) : GcloudOptions;