using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf", "create-web-acl-migration-stack")]
public record AwsWafCreateWebAclMigrationStackOptions(
[property: CliOption("--web-acl-id")] string WebAclId,
[property: CliOption("--s3-bucket-name")] string S3BucketName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}