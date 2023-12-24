using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "create-voice-profile-domain")]
public record AwsChimeSdkVoiceCreateVoiceProfileDomainOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--server-side-encryption-configuration")] string ServerSideEncryptionConfiguration
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}