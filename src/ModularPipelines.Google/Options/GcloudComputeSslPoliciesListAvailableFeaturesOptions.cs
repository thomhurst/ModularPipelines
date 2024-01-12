using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "ssl-policies", "list-available-features")]
public record GcloudComputeSslPoliciesListAvailableFeaturesOptions : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}