using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-snapshot-attribute")]
public record AwsEc2ModifySnapshotAttributeOptions(
[property: CliOption("--snapshot-id")] string SnapshotId
) : AwsOptions
{
    [CliOption("--attribute")]
    public string? Attribute { get; set; }

    [CliOption("--create-volume-permission")]
    public string? CreateVolumePermission { get; set; }

    [CliOption("--group-names")]
    public string[]? GroupNames { get; set; }

    [CliOption("--operation-type")]
    public string? OperationType { get; set; }

    [CliOption("--user-ids")]
    public string[]? UserIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}