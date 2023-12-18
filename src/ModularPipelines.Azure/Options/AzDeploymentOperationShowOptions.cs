using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "operation", "show")]
public record AzDeploymentOperationShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--operation-ids")] string OperationIds
) : AzOptions;