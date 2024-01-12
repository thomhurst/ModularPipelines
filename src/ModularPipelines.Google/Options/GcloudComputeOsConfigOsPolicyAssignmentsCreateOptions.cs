using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-config", "os-policy-assignments", "create")]
public record GcloudComputeOsConfigOsPolicyAssignmentsCreateOptions(
[property: PositionalArgument] string OsPolicyAssignment,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--file")] string File
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}