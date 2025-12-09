using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("deployment", "tenant", "cancel")]
public record AzDeploymentTenantCancelOptions(
[property: CliOption("--name")] string Name
) : AzOptions;