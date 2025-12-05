using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "update-smb-file-share")]
public record AwsStoragegatewayUpdateSmbFileShareOptions(
[property: CliOption("--file-share-arn")] string FileShareArn
) : AwsOptions
{
    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--default-storage-class")]
    public string? DefaultStorageClass { get; set; }

    [CliOption("--object-acl")]
    public string? ObjectAcl { get; set; }

    [CliOption("--admin-user-list")]
    public string[]? AdminUserList { get; set; }

    [CliOption("--valid-user-list")]
    public string[]? ValidUserList { get; set; }

    [CliOption("--invalid-user-list")]
    public string[]? InvalidUserList { get; set; }

    [CliOption("--audit-destination-arn")]
    public string? AuditDestinationArn { get; set; }

    [CliOption("--case-sensitivity")]
    public string? CaseSensitivity { get; set; }

    [CliOption("--file-share-name")]
    public string? FileShareName { get; set; }

    [CliOption("--cache-attributes")]
    public string? CacheAttributes { get; set; }

    [CliOption("--notification-policy")]
    public string? NotificationPolicy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}