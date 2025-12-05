using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "custom-jobs", "list")]
public record GcloudAiCustomJobsListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}