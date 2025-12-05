using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "ssl-policies", "list-available-features")]
public record GcloudComputeSslPoliciesListAvailableFeaturesOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}