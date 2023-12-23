using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "copy-db-snapshot")]
public record AwsRdsCopyDbSnapshotOptions(
[property: CommandSwitch("--source-db-snapshot-identifier")] string SourceDbSnapshotIdentifier,
[property: CommandSwitch("--target-db-snapshot-identifier")] string TargetDbSnapshotIdentifier
) : AwsOptions
{
    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--pre-signed-url")]
    public string? PreSignedUrl { get; set; }

    [CommandSwitch("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CommandSwitch("--target-custom-availability-zone")]
    public string? TargetCustomAvailabilityZone { get; set; }

    [CommandSwitch("--source-region")]
    public string? SourceRegion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}