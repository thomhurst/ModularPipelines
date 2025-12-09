using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("deployment", "tenant", "export")]
public record AzDeploymentTenantExportOptions(
[property: CliOption("--name")] string Name
) : AzOptions;