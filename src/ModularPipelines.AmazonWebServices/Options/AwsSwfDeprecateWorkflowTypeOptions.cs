using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "deprecate-workflow-type")]
public record AwsSwfDeprecateWorkflowTypeOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--workflow-type")] string WorkflowType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}