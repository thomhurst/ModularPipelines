using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "service", "country", "list")]
public record AzPeeringServiceCountryListOptions : AzOptions;