using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automanage", "service-principal", "show-default")]
public record AzAutomanageServicePrincipalShowDefaultOptions : AzOptions;