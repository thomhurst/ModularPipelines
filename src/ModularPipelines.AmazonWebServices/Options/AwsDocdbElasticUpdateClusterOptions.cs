using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("docdb-elastic", "update-cluster")]
public record AwsDocdbElasticUpdateClusterOptions(
[property: CommandSwitch("--cluster-arn")] string ClusterArn
) : AwsOptions
{
    [CommandSwitch("--admin-user-password")]
    public string? AdminUserPassword { get; set; }

    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CommandSwitch("--shard-capacity")]
    public int? ShardCapacity { get; set; }

    [CommandSwitch("--shard-count")]
    public int? ShardCount { get; set; }

    [CommandSwitch("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CommandSwitch("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}