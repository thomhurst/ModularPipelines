using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "os-policy-assignment-reports", "list")]
public record GcloudComputeOsConfigOsPolicyAssignmentReportsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--assignment-id")]
    public string? AssignmentId { get; set; }

    [CliOption("--instance")]
    public string? Instance { get; set; }
}