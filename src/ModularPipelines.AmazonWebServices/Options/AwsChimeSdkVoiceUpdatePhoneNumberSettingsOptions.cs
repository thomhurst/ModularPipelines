using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "update-phone-number-settings")]
public record AwsChimeSdkVoiceUpdatePhoneNumberSettingsOptions(
[property: CliOption("--calling-name")] string CallingName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}