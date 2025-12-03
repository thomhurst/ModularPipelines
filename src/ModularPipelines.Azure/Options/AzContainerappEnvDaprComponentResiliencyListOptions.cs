using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "env", "dapr-component", "resiliency", "list")]
public record AzContainerappEnvDaprComponentResiliencyListOptions(
[property: CliOption("--dapr-component-name")] string DaprComponentName,
[property: CliOption("--environment")] string Environment,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;