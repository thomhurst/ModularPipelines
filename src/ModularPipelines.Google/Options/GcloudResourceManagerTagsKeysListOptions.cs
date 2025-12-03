using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "tags", "keys", "list")]
public record GcloudResourceManagerTagsKeysListOptions(
[property: CliOption("--parent")] string Parent
) : GcloudOptions;