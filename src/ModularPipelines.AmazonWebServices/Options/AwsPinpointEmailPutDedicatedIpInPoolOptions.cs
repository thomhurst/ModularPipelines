using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-email", "put-dedicated-ip-in-pool")]
public record AwsPinpointEmailPutDedicatedIpInPoolOptions(
[property: CommandSwitch("--ip")] string Ip,
[property: CommandSwitch("--destination-pool-name")] string DestinationPoolName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}