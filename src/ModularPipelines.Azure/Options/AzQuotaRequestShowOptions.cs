using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quota", "request", "show")]
public record AzQuotaRequestShowOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--scope")] string Scope
) : AzOptions
{
}

