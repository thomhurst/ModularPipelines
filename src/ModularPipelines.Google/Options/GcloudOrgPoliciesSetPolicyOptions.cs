using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("org-policies", "set-policy")]
public record GcloudOrgPoliciesSetPolicyOptions(
[property: CliArgument] string PolicyFile
) : GcloudOptions
{
    [CliOption("--update-mask")]
    public string? UpdateMask { get; set; }
}