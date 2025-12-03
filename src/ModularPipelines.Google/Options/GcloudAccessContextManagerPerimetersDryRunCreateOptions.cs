using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "perimeters", "dry-run", "create")]
public record GcloudAccessContextManagerPerimetersDryRunCreateOptions(
[property: CliArgument] string Perimeter,
[property: CliArgument] string Policy,
[property: CliOption("--access-levels")] string[] AccessLevels,
[property: CliOption("--egress-policies")] string EgressPolicies,
[property: CliOption("--ingress-policies")] string IngressPolicies,
[property: CliOption("--resources")] string[] Resources,
[property: CliOption("--restricted-services")] string[] RestrictedServices,
[property: CliFlag("--enable-vpc-accessible-services")] bool EnableVpcAccessibleServices,
[property: CliOption("--vpc-allowed-services")] string[] VpcAllowedServices,
[property: CliOption("--perimeter-title")] string PerimeterTitle,
[property: CliOption("--perimeter-type")] string PerimeterType,
[property: CliOption("--perimeter-access-levels")] string[] PerimeterAccessLevels,
[property: CliOption("--perimeter-description")] string PerimeterDescription,
[property: CliOption("--perimeter-egress-policies")] string PerimeterEgressPolicies,
[property: CliOption("--perimeter-ingress-policies")] string PerimeterIngressPolicies,
[property: CliOption("--perimeter-resources")] string[] PerimeterResources,
[property: CliOption("--perimeter-restricted-services")] string[] PerimeterRestrictedServices,
[property: CliFlag("--perimeter-enable-vpc-accessible-services")] bool PerimeterEnableVpcAccessibleServices,
[property: CliOption("--perimeter-vpc-allowed-services")] string[] PerimeterVpcAllowedServices
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}