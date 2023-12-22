using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "list-consumption-locations")]
public record AzFunctionappListConsumptionLocationsOptions : AzOptions;