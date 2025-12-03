using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "resiliency", "list")]
public record AzContainerappResiliencyListOptions(
[property: CliOption("--container-app-name")] string ContainerAppName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;