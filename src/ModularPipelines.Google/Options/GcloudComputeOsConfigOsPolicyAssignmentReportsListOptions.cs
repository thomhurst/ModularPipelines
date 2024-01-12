using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-config", "os-policy-assignment-reports", "list")]
public record GcloudComputeOsConfigOsPolicyAssignmentReportsListOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--assignment-id")]
    public string? AssignmentId { get; set; }

    [CommandSwitch("--instance")]
    public string? Instance { get; set; }
}