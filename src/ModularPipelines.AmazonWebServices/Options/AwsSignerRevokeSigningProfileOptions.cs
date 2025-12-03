using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("signer", "revoke-signing-profile")]
public record AwsSignerRevokeSigningProfileOptions(
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--profile-version")] string ProfileVersion,
[property: CliOption("--reason")] string Reason,
[property: CliOption("--effective-time")] long EffectiveTime
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}