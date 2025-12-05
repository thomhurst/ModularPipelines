using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "signal-workflow-execution")]
public record AwsSwfSignalWorkflowExecutionOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--workflow-id")] string WorkflowId,
[property: CliOption("--signal-name")] string SignalName
) : AwsOptions
{
    [CliOption("--run-id")]
    public string? RunId { get; set; }

    [CliOption("--input")]
    public string? Input { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}