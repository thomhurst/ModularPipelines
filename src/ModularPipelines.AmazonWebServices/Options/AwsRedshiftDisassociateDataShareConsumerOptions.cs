using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "disassociate-data-share-consumer")]
public record AwsRedshiftDisassociateDataShareConsumerOptions(
[property: CliOption("--data-share-arn")] string DataShareArn
) : AwsOptions
{
    [CliOption("--consumer-arn")]
    public string? ConsumerArn { get; set; }

    [CliOption("--consumer-region")]
    public string? ConsumerRegion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}