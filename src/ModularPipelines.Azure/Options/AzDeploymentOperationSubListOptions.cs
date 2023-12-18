using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "operation", "sub", "list")]
public record AzDeploymentOperationSubListOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}

