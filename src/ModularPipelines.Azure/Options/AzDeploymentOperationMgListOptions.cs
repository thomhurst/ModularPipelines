using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "operation", "mg", "list")]
public record AzDeploymentOperationMgListOptions(
[property: CommandSwitch("--management-group-id")] string ManagementGroupId,
[property: CommandSwitch("--name")] string Name
) : AzOptions;