using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp", "list-consumption-locations")]
public record AzFunctionappListConsumptionLocationsOptions : AzOptions;