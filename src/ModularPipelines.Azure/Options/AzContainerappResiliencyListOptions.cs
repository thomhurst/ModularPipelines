using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "resiliency", "list")]
public record AzContainerappResiliencyListOptions(
[property: CommandSwitch("--container-app-name")] string ContainerAppName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}