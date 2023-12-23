using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup-gateway", "start-virtual-machines-metadata-sync")]
public record AwsBackupGatewayStartVirtualMachinesMetadataSyncOptions(
[property: CommandSwitch("--hypervisor-arn")] string HypervisorArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}