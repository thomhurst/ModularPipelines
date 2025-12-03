using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize-events", "put-items")]
public record AwsPersonalizeEventsPutItemsOptions(
[property: CliOption("--dataset-arn")] string DatasetArn,
[property: CliOption("--items")] string[] Items
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}