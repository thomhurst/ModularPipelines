using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "associate-tracker-consumer")]
public record AwsLocationAssociateTrackerConsumerOptions(
[property: CliOption("--consumer-arn")] string ConsumerArn,
[property: CliOption("--tracker-name")] string TrackerName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}