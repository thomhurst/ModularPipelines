using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-email", "put-dedicated-ip-warmup-attributes")]
public record AwsPinpointEmailPutDedicatedIpWarmupAttributesOptions(
[property: CommandSwitch("--ip")] string Ip,
[property: CommandSwitch("--warmup-percentage")] int WarmupPercentage
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}