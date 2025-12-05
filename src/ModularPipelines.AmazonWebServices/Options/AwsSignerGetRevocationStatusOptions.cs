using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("signer", "get-revocation-status")]
public record AwsSignerGetRevocationStatusOptions(
[property: CliOption("--signature-timestamp")] long SignatureTimestamp,
[property: CliOption("--platform-id")] string PlatformId,
[property: CliOption("--profile-version-arn")] string ProfileVersionArn,
[property: CliOption("--job-arn")] string JobArn,
[property: CliOption("--certificate-hashes")] string[] CertificateHashes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}