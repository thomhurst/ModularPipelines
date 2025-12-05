using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "os-policy-assignments", "delete")]
public record GcloudComputeOsConfigOsPolicyAssignmentsDeleteOptions(
[property: CliArgument] string OsPolicyAssignment,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}