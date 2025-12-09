using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "delete-task-definitions")]
public record AwsEcsDeleteTaskDefinitionsOptions(
[property: CliOption("--task-definitions")] string[] TaskDefinitions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}