using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "jobs", "get-iam-policy")]
public record GcloudRunJobsGetIamPolicyOptions(
[property: CliArgument] string Job
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}