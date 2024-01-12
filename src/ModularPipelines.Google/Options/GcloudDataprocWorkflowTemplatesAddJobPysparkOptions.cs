using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "workflow-templates", "add-job", "pyspark")]
public record GcloudDataprocWorkflowTemplatesAddJobPysparkOptions(
[property: PositionalArgument] string PyFile,
[property: PositionalArgument] string JobArgs,
[property: CommandSwitch("--step-id")] string StepId,
[property: CommandSwitch("--workflow-template")] string WorkflowTemplate,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions
{
    [CommandSwitch("--archives")]
    public string[]? Archives { get; set; }

    [CommandSwitch("--driver-log-levels")]
    public string[]? DriverLogLevels { get; set; }

    [CommandSwitch("--files")]
    public string[]? Files { get; set; }

    [CommandSwitch("--jars")]
    public string[]? Jars { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--properties")]
    public string[]? Properties { get; set; }

    [CommandSwitch("--properties-file")]
    public string? PropertiesFile { get; set; }

    [CommandSwitch("--py-files")]
    public string[]? PyFiles { get; set; }

    [CommandSwitch("--start-after")]
    public string[]? StartAfter { get; set; }
}