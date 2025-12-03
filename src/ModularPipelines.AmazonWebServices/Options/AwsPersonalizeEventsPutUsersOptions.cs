using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize-events", "put-users")]
public record AwsPersonalizeEventsPutUsersOptions(
[property: CliOption("--dataset-arn")] string DatasetArn,
[property: CliOption("--users")] string[] Users
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}