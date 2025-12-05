using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "delete-key-pair")]
public record AwsLightsailDeleteKeyPairOptions(
[property: CliOption("--key-pair-name")] string KeyPairName
) : AwsOptions
{
    [CliOption("--expected-fingerprint")]
    public string? ExpectedFingerprint { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}