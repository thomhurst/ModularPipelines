using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "bastion", "ssh")]
public record AzNetworkBastionSshOptions(
[property: CommandSwitch("--auth-type")] string AuthType
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-port")]
    public string? ResourcePort { get; set; }

    [CommandSwitch("--ssh-key")]
    public string? SshKey { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--target-ip-address")]
    public string? TargetIpAddress { get; set; }

    [CommandSwitch("--target-resource-id")]
    public string? TargetResourceId { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }
}