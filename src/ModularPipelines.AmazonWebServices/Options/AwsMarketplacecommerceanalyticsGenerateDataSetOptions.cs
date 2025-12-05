using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("marketplacecommerceanalytics", "generate-data-set")]
public record AwsMarketplacecommerceanalyticsGenerateDataSetOptions(
[property: CliOption("--data-set-type")] string DataSetType,
[property: CliOption("--data-set-publication-date")] long DataSetPublicationDate,
[property: CliOption("--role-name-arn")] string RoleNameArn,
[property: CliOption("--destination-s3-bucket-name")] string DestinationS3BucketName,
[property: CliOption("--sns-topic-arn")] string SnsTopicArn
) : AwsOptions
{
    [CliOption("--destination-s3-prefix")]
    public string? DestinationS3Prefix { get; set; }

    [CliOption("--customer-defined-values")]
    public IEnumerable<KeyValue>? CustomerDefinedValues { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}