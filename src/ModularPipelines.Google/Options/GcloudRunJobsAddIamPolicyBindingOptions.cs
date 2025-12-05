using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "jobs", "add-iam-policy-binding")]
public record GcloudRunJobsAddIamPolicyBindingOptions(
[property: CliArgument] string Job,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}