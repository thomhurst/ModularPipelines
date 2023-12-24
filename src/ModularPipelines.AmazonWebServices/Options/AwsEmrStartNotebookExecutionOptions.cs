using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "start-notebook-execution")]
public record AwsEmrStartNotebookExecutionOptions(
[property: CommandSwitch("--execution-engine")] string ExecutionEngine,
[property: CommandSwitch("--service-role")] string ServiceRole
) : AwsOptions
{
    [CommandSwitch("--editor-id")]
    public string? EditorId { get; set; }

    [CommandSwitch("--relative-path")]
    public string? RelativePath { get; set; }

    [CommandSwitch("--notebook-execution-name")]
    public string? NotebookExecutionName { get; set; }

    [CommandSwitch("--notebook-params")]
    public string? NotebookParams { get; set; }

    [CommandSwitch("--notebook-instance-security-group-id")]
    public string? NotebookInstanceSecurityGroupId { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--notebook-s3-location")]
    public string? NotebookS3Location { get; set; }

    [CommandSwitch("--output-notebook-s3-location")]
    public string? OutputNotebookS3Location { get; set; }

    [CommandSwitch("--output-notebook-format")]
    public string? OutputNotebookFormat { get; set; }

    [CommandSwitch("--environment-variables")]
    public IEnumerable<KeyValue>? AwsEmrSEnvironmentVariables { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}