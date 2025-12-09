using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "describe-delivery-channel-status")]
public record AwsConfigserviceDescribeDeliveryChannelStatusOptions : AwsOptions
{
    [CliOption("--delivery-channel-names")]
    public string[]? DeliveryChannelNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}