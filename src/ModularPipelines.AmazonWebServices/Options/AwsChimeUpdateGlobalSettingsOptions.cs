using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "update-global-settings")]
public record AwsChimeUpdateGlobalSettingsOptions : AwsOptions
{
    [CliOption("--business-calling")]
    public string? BusinessCalling { get; set; }

    [CliOption("--voice-connector")]
    public string? VoiceConnector { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}