using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "perimeters", "create")]
public record GcloudAccessContextManagerPerimetersCreateOptions(
[property: PositionalArgument] string Perimeter,
[property: PositionalArgument] string Policy,
[property: CommandSwitch("--title")] string Title
) : GcloudOptions
{
    [CommandSwitch("--access-levels")]
    public string[]? AccessLevels { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--egress-policies")]
    public string? EgressPolicies { get; set; }

    [CommandSwitch("--ingress-policies")]
    public string? IngressPolicies { get; set; }

    [CommandSwitch("--perimeter-type")]
    public string? PerimeterType { get; set; }

    [CommandSwitch("--resources")]
    public string[]? Resources { get; set; }

    [CommandSwitch("--restricted-services")]
    public string[]? RestrictedServices { get; set; }

    [BooleanCommandSwitch("--enable-vpc-accessible-services")]
    public bool? EnableVpcAccessibleServices { get; set; }

    [CommandSwitch("--vpc-allowed-services")]
    public string[]? VpcAllowedServices { get; set; }
}