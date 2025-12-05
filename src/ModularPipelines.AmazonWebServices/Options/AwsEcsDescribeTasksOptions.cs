using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "describe-tasks")]
public record AwsEcsDescribeTasksOptions(
[property: CliOption("--tasks")] string[] Tasks
) : AwsOptions
{
    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--include")]
    public string[]? Include { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}