using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "resource-policies", "create", "group-placement")]
public record GcloudComputeResourcePoliciesCreateGroupPlacementOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--availability-domain-count")]
    public string? AvailabilityDomainCount { get; set; }

    [CliOption("--collocation")]
    public string? Collocation { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--vm-count")]
    public string? VmCount { get; set; }
}