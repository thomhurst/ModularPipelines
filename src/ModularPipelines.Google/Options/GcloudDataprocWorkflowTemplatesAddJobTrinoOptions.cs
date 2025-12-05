using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "workflow-templates", "add-job", "trino")]
public record GcloudDataprocWorkflowTemplatesAddJobTrinoOptions(
[property: CliOption("--step-id")] string StepId,
[property: CliOption("--execute")] string Execute,
[property: CliOption("--file")] string File,
[property: CliOption("--workflow-template")] string WorkflowTemplate,
[property: CliOption("--region")] string Region
) : GcloudOptions
{
    [CliOption("--client-tags")]
    public string[]? ClientTags { get; set; }

    [CliFlag("--continue-on-failure")]
    public bool? ContinueOnFailure { get; set; }

    [CliOption("--driver-log-levels")]
    public string[]? DriverLogLevels { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--properties")]
    public string[]? Properties { get; set; }

    [CliOption("--properties-file")]
    public string? PropertiesFile { get; set; }

    [CliOption("--query-output-format")]
    public string? QueryOutputFormat { get; set; }

    [CliOption("--start-after")]
    public string[]? StartAfter { get; set; }
}