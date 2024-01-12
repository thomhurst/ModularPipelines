using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-config", "os-policy-assignments", "list")]
public record GcloudComputeOsConfigOsPolicyAssignmentsListOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}