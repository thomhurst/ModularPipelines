using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "send-text-message")]
public record AwsPinpointSmsVoiceV2SendTextMessageOptions(
[property: CliOption("--destination-phone-number")] string DestinationPhoneNumber
) : AwsOptions
{
    [CliOption("--origination-identity")]
    public string? OriginationIdentity { get; set; }

    [CliOption("--message-body")]
    public string? MessageBody { get; set; }

    [CliOption("--message-type")]
    public string? MessageType { get; set; }

    [CliOption("--keyword")]
    public string? Keyword { get; set; }

    [CliOption("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CliOption("--max-price")]
    public string? MaxPrice { get; set; }

    [CliOption("--time-to-live")]
    public int? TimeToLive { get; set; }

    [CliOption("--context")]
    public IEnumerable<KeyValue>? Context { get; set; }

    [CliOption("--destination-country-parameters")]
    public IEnumerable<KeyValue>? DestinationCountryParameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}