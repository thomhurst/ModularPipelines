using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("entityresolution", "update-matching-workflow")]
public record AwsEntityresolutionUpdateMatchingWorkflowOptions(
[property: CliOption("--input-source-config")] string[] InputSourceConfig,
[property: CliOption("--output-source-config")] string[] OutputSourceConfig,
[property: CliOption("--resolution-techniques")] string ResolutionTechniques,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--workflow-name")] string WorkflowName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--incremental-run-config")]
    public string? IncrementalRunConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}