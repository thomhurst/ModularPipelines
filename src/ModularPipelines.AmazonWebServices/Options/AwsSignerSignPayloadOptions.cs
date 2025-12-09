using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("signer", "sign-payload")]
public record AwsSignerSignPayloadOptions(
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--payload")] string Payload,
[property: CliOption("--payload-format")] string PayloadFormat
) : AwsOptions
{
    [CliOption("--profile-owner")]
    public string? ProfileOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}