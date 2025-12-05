using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "start-notebook-execution")]
public record AwsEmrStartNotebookExecutionOptions(
[property: CliOption("--execution-engine")] string ExecutionEngine,
[property: CliOption("--service-role")] string ServiceRole
) : AwsOptions
{
    [CliOption("--editor-id")]
    public string? EditorId { get; set; }

    [CliOption("--relative-path")]
    public string? RelativePath { get; set; }

    [CliOption("--notebook-execution-name")]
    public string? NotebookExecutionName { get; set; }

    [CliOption("--notebook-params")]
    public string? NotebookParams { get; set; }

    [CliOption("--notebook-instance-security-group-id")]
    public string? NotebookInstanceSecurityGroupId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--notebook-s3-location")]
    public string? NotebookS3Location { get; set; }

    [CliOption("--output-notebook-s3-location")]
    public string? OutputNotebookS3Location { get; set; }

    [CliOption("--output-notebook-format")]
    public string? OutputNotebookFormat { get; set; }

    [CliOption("--environment-variables")]
    public IEnumerable<KeyValue>? AwsEmrSEnvironmentVariables { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}