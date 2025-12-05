using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "describe-task-sets")]
public record AwsEcsDescribeTaskSetsOptions(
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--service")] string Service
) : AwsOptions
{
    [CliOption("--task-sets")]
    public string[]? TaskSets { get; set; }

    [CliOption("--include")]
    public string[]? Include { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}