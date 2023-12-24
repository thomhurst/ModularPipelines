using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("signer", "sign-payload")]
public record AwsSignerSignPayloadOptions(
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--payload")] string Payload,
[property: CommandSwitch("--payload-format")] string PayloadFormat
) : AwsOptions
{
    [CommandSwitch("--profile-owner")]
    public string? ProfileOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}