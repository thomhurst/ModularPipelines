using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "update-task-set")]
public record AwsEcsUpdateTaskSetOptions(
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--service")] string Service,
[property: CliOption("--task-set")] string TaskSet,
[property: CliOption("--scale")] string Scale
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}