using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fsx", "cancel-data-repository-task")]
public record AwsFsxCancelDataRepositoryTaskOptions(
[property: CliOption("--task-id")] string TaskId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}