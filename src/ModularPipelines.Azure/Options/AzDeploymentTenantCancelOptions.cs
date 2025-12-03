using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment", "tenant", "cancel")]
public record AzDeploymentTenantCancelOptions(
[property: CliOption("--name")] string Name
) : AzOptions;