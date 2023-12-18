using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("group", "show")]
public record AzGroupShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;