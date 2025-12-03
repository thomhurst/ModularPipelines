using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("automanage", "service-principal", "show-default")]
public record AzAutomanageServicePrincipalShowDefaultOptions : AzOptions;