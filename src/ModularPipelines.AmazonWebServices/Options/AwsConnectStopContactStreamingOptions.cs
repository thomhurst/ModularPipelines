using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "stop-contact-streaming")]
public record AwsConnectStopContactStreamingOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--contact-id")] string ContactId,
[property: CommandSwitch("--streaming-id")] string StreamingId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}