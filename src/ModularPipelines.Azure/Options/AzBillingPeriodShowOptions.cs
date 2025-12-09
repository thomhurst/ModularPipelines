using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "period", "show")]
public record AzBillingPeriodShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;