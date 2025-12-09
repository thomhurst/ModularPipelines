using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "create-sip-media-application")]
public record AwsChimeSdkVoiceCreateSipMediaApplicationOptions(
[property: CliOption("--aws-region")] string AwsRegion,
[property: CliOption("--name")] string Name,
[property: CliOption("--endpoints")] string[] Endpoints
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}