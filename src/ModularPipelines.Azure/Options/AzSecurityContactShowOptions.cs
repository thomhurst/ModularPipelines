using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "contact", "show")]
public record AzSecurityContactShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;