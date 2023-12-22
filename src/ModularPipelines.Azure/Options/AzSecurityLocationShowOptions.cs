using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "location", "show")]
public record AzSecurityLocationShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;