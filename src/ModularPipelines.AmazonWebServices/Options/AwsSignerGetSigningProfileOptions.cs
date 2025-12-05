using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("signer", "get-signing-profile")]
public record AwsSignerGetSigningProfileOptions(
[property: CliOption("--profile-name")] string ProfileName
) : AwsOptions
{
    [CliOption("--profile-owner")]
    public string? ProfileOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}