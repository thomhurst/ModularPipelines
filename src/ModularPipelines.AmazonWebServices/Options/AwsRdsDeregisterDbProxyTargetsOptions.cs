using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "deregister-db-proxy-targets")]
public record AwsRdsDeregisterDbProxyTargetsOptions(
[property: CommandSwitch("--db-proxy-name")] string DbProxyName
) : AwsOptions
{
    [CommandSwitch("--target-group-name")]
    public string? TargetGroupName { get; set; }

    [CommandSwitch("--db-instance-identifiers")]
    public string[]? DbInstanceIdentifiers { get; set; }

    [CommandSwitch("--db-cluster-identifiers")]
    public string[]? DbClusterIdentifiers { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}