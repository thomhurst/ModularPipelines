using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup-gateway", "test-hypervisor-configuration")]
public record AwsBackupGatewayTestHypervisorConfigurationOptions(
[property: CommandSwitch("--gateway-arn")] string GatewayArn,
[property: CommandSwitch("--host")] string Host
) : AwsOptions
{
    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}