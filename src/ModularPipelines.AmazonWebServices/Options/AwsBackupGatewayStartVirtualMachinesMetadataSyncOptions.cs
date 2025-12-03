using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup-gateway", "start-virtual-machines-metadata-sync")]
public record AwsBackupGatewayStartVirtualMachinesMetadataSyncOptions(
[property: CliOption("--hypervisor-arn")] string HypervisorArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}