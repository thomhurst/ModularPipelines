using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tag", "remove-value")]
public record AzTagRemoveValueOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--value")] string Value
) : AzOptions;