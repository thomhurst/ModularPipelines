using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "period", "show")]
public record AzBillingPeriodShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;