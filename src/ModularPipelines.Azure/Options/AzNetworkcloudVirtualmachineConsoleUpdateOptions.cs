using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud", "virtualmachine", "console", "update")]
public record AzNetworkcloudVirtualmachineConsoleUpdateOptions : AzOptions
{
    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--expiration")]
    public string? Expiration { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--ssh-public-key")]
    public string? SshPublicKey { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--virtual-machine-name")]
    public string? VirtualMachineName { get; set; }
}