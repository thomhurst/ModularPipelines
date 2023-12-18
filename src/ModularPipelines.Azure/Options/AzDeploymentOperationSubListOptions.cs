using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "operation", "sub", "list")]
public record AzDeploymentOperationSubListOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;