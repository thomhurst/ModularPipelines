using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-email", "delete-dedicated-ip-pool")]
public record AwsPinpointEmailDeleteDedicatedIpPoolOptions(
[property: CommandSwitch("--pool-name")] string PoolName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}