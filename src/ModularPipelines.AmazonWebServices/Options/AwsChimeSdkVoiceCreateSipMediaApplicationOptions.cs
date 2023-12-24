using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "create-sip-media-application")]
public record AwsChimeSdkVoiceCreateSipMediaApplicationOptions(
[property: CommandSwitch("--aws-region")] string AwsRegion,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--endpoints")] string[] Endpoints
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}