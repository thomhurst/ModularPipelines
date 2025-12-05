using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "describe-event-tracker")]
public record AwsPersonalizeDescribeEventTrackerOptions(
[property: CliOption("--event-tracker-arn")] string EventTrackerArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}