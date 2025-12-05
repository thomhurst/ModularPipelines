using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize-events", "put-actions")]
public record AwsPersonalizeEventsPutActionsOptions(
[property: CliOption("--dataset-arn")] string DatasetArn,
[property: CliOption("--actions")] string[] Actions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}