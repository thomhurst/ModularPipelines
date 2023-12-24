using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "modify-db-proxy-target-group")]
public record AwsRdsModifyDbProxyTargetGroupOptions(
[property: CommandSwitch("--target-group-name")] string TargetGroupName,
[property: CommandSwitch("--db-proxy-name")] string DbProxyName
) : AwsOptions
{
    [CommandSwitch("--connection-pool-config")]
    public string? ConnectionPoolConfig { get; set; }

    [CommandSwitch("--new-name")]
    public string? NewName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}