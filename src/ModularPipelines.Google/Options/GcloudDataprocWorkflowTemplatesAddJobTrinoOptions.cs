using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "workflow-templates", "add-job", "trino")]
public record GcloudDataprocWorkflowTemplatesAddJobTrinoOptions(
[property: CommandSwitch("--step-id")] string StepId,
[property: CommandSwitch("--execute")] string Execute,
[property: CommandSwitch("--file")] string File,
[property: CommandSwitch("--workflow-template")] string WorkflowTemplate,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions
{
    [CommandSwitch("--client-tags")]
    public string[]? ClientTags { get; set; }

    [BooleanCommandSwitch("--continue-on-failure")]
    public bool? ContinueOnFailure { get; set; }

    [CommandSwitch("--driver-log-levels")]
    public string[]? DriverLogLevels { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--properties")]
    public string[]? Properties { get; set; }

    [CommandSwitch("--properties-file")]
    public string? PropertiesFile { get; set; }

    [CommandSwitch("--query-output-format")]
    public string? QueryOutputFormat { get; set; }

    [CommandSwitch("--start-after")]
    public string[]? StartAfter { get; set; }
}