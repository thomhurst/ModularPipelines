using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("snowball", "create-return-shipping-label")]
public record AwsSnowballCreateReturnShippingLabelOptions(
[property: CliOption("--job-id")] string JobId
) : AwsOptions
{
    [CliOption("--shipping-option")]
    public string? ShippingOption { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}