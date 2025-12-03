using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment", "operation", "tenant", "show")]
public record AzDeploymentOperationTenantShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--operation-ids")] string OperationIds
) : AzOptions;