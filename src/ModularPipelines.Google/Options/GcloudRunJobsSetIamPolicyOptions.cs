using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run", "jobs", "set-iam-policy")]
public record GcloudRunJobsSetIamPolicyOptions(
[property: PositionalArgument] string Job,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}