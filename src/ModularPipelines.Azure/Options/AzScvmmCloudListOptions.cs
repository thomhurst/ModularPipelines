using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scvmm", "cloud", "list")]
public record AzScvmmCloudListOptions(
[property: CommandSwitch("--cloud-name")] string CloudName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}