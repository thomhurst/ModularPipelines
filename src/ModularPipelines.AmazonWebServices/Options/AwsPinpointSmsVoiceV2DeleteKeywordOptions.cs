using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "delete-keyword")]
public record AwsPinpointSmsVoiceV2DeleteKeywordOptions(
[property: CliOption("--origination-identity")] string OriginationIdentity,
[property: CliOption("--keyword")] string Keyword
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}