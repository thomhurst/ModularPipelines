using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("provider", "operation", "list")]
public record AzProviderOperationListOptions(
[property: CommandSwitch("--namespace")] string Namespace
) : AzOptions
{
}