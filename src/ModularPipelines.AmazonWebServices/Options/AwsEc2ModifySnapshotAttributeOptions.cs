using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-snapshot-attribute")]
public record AwsEc2ModifySnapshotAttributeOptions(
[property: CommandSwitch("--snapshot-id")] string SnapshotId
) : AwsOptions
{
    [CommandSwitch("--attribute")]
    public string? Attribute { get; set; }

    [CommandSwitch("--create-volume-permission")]
    public string? CreateVolumePermission { get; set; }

    [CommandSwitch("--group-names")]
    public string[]? GroupNames { get; set; }

    [CommandSwitch("--operation-type")]
    public string? OperationType { get; set; }

    [CommandSwitch("--user-ids")]
    public string[]? UserIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}