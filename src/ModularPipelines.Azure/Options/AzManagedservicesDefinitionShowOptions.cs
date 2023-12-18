using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedservices", "definition", "show")]
public record AzManagedservicesDefinitionShowOptions(
[property: CommandSwitch("--definition")] string Definition
) : AzOptions;