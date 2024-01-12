using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run", "jobs", "add-iam-policy-binding")]
public record GcloudRunJobsAddIamPolicyBindingOptions(
[property: PositionalArgument] string Job,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}