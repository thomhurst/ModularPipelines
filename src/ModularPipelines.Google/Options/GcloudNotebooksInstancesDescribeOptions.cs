using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notebooks", "instances", "describe")]
public record GcloudNotebooksInstancesDescribeOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Location
) : GcloudOptions;