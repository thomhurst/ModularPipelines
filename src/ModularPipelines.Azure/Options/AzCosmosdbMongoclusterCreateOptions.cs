using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongocluster", "create")]
public record AzCosmosdbMongoclusterCreateOptions(
[property: CommandSwitch("--administrator-login")] string AdministratorLogin,
[property: CommandSwitch("--administrator-login-password")] string AdministratorLoginPassword,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--server-version")] string ServerVersion,
[property: CommandSwitch("--shard-node-count")] int ShardNodeCount,
[property: CommandSwitch("--shard-node-disk-size-gb")] string ShardNodeDiskSizeGb,
[property: BooleanCommandSwitch("--shard-node-ha")] bool ShardNodeHa,
[property: CommandSwitch("--shard-node-tier")] string ShardNodeTier
) : AzOptions
{
    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}