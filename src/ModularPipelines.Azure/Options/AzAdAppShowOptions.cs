using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "show")]
public record AzAdAppShowOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions;