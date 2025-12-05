using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "workflow-templates", "remove-job")]
public record GcloudDataprocWorkflowTemplatesRemoveJobOptions(
[property: CliArgument] string Template,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--step-id")]
    public string? StepId { get; set; }
}