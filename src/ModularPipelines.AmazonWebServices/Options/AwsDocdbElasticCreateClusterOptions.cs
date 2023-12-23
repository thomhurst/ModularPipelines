using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("docdb-elastic", "create-cluster")]
public record AwsDocdbElasticCreateClusterOptions(
[property: CommandSwitch("--admin-user-name")] string AdminUserName,
[property: CommandSwitch("--admin-user-password")] string AdminUserPassword,
[property: CommandSwitch("--auth-type")] string AuthType,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--shard-capacity")] int ShardCapacity,
[property: CommandSwitch("--shard-count")] int ShardCount
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CommandSwitch("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}