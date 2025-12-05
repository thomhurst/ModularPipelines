using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "workflow-templates", "add-job", "pig")]
public record GcloudDataprocWorkflowTemplatesAddJobPigOptions(
[property: CliOption("--step-id")] string StepId,
[property: CliOption("--execute")] string Execute,
[property: CliOption("--file")] string File,
[property: CliOption("--workflow-template")] string WorkflowTemplate,
[property: CliOption("--region")] string Region
) : GcloudOptions
{
    [CliFlag("--continue-on-failure")]
    public bool? ContinueOnFailure { get; set; }

    [CliOption("--driver-log-levels")]
    public string[]? DriverLogLevels { get; set; }

    [CliOption("--jars")]
    public string[]? Jars { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--params")]
    public string[]? Params { get; set; }

    [CliOption("--properties")]
    public string[]? Properties { get; set; }

    [CliOption("--properties-file")]
    public string? PropertiesFile { get; set; }

    [CliOption("--start-after")]
    public string[]? StartAfter { get; set; }
}