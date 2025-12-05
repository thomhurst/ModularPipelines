using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privatenetworks", "acknowledge-order-receipt")]
public record AwsPrivatenetworksAcknowledgeOrderReceiptOptions(
[property: CliOption("--order-arn")] string OrderArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}