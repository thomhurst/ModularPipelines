using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("signer", "get-revocation-status")]
public record AwsSignerGetRevocationStatusOptions(
[property: CommandSwitch("--signature-timestamp")] long SignatureTimestamp,
[property: CommandSwitch("--platform-id")] string PlatformId,
[property: CommandSwitch("--profile-version-arn")] string ProfileVersionArn,
[property: CommandSwitch("--job-arn")] string JobArn,
[property: CommandSwitch("--certificate-hashes")] string[] CertificateHashes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}