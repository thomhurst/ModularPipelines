using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "period", "show")]
public record AzBillingPeriodShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}

