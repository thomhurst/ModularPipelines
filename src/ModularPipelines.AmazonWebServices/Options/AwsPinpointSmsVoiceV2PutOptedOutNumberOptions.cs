using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "put-opted-out-number")]
public record AwsPinpointSmsVoiceV2PutOptedOutNumberOptions(
[property: CliOption("--opt-out-list-name")] string OptOutListName,
[property: CliOption("--opted-out-number")] string OptedOutNumber
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}