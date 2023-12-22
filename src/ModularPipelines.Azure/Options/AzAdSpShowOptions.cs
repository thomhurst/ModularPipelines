using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "sp", "show")]
public record AzAdSpShowOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions;