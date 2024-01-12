using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("org-policies", "set-policy")]
public record GcloudOrgPoliciesSetPolicyOptions(
[property: PositionalArgument] string PolicyFile
) : GcloudOptions
{
    [CommandSwitch("--update-mask")]
    public string? UpdateMask { get; set; }
}