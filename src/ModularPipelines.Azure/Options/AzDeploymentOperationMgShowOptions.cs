using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "operation", "mg", "show")]
public record AzDeploymentOperationMgShowOptions(
[property: CommandSwitch("--management-group-id")] string ManagementGroupId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--operation-ids")] string OperationIds
) : AzOptions;