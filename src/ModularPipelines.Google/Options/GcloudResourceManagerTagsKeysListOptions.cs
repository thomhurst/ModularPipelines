using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "tags", "keys", "list")]
public record GcloudResourceManagerTagsKeysListOptions(
[property: CommandSwitch("--parent")] string Parent
) : GcloudOptions;