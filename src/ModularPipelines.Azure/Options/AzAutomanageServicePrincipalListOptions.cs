using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("automanage", "service-principal", "list")]
public record AzAutomanageServicePrincipalListOptions : AzOptions;