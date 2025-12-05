using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "restore-from-snapshot")]
public record AwsRedshiftServerlessRestoreFromSnapshotOptions(
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--workgroup-name")] string WorkgroupName
) : AwsOptions
{
    [CliOption("--admin-password-secret-kms-key-id")]
    public string? AdminPasswordSecretKmsKeyId { get; set; }

    [CliOption("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CliOption("--snapshot-arn")]
    public string? SnapshotArn { get; set; }

    [CliOption("--snapshot-name")]
    public string? SnapshotName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}