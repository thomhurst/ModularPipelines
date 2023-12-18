using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedservices", "definition", "list")]
public record AzManagedservicesDefinitionListOptions(
[property: CommandSwitch("--definition")] string Definition
) : AzOptions
{
}