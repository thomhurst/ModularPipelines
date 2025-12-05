using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("docdb-elastic", "create-cluster")]
public record AwsDocdbElasticCreateClusterOptions(
[property: CliOption("--admin-user-name")] string AdminUserName,
[property: CliOption("--admin-user-password")] string AdminUserPassword,
[property: CliOption("--auth-type")] string AuthType,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--shard-capacity")] int ShardCapacity,
[property: CliOption("--shard-count")] int ShardCount
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}