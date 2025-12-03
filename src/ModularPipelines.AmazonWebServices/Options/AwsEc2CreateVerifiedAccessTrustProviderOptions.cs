using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-verified-access-trust-provider")]
public record AwsEc2CreateVerifiedAccessTrustProviderOptions(
[property: CliOption("--trust-provider-type")] string TrustProviderType,
[property: CliOption("--policy-reference-name")] string PolicyReferenceName
) : AwsOptions
{
    [CliOption("--user-trust-provider-type")]
    public string? UserTrustProviderType { get; set; }

    [CliOption("--device-trust-provider-type")]
    public string? DeviceTrustProviderType { get; set; }

    [CliOption("--oidc-options")]
    public string? OidcOptions { get; set; }

    [CliOption("--device-options")]
    public string? DeviceOptions { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--sse-specification")]
    public string? SseSpecification { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}