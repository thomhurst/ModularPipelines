using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "update-sip-media-application-call")]
public record AwsChimeSdkVoiceUpdateSipMediaApplicationCallOptions(
[property: CliOption("--sip-media-application-id")] string SipMediaApplicationId,
[property: CliOption("--transaction-id")] string TransactionId,
[property: CliOption("--arguments")] IEnumerable<KeyValue> AwsChimArguments
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}