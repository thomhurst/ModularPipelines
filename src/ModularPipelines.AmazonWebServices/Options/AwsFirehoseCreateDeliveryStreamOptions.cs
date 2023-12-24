using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firehose", "create-delivery-stream")]
public record AwsFirehoseCreateDeliveryStreamOptions(
[property: CommandSwitch("--delivery-stream-name")] string DeliveryStreamName
) : AwsOptions
{
    [CommandSwitch("--delivery-stream-type")]
    public string? DeliveryStreamType { get; set; }

    [CommandSwitch("--kinesis-stream-source-configuration")]
    public string? KinesisStreamSourceConfiguration { get; set; }

    [CommandSwitch("--delivery-stream-encryption-configuration-input")]
    public string? DeliveryStreamEncryptionConfigurationInput { get; set; }

    [CommandSwitch("--s3-destination-configuration")]
    public string? S3DestinationConfiguration { get; set; }

    [CommandSwitch("--extended-s3-destination-configuration")]
    public string? ExtendedS3DestinationConfiguration { get; set; }

    [CommandSwitch("--redshift-destination-configuration")]
    public string? RedshiftDestinationConfiguration { get; set; }

    [CommandSwitch("--elasticsearch-destination-configuration")]
    public string? ElasticsearchDestinationConfiguration { get; set; }

    [CommandSwitch("--amazonopensearchservice-destination-configuration")]
    public string? AmazonopensearchserviceDestinationConfiguration { get; set; }

    [CommandSwitch("--splunk-destination-configuration")]
    public string? SplunkDestinationConfiguration { get; set; }

    [CommandSwitch("--http-endpoint-destination-configuration")]
    public string? HttpEndpointDestinationConfiguration { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--amazon-open-search-serverless-destination-configuration")]
    public string? AmazonOpenSearchServerlessDestinationConfiguration { get; set; }

    [CommandSwitch("--msk-source-configuration")]
    public string? MskSourceConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}