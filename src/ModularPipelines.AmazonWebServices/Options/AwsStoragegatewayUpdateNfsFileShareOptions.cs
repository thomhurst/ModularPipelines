using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "update-nfs-file-share")]
public record AwsStoragegatewayUpdateNfsFileShareOptions(
[property: CommandSwitch("--file-share-arn")] string FileShareArn
) : AwsOptions
{
    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--nfs-file-share-defaults")]
    public string? NfsFileShareDefaults { get; set; }

    [CommandSwitch("--default-storage-class")]
    public string? DefaultStorageClass { get; set; }

    [CommandSwitch("--object-acl")]
    public string? ObjectAcl { get; set; }

    [CommandSwitch("--client-list")]
    public string[]? ClientList { get; set; }

    [CommandSwitch("--squash")]
    public string? Squash { get; set; }

    [CommandSwitch("--file-share-name")]
    public string? FileShareName { get; set; }

    [CommandSwitch("--cache-attributes")]
    public string? CacheAttributes { get; set; }

    [CommandSwitch("--notification-policy")]
    public string? NotificationPolicy { get; set; }

    [CommandSwitch("--audit-destination-arn")]
    public string? AuditDestinationArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}