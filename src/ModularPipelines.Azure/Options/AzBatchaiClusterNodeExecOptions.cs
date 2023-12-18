using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batchai", "cluster", "node", "exec")]
public record AzBatchaiClusterNodeExecOptions(
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace")] string Workspace
) : AzOptions
{
    [CommandSwitch("--address")]
    public string? Address { get; set; }

    [CommandSwitch("--exec")]
    public string? Exec { get; set; }

    [CommandSwitch("--node-id")]
    public string? NodeId { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--ssh-private-key")]
    public string? SshPrivateKey { get; set; }
}