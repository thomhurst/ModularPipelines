using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf", "create-web-acl-migration-stack")]
public record AwsWafCreateWebAclMigrationStackOptions(
[property: CommandSwitch("--web-acl-id")] string WebAclId,
[property: CommandSwitch("--s3-bucket-name")] string S3BucketName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}