using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "workflow-templates", "add-job", "pig")]
public record GcloudDataprocWorkflowTemplatesAddJobPigOptions(
[property: CommandSwitch("--step-id")] string StepId,
[property: CommandSwitch("--execute")] string Execute,
[property: CommandSwitch("--file")] string File,
[property: CommandSwitch("--workflow-template")] string WorkflowTemplate,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--continue-on-failure")]
    public bool? ContinueOnFailure { get; set; }

    [CommandSwitch("--driver-log-levels")]
    public string[]? DriverLogLevels { get; set; }

    [CommandSwitch("--jars")]
    public string[]? Jars { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--params")]
    public string[]? Params { get; set; }

    [CommandSwitch("--properties")]
    public string[]? Properties { get; set; }

    [CommandSwitch("--properties-file")]
    public string? PropertiesFile { get; set; }

    [CommandSwitch("--start-after")]
    public string[]? StartAfter { get; set; }
}