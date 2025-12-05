using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "pipeline", "update-schedule")]
public record AzMlPipelineUpdateScheduleOptions(
[property: CliOption("--schedule-id")] string ScheduleId
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--schedule-yaml")]
    public string? ScheduleYaml { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}