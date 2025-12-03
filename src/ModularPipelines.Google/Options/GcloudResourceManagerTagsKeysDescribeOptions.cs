using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "tags", "keys", "describe")]
public record GcloudResourceManagerTagsKeysDescribeOptions(
[property: CliArgument] string ResourceName
) : GcloudOptions;