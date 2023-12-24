using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "create-db-proxy-endpoint")]
public record AwsRdsCreateDbProxyEndpointOptions(
[property: CommandSwitch("--db-proxy-name")] string DbProxyName,
[property: CommandSwitch("--db-proxy-endpoint-name")] string DbProxyEndpointName,
[property: CommandSwitch("--vpc-subnet-ids")] string[] VpcSubnetIds
) : AwsOptions
{
    [CommandSwitch("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CommandSwitch("--target-role")]
    public string? TargetRole { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}