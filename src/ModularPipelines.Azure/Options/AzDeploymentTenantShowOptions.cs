using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment", "tenant", "show")]
public record AzDeploymentTenantShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;