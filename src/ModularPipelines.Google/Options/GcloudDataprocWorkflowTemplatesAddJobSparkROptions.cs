using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "workflow-templates", "add-job", "spark-r")]
public record GcloudDataprocWorkflowTemplatesAddJobSparkROptions(
[property: CliArgument] string RFile,
[property: CliArgument] string JobArgs,
[property: CliOption("--step-id")] string StepId,
[property: CliOption("--workflow-template")] string WorkflowTemplate,
[property: CliOption("--region")] string Region
) : GcloudOptions
{
    [CliOption("--archives")]
    public string[]? Archives { get; set; }

    [CliOption("--driver-log-levels")]
    public string[]? DriverLogLevels { get; set; }

    [CliOption("--files")]
    public string[]? Files { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--properties")]
    public string[]? Properties { get; set; }

    [CliOption("--properties-file")]
    public string? PropertiesFile { get; set; }

    [CliOption("--start-after")]
    public string[]? StartAfter { get; set; }
}