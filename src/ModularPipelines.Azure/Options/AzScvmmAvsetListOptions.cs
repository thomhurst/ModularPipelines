using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scvmm", "avset", "list")]
public record AzScvmmAvsetListOptions(
[property: CommandSwitch("--availability-set-name")] string AvailabilitySetName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}