using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "jobs", "delete")]
public record GcloudRunJobsDeleteOptions(
[property: CliArgument] string Job
) : GcloudOptions
{
    [CliOption("--[no-]async")]
    public string[]? NoAsync { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}