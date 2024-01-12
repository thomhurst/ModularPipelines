using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "tags", "keys", "describe")]
public record GcloudResourceManagerTagsKeysDescribeOptions(
[property: PositionalArgument] string ResourceName
) : GcloudOptions;