using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("signer", "get-signing-profile")]
public record AwsSignerGetSigningProfileOptions(
[property: CommandSwitch("--profile-name")] string ProfileName
) : AwsOptions
{
    [CommandSwitch("--profile-owner")]
    public string? ProfileOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}