using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "start-mailbox-export-job")]
public record AwsWorkmailStartMailboxExportJobOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--entity-id")] string EntityId,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--kms-key-arn")] string KmsKeyArn,
[property: CliOption("--s3-bucket-name")] string S3BucketName,
[property: CliOption("--s3-prefix")] string S3Prefix
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}