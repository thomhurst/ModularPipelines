using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "update-sip-media-application-call")]
public record AwsChimeSdkVoiceUpdateSipMediaApplicationCallOptions(
[property: CommandSwitch("--sip-media-application-id")] string SipMediaApplicationId,
[property: CommandSwitch("--transaction-id")] string TransactionId,
[property: CommandSwitch("--arguments")] IEnumerable<KeyValue> AwsChimArguments
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}