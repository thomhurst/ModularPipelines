using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firehose", "update-destination")]
public record AwsFirehoseUpdateDestinationOptions(
[property: CommandSwitch("--delivery-stream-name")] string DeliveryStreamName,
[property: CommandSwitch("--current-delivery-stream-version-id")] string CurrentDeliveryStreamVersionId,
[property: CommandSwitch("--destination-id")] string DestinationId
) : AwsOptions
{
    [CommandSwitch("--s3-destination-update")]
    public string? S3DestinationUpdate { get; set; }

    [CommandSwitch("--extended-s3-destination-update")]
    public string? ExtendedS3DestinationUpdate { get; set; }

    [CommandSwitch("--redshift-destination-update")]
    public string? RedshiftDestinationUpdate { get; set; }

    [CommandSwitch("--elasticsearch-destination-update")]
    public string? ElasticsearchDestinationUpdate { get; set; }

    [CommandSwitch("--amazonopensearchservice-destination-update")]
    public string? AmazonopensearchserviceDestinationUpdate { get; set; }

    [CommandSwitch("--splunk-destination-update")]
    public string? SplunkDestinationUpdate { get; set; }

    [CommandSwitch("--http-endpoint-destination-update")]
    public string? HttpEndpointDestinationUpdate { get; set; }

    [CommandSwitch("--amazon-open-search-serverless-destination-update")]
    public string? AmazonOpenSearchServerlessDestinationUpdate { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}