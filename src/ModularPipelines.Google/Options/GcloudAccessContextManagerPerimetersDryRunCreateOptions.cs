using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "perimeters", "dry-run", "create")]
public record GcloudAccessContextManagerPerimetersDryRunCreateOptions(
[property: PositionalArgument] string Perimeter,
[property: PositionalArgument] string Policy,
[property: CommandSwitch("--access-levels")] string[] AccessLevels,
[property: CommandSwitch("--egress-policies")] string EgressPolicies,
[property: CommandSwitch("--ingress-policies")] string IngressPolicies,
[property: CommandSwitch("--resources")] string[] Resources,
[property: CommandSwitch("--restricted-services")] string[] RestrictedServices,
[property: BooleanCommandSwitch("--enable-vpc-accessible-services")] bool EnableVpcAccessibleServices,
[property: CommandSwitch("--vpc-allowed-services")] string[] VpcAllowedServices,
[property: CommandSwitch("--perimeter-title")] string PerimeterTitle,
[property: CommandSwitch("--perimeter-type")] string PerimeterType,
[property: CommandSwitch("--perimeter-access-levels")] string[] PerimeterAccessLevels,
[property: CommandSwitch("--perimeter-description")] string PerimeterDescription,
[property: CommandSwitch("--perimeter-egress-policies")] string PerimeterEgressPolicies,
[property: CommandSwitch("--perimeter-ingress-policies")] string PerimeterIngressPolicies,
[property: CommandSwitch("--perimeter-resources")] string[] PerimeterResources,
[property: CommandSwitch("--perimeter-restricted-services")] string[] PerimeterRestrictedServices,
[property: BooleanCommandSwitch("--perimeter-enable-vpc-accessible-services")] bool PerimeterEnableVpcAccessibleServices,
[property: CommandSwitch("--perimeter-vpc-allowed-services")] string[] PerimeterVpcAllowedServices
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}