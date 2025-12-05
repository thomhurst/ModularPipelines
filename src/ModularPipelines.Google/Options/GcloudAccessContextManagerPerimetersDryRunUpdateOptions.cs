using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "perimeters", "dry-run", "update")]
public record GcloudAccessContextManagerPerimetersDryRunUpdateOptions(
[property: CliArgument] string Perimeter,
[property: CliArgument] string Policy
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--add-access-levels")]
    public string[]? AddAccessLevels { get; set; }

    [CliFlag("--clear-access-levels")]
    public bool? ClearAccessLevels { get; set; }

    [CliOption("--remove-access-levels")]
    public string[]? RemoveAccessLevels { get; set; }

    [CliOption("--add-resources")]
    public string[]? AddResources { get; set; }

    [CliFlag("--clear-resources")]
    public bool? ClearResources { get; set; }

    [CliOption("--remove-resources")]
    public string[]? RemoveResources { get; set; }

    [CliOption("--add-restricted-services")]
    public string[]? AddRestrictedServices { get; set; }

    [CliFlag("--clear-restricted-services")]
    public bool? ClearRestrictedServices { get; set; }

    [CliOption("--remove-restricted-services")]
    public string[]? RemoveRestrictedServices { get; set; }

    [CliFlag("--clear-egress-policies")]
    public bool? ClearEgressPolicies { get; set; }

    [CliOption("--set-egress-policies")]
    public string? SetEgressPolicies { get; set; }

    [CliFlag("--clear-ingress-policies")]
    public bool? ClearIngressPolicies { get; set; }

    [CliOption("--set-ingress-policies")]
    public string? SetIngressPolicies { get; set; }

    [CliFlag("--enable-vpc-accessible-services")]
    public bool? EnableVpcAccessibleServices { get; set; }

    [CliOption("--add-vpc-allowed-services")]
    public string[]? AddVpcAllowedServices { get; set; }

    [CliFlag("--clear-vpc-allowed-services")]
    public bool? ClearVpcAllowedServices { get; set; }

    [CliOption("--remove-vpc-allowed-services")]
    public string[]? RemoveVpcAllowedServices { get; set; }
}