using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "disassociate-instance-event-window")]
public record AwsEc2DisassociateInstanceEventWindowOptions(
[property: CliOption("--instance-event-window-id")] string InstanceEventWindowId,
[property: CliOption("--association-target")] string AssociationTarget
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}