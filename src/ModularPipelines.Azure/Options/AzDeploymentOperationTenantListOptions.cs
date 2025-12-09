using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("deployment", "operation", "tenant", "list")]
public record AzDeploymentOperationTenantListOptions(
[property: CliOption("--name")] string Name
) : AzOptions;