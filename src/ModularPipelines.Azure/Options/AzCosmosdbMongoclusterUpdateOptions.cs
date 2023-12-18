using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongocluster", "update")]
public record AzCosmosdbMongoclusterUpdateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--administrator-login")]
    public string? AdministratorLogin { get; set; }

    [CommandSwitch("--administrator-login-password")]
    public string? AdministratorLoginPassword { get; set; }

    [CommandSwitch("--server-version")]
    public string? ServerVersion { get; set; }

    [CommandSwitch("--shard-node-disk-size-gb")]
    public string? ShardNodeDiskSizeGb { get; set; }

    [BooleanCommandSwitch("--shard-node-ha")]
    public bool? ShardNodeHa { get; set; }

    [CommandSwitch("--shard-node-tier")]
    public string? ShardNodeTier { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

