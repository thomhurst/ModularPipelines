using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("marketplacecommerceanalytics", "generate-data-set")]
public record AwsMarketplacecommerceanalyticsGenerateDataSetOptions(
[property: CommandSwitch("--data-set-type")] string DataSetType,
[property: CommandSwitch("--data-set-publication-date")] long DataSetPublicationDate,
[property: CommandSwitch("--role-name-arn")] string RoleNameArn,
[property: CommandSwitch("--destination-s3-bucket-name")] string DestinationS3BucketName,
[property: CommandSwitch("--sns-topic-arn")] string SnsTopicArn
) : AwsOptions
{
    [CommandSwitch("--destination-s3-prefix")]
    public string? DestinationS3Prefix { get; set; }

    [CommandSwitch("--customer-defined-values")]
    public IEnumerable<KeyValue>? CustomerDefinedValues { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}