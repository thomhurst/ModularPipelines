using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("entityresolution", "create-matching-workflow")]
public record AwsEntityresolutionCreateMatchingWorkflowOptions(
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

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}