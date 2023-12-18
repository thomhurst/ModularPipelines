using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network", "service", "create")]
public record AzMobileNetworkServiceCreateOptions(
[property: CommandSwitch("--mobile-network-name")] string MobileNetworkName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--pcc-rules")] string PccRules,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-precedence")] string ServicePrecedence
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--service-qos-policy")]
    public string? ServiceQosPolicy { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}