using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "resource-policies", "create", "group-placement")]
public record GcloudComputeResourcePoliciesCreateGroupPlacementOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--availability-domain-count")]
    public string? AvailabilityDomainCount { get; set; }

    [CommandSwitch("--collocation")]
    public string? Collocation { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--vm-count")]
    public string? VmCount { get; set; }
}