using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "modify-db-proxy-endpoint")]
public record AwsRdsModifyDbProxyEndpointOptions(
[property: CommandSwitch("--db-proxy-endpoint-name")] string DbProxyEndpointName
) : AwsOptions
{
    [CommandSwitch("--new-db-proxy-endpoint-name")]
    public string? NewDbProxyEndpointName { get; set; }

    [CommandSwitch("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}