using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run", "jobs", "get-iam-policy")]
public record GcloudRunJobsGetIamPolicyOptions(
[property: PositionalArgument] string Job
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}