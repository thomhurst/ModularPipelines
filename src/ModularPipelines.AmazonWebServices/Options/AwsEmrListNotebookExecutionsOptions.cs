using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "list-notebook-executions")]
public record AwsEmrListNotebookExecutionsOptions : AwsOptions
{
    [CliOption("--editor-id")]
    public string? EditorId { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--from")]
    public long? From { get; set; }

    [CliOption("--to")]
    public long? To { get; set; }

    [CliOption("--execution-engine-id")]
    public string? ExecutionEngineId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}