using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "create-db-proxy")]
public record AwsRdsCreateDbProxyOptions(
[property: CommandSwitch("--db-proxy-name")] string DbProxyName,
[property: CommandSwitch("--engine-family")] string EngineFamily,
[property: CommandSwitch("--auth")] string[] Auth,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--vpc-subnet-ids")] string[] VpcSubnetIds
) : AwsOptions
{
    [CommandSwitch("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CommandSwitch("--idle-client-timeout")]
    public int? IdleClientTimeout { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}