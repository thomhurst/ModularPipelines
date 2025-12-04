using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "mongocluster", "create")]
public record AzCosmosdbMongoclusterCreateOptions(
[property: CliOption("--administrator-login")] string AdministratorLogin,
[property: CliOption("--administrator-login-password")] string AdministratorLoginPassword,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--location")] string Location,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server-version")] string ServerVersion,
[property: CliOption("--shard-node-count")] int ShardNodeCount,
[property: CliOption("--shard-node-disk-size-gb")] string ShardNodeDiskSizeGb,
[property: CliFlag("--shard-node-ha")] bool ShardNodeHa,
[property: CliOption("--shard-node-tier")] string ShardNodeTier
) : AzOptions
{
    [CliOption("--tags")]
    public string? Tags { get; set; }
}