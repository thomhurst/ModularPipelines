using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "os-policy-assignments", "create")]
public record GcloudComputeOsConfigOsPolicyAssignmentsCreateOptions(
[property: CliArgument] string OsPolicyAssignment,
[property: CliArgument] string Location,
[property: CliOption("--file")] string File
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}