using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("automanage", "service-principal", "list")]
public record AzAutomanageServicePrincipalListOptions : AzOptions;