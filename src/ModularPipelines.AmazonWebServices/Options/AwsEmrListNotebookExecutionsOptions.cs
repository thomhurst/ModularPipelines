using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "list-notebook-executions")]
public record AwsEmrListNotebookExecutionsOptions : AwsOptions
{
    [CommandSwitch("--editor-id")]
    public string? EditorId { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--from")]
    public long? From { get; set; }

    [CommandSwitch("--to")]
    public long? To { get; set; }

    [CommandSwitch("--execution-engine-id")]
    public string? ExecutionEngineId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}