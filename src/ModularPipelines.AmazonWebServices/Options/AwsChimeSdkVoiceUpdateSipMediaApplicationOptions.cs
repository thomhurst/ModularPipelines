using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "update-sip-media-application")]
public record AwsChimeSdkVoiceUpdateSipMediaApplicationOptions(
[property: CliOption("--sip-media-application-id")] string SipMediaApplicationId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--endpoints")]
    public string[]? Endpoints { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}