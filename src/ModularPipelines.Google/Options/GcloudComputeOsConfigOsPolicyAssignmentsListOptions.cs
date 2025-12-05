using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "os-policy-assignments", "list")]
public record GcloudComputeOsConfigOsPolicyAssignmentsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}