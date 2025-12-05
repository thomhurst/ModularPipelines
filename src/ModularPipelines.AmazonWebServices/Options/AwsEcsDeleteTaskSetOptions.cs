using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "delete-task-set")]
public record AwsEcsDeleteTaskSetOptions(
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--service")] string Service,
[property: CliOption("--task-set")] string TaskSet
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}