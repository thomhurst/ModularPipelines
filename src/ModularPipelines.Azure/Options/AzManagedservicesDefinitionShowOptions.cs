using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedservices", "definition", "show")]
public record AzManagedservicesDefinitionShowOptions(
[property: CommandSwitch("--definition")] string Definition
) : AzOptions
{
}