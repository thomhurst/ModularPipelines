using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tag", "update")]
public record AzTagUpdateOptions(
[property: CommandSwitch("--operation")] string Operation,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--tags")] string Tags
) : AzOptions;