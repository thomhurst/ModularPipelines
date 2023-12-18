using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmss", "show")]
public record AzVmssShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--include-user-data")]
    public bool? IncludeUserData { get; set; }

    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }
}