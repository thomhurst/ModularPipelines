using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "put-dedicated-ip-pool-scaling-attributes")]
public record AwsSesv2PutDedicatedIpPoolScalingAttributesOptions(
[property: CommandSwitch("--pool-name")] string PoolName,
[property: CommandSwitch("--scaling-mode")] string ScalingMode
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}