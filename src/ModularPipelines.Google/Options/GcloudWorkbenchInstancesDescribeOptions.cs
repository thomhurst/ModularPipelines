using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workbench", "instances", "describe")]
public record GcloudWorkbenchInstancesDescribeOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Location
) : GcloudOptions;