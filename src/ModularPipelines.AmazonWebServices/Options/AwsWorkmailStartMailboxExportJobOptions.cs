using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "start-mailbox-export-job")]
public record AwsWorkmailStartMailboxExportJobOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--entity-id")] string EntityId,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--kms-key-arn")] string KmsKeyArn,
[property: CommandSwitch("--s3-bucket-name")] string S3BucketName,
[property: CommandSwitch("--s3-prefix")] string S3Prefix
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}