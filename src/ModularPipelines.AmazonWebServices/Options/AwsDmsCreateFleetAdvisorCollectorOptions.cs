using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "create-fleet-advisor-collector")]
public record AwsDmsCreateFleetAdvisorCollectorOptions(
[property: CliOption("--collector-name")] string CollectorName,
[property: CliOption("--service-access-role-arn")] string ServiceAccessRoleArn,
[property: CliOption("--s3-bucket-name")] string S3BucketName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}