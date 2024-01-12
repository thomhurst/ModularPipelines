using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "tags", "values", "describe")]
public record GcloudResourceManagerTagsValuesDescribeOptions(
[property: PositionalArgument] string ResourceName
) : GcloudOptions;