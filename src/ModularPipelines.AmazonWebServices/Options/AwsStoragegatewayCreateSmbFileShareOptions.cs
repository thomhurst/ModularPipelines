using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "create-smb-file-share")]
public record AwsStoragegatewayCreateSmbFileShareOptions(
[property: CommandSwitch("--client-token")] string ClientToken,
[property: CommandSwitch("--gateway-arn")] string GatewayArn,
[property: CommandSwitch("--role")] string Role,
[property: CommandSwitch("--location-arn")] string LocationArn
) : AwsOptions
{
    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--default-storage-class")]
    public string? DefaultStorageClass { get; set; }

    [CommandSwitch("--object-acl")]
    public string? ObjectAcl { get; set; }

    [CommandSwitch("--admin-user-list")]
    public string[]? AdminUserList { get; set; }

    [CommandSwitch("--valid-user-list")]
    public string[]? ValidUserList { get; set; }

    [CommandSwitch("--invalid-user-list")]
    public string[]? InvalidUserList { get; set; }

    [CommandSwitch("--audit-destination-arn")]
    public string? AuditDestinationArn { get; set; }

    [CommandSwitch("--authentication")]
    public string? Authentication { get; set; }

    [CommandSwitch("--case-sensitivity")]
    public string? CaseSensitivity { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--file-share-name")]
    public string? FileShareName { get; set; }

    [CommandSwitch("--cache-attributes")]
    public string? CacheAttributes { get; set; }

    [CommandSwitch("--notification-policy")]
    public string? NotificationPolicy { get; set; }

    [CommandSwitch("--vpc-endpoint-dns-name")]
    public string? VpcEndpointDnsName { get; set; }

    [CommandSwitch("--bucket-region")]
    public string? BucketRegion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}