using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup-gateway", "get-hypervisor")]
public record AwsBackupGatewayGetHypervisorOptions(
[property: CommandSwitch("--hypervisor-arn")] string HypervisorArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}