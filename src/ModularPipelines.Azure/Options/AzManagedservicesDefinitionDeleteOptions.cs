using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedservices", "definition", "delete")]
public record AzManagedservicesDefinitionDeleteOptions(
[property: CommandSwitch("--definition")] string Definition
) : AzOptions
{
}

