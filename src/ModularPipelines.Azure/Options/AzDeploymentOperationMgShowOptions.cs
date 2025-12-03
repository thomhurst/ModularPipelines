using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment", "operation", "mg", "show")]
public record AzDeploymentOperationMgShowOptions(
[property: CliOption("--management-group-id")] string ManagementGroupId,
[property: CliOption("--name")] string Name,
[property: CliOption("--operation-ids")] string OperationIds
) : AzOptions;