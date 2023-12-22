using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "group", "show")]
public record AzAdGroupShowOptions(
[property: CommandSwitch("--group")] string Group
) : AzOptions;