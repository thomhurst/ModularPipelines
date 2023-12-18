using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloud-service", "list")]
public record AzCloudServiceListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}