using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "perimeters", "dry-run", "update")]
public record GcloudAccessContextManagerPerimetersDryRunUpdateOptions(
[property: PositionalArgument] string Perimeter,
[property: PositionalArgument] string Policy
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--add-access-levels")]
    public string[]? AddAccessLevels { get; set; }

    [BooleanCommandSwitch("--clear-access-levels")]
    public bool? ClearAccessLevels { get; set; }

    [CommandSwitch("--remove-access-levels")]
    public string[]? RemoveAccessLevels { get; set; }

    [CommandSwitch("--add-resources")]
    public string[]? AddResources { get; set; }

    [BooleanCommandSwitch("--clear-resources")]
    public bool? ClearResources { get; set; }

    [CommandSwitch("--remove-resources")]
    public string[]? RemoveResources { get; set; }

    [CommandSwitch("--add-restricted-services")]
    public string[]? AddRestrictedServices { get; set; }

    [BooleanCommandSwitch("--clear-restricted-services")]
    public bool? ClearRestrictedServices { get; set; }

    [CommandSwitch("--remove-restricted-services")]
    public string[]? RemoveRestrictedServices { get; set; }

    [BooleanCommandSwitch("--clear-egress-policies")]
    public bool? ClearEgressPolicies { get; set; }

    [CommandSwitch("--set-egress-policies")]
    public string? SetEgressPolicies { get; set; }

    [BooleanCommandSwitch("--clear-ingress-policies")]
    public bool? ClearIngressPolicies { get; set; }

    [CommandSwitch("--set-ingress-policies")]
    public string? SetIngressPolicies { get; set; }

    [BooleanCommandSwitch("--enable-vpc-accessible-services")]
    public bool? EnableVpcAccessibleServices { get; set; }

    [CommandSwitch("--add-vpc-allowed-services")]
    public string[]? AddVpcAllowedServices { get; set; }

    [BooleanCommandSwitch("--clear-vpc-allowed-services")]
    public bool? ClearVpcAllowedServices { get; set; }

    [CommandSwitch("--remove-vpc-allowed-services")]
    public string[]? RemoveVpcAllowedServices { get; set; }
}