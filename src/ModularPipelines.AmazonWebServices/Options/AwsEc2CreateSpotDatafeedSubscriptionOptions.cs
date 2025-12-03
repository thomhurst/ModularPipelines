using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-spot-datafeed-subscription")]
public record AwsEc2CreateSpotDatafeedSubscriptionOptions(
[property: CliOption("--bucket")] string Bucket
) : AwsOptions
{
    [CliOption("--prefix")]
    public string? Prefix { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}