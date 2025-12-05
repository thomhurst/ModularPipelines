using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firehose", "create-delivery-stream")]
public record AwsFirehoseCreateDeliveryStreamOptions(
[property: CliOption("--delivery-stream-name")] string DeliveryStreamName
) : AwsOptions
{
    [CliOption("--delivery-stream-type")]
    public string? DeliveryStreamType { get; set; }

    [CliOption("--kinesis-stream-source-configuration")]
    public string? KinesisStreamSourceConfiguration { get; set; }

    [CliOption("--delivery-stream-encryption-configuration-input")]
    public string? DeliveryStreamEncryptionConfigurationInput { get; set; }

    [CliOption("--s3-destination-configuration")]
    public string? S3DestinationConfiguration { get; set; }

    [CliOption("--extended-s3-destination-configuration")]
    public string? ExtendedS3DestinationConfiguration { get; set; }

    [CliOption("--redshift-destination-configuration")]
    public string? RedshiftDestinationConfiguration { get; set; }

    [CliOption("--elasticsearch-destination-configuration")]
    public string? ElasticsearchDestinationConfiguration { get; set; }

    [CliOption("--amazonopensearchservice-destination-configuration")]
    public string? AmazonopensearchserviceDestinationConfiguration { get; set; }

    [CliOption("--splunk-destination-configuration")]
    public string? SplunkDestinationConfiguration { get; set; }

    [CliOption("--http-endpoint-destination-configuration")]
    public string? HttpEndpointDestinationConfiguration { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--amazon-open-search-serverless-destination-configuration")]
    public string? AmazonOpenSearchServerlessDestinationConfiguration { get; set; }

    [CliOption("--msk-source-configuration")]
    public string? MskSourceConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}