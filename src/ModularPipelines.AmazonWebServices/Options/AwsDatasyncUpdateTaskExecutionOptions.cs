using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "update-task-execution")]
public record AwsDatasyncUpdateTaskExecutionOptions(
[property: CliOption("--task-execution-arn")] string TaskExecutionArn,
[property: CliOption("--options")] string Options
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}