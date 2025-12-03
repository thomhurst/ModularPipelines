using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "bastion", "ssh")]
public record AzNetworkBastionSshOptions(
[property: CliOption("--auth-type")] string AuthType
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-port")]
    public string? ResourcePort { get; set; }

    [CliOption("--ssh-key")]
    public string? SshKey { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--target-ip-address")]
    public string? TargetIpAddress { get; set; }

    [CliOption("--target-resource-id")]
    public string? TargetResourceId { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}