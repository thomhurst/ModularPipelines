using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "operation", "list")]
public record AzDeploymentOperationListOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;