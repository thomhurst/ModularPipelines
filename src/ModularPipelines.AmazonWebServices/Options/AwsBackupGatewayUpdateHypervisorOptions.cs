using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup-gateway", "update-hypervisor")]
public record AwsBackupGatewayUpdateHypervisorOptions(
[property: CommandSwitch("--hypervisor-arn")] string HypervisorArn
) : AwsOptions
{
    [CommandSwitch("--host")]
    public string? Host { get; set; }

    [CommandSwitch("--log-group-arn")]
    public string? LogGroupArn { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}