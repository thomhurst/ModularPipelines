using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "create-realtime-log-config")]
public record AwsCloudfrontCreateRealtimeLogConfigOptions(
[property: CommandSwitch("--end-points")] string[] EndPoints,
[property: CommandSwitch("--fields")] string[] Fields,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--sampling-rate")] long SamplingRate
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}