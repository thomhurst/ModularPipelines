using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "perimeters", "create")]
public record GcloudAccessContextManagerPerimetersCreateOptions(
[property: CliArgument] string Perimeter,
[property: CliArgument] string Policy,
[property: CliOption("--title")] string Title
) : GcloudOptions
{
    [CliOption("--access-levels")]
    public string[]? AccessLevels { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--egress-policies")]
    public string? EgressPolicies { get; set; }

    [CliOption("--ingress-policies")]
    public string? IngressPolicies { get; set; }

    [CliOption("--perimeter-type")]
    public string? PerimeterType { get; set; }

    [CliOption("--resources")]
    public string[]? Resources { get; set; }

    [CliOption("--restricted-services")]
    public string[]? RestrictedServices { get; set; }

    [CliFlag("--enable-vpc-accessible-services")]
    public bool? EnableVpcAccessibleServices { get; set; }

    [CliOption("--vpc-allowed-services")]
    public string[]? VpcAllowedServices { get; set; }
}