using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devops", "service-endpoint", "create")]
public record AzDevopsServiceEndpointCreateOptions(
[property: CliOption("--service-endpoint-configuration")] string ServiceEndpointConfiguration
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--encoding")]
    public string? Encoding { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }
}