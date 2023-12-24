using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "update-realtime-log-config")]
public record AwsCloudfrontUpdateRealtimeLogConfigOptions : AwsOptions
{
    [CommandSwitch("--end-points")]
    public string[]? EndPoints { get; set; }

    [CommandSwitch("--fields")]
    public string[]? Fields { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--arn")]
    public string? Arn { get; set; }

    [CommandSwitch("--sampling-rate")]
    public long? SamplingRate { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}