using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "mongocluster", "update")]
public record AzCosmosdbMongoclusterUpdateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--administrator-login")]
    public string? AdministratorLogin { get; set; }

    [CliOption("--administrator-login-password")]
    public string? AdministratorLoginPassword { get; set; }

    [CliOption("--server-version")]
    public string? ServerVersion { get; set; }

    [CliOption("--shard-node-disk-size-gb")]
    public string? ShardNodeDiskSizeGb { get; set; }

    [CliFlag("--shard-node-ha")]
    public bool? ShardNodeHa { get; set; }

    [CliOption("--shard-node-tier")]
    public string? ShardNodeTier { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}