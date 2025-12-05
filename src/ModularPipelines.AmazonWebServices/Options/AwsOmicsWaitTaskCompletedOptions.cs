using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "wait", "task-completed")]
public record AwsOmicsWaitTaskCompletedOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--task-id")] string TaskId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}