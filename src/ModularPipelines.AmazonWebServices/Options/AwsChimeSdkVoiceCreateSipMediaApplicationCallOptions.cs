using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "create-sip-media-application-call")]
public record AwsChimeSdkVoiceCreateSipMediaApplicationCallOptions(
[property: CliOption("--from-phone-number")] string FromPhoneNumber,
[property: CliOption("--to-phone-number")] string ToPhoneNumber,
[property: CliOption("--sip-media-application-id")] string SipMediaApplicationId
) : AwsOptions
{
    [CliOption("--sip-headers")]
    public IEnumerable<KeyValue>? SipHeaders { get; set; }

    [CliOption("--arguments-map")]
    public IEnumerable<KeyValue>? ArgumentsMap { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}