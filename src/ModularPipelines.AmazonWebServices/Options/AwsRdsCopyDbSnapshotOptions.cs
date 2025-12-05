using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "copy-db-snapshot")]
public record AwsRdsCopyDbSnapshotOptions(
[property: CliOption("--source-db-snapshot-identifier")] string SourceDbSnapshotIdentifier,
[property: CliOption("--target-db-snapshot-identifier")] string TargetDbSnapshotIdentifier
) : AwsOptions
{
    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--pre-signed-url")]
    public string? PreSignedUrl { get; set; }

    [CliOption("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CliOption("--target-custom-availability-zone")]
    public string? TargetCustomAvailabilityZone { get; set; }

    [CliOption("--source-region")]
    public string? SourceRegion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}