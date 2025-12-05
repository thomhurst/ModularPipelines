using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-hosts")]
public record AwsEc2ModifyHostsOptions(
[property: CliOption("--host-ids")] string[] HostIds
) : AwsOptions
{
    [CliOption("--auto-placement")]
    public string? AutoPlacement { get; set; }

    [CliOption("--host-recovery")]
    public string? HostRecovery { get; set; }

    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--instance-family")]
    public string? InstanceFamily { get; set; }

    [CliOption("--host-maintenance")]
    public string? HostMaintenance { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}