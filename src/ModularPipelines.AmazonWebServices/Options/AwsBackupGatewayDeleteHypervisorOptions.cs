using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup-gateway", "delete-hypervisor")]
public record AwsBackupGatewayDeleteHypervisorOptions(
[property: CommandSwitch("--hypervisor-arn")] string HypervisorArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}