using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firehose", "update-destination")]
public record AwsFirehoseUpdateDestinationOptions(
[property: CliOption("--delivery-stream-name")] string DeliveryStreamName,
[property: CliOption("--current-delivery-stream-version-id")] string CurrentDeliveryStreamVersionId,
[property: CliOption("--destination-id")] string DestinationId
) : AwsOptions
{
    [CliOption("--s3-destination-update")]
    public string? S3DestinationUpdate { get; set; }

    [CliOption("--extended-s3-destination-update")]
    public string? ExtendedS3DestinationUpdate { get; set; }

    [CliOption("--redshift-destination-update")]
    public string? RedshiftDestinationUpdate { get; set; }

    [CliOption("--elasticsearch-destination-update")]
    public string? ElasticsearchDestinationUpdate { get; set; }

    [CliOption("--amazonopensearchservice-destination-update")]
    public string? AmazonopensearchserviceDestinationUpdate { get; set; }

    [CliOption("--splunk-destination-update")]
    public string? SplunkDestinationUpdate { get; set; }

    [CliOption("--http-endpoint-destination-update")]
    public string? HttpEndpointDestinationUpdate { get; set; }

    [CliOption("--amazon-open-search-serverless-destination-update")]
    public string? AmazonOpenSearchServerlessDestinationUpdate { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}