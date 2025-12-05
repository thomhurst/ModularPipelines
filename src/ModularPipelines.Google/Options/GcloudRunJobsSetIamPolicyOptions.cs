using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "jobs", "set-iam-policy")]
public record GcloudRunJobsSetIamPolicyOptions(
[property: CliArgument] string Job,
[property: CliArgument] string PolicyFile
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}