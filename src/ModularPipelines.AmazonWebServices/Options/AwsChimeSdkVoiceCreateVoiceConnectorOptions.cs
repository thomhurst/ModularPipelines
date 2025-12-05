using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "create-voice-connector")]
public record AwsChimeSdkVoiceCreateVoiceConnectorOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--aws-region")]
    public string? AwsRegion { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}