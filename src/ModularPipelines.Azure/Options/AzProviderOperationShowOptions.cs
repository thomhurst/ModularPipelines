using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("provider", "operation", "show")]
public record AzProviderOperationShowOptions(
[property: CommandSwitch("--namespace")] string Namespace
) : AzOptions
{
}

