using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "tags", "values", "list")]
public record GcloudResourceManagerTagsValuesListOptions(
[property: CommandSwitch("--parent")] string Parent
) : GcloudOptions;