using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("signer", "revoke-signing-profile")]
public record AwsSignerRevokeSigningProfileOptions(
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--profile-version")] string ProfileVersion,
[property: CommandSwitch("--reason")] string Reason,
[property: CommandSwitch("--effective-time")] long EffectiveTime
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}