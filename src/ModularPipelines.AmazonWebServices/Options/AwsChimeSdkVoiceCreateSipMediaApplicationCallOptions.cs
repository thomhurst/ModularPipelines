using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "create-sip-media-application-call")]
public record AwsChimeSdkVoiceCreateSipMediaApplicationCallOptions(
[property: CommandSwitch("--from-phone-number")] string FromPhoneNumber,
[property: CommandSwitch("--to-phone-number")] string ToPhoneNumber,
[property: CommandSwitch("--sip-media-application-id")] string SipMediaApplicationId
) : AwsOptions
{
    [CommandSwitch("--sip-headers")]
    public IEnumerable<KeyValue>? SipHeaders { get; set; }

    [CommandSwitch("--arguments-map")]
    public IEnumerable<KeyValue>? ArgumentsMap { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}