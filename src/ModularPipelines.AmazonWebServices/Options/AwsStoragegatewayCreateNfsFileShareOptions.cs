using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "create-nfs-file-share")]
public record AwsStoragegatewayCreateNfsFileShareOptions(
[property: CliOption("--client-token")] string ClientToken,
[property: CliOption("--gateway-arn")] string GatewayArn,
[property: CliOption("--role")] string Role,
[property: CliOption("--location-arn")] string LocationArn
) : AwsOptions
{
    [CliOption("--nfs-file-share-defaults")]
    public string? NfsFileShareDefaults { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--default-storage-class")]
    public string? DefaultStorageClass { get; set; }

    [CliOption("--object-acl")]
    public string? ObjectAcl { get; set; }

    [CliOption("--client-list")]
    public string[]? ClientList { get; set; }

    [CliOption("--squash")]
    public string? Squash { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--file-share-name")]
    public string? FileShareName { get; set; }

    [CliOption("--cache-attributes")]
    public string? CacheAttributes { get; set; }

    [CliOption("--notification-policy")]
    public string? NotificationPolicy { get; set; }

    [CliOption("--vpc-endpoint-dns-name")]
    public string? VpcEndpointDnsName { get; set; }

    [CliOption("--bucket-region")]
    public string? BucketRegion { get; set; }

    [CliOption("--audit-destination-arn")]
    public string? AuditDestinationArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}