using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "jobs", "executions", "cancel")]
public record GcloudRunJobsExecutionsCancelOptions(
[property: CliArgument] string Execution
) : GcloudOptions
{
    [CliOption("--[no-]async")]
    public string[]? NoAsync { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}