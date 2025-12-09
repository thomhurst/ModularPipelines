using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-verified-access-trust-provider")]
public record AwsEc2ModifyVerifiedAccessTrustProviderOptions(
[property: CliOption("--verified-access-trust-provider-id")] string VerifiedAccessTrustProviderId
) : AwsOptions
{
    [CliOption("--oidc-options")]
    public string? OidcOptions { get; set; }

    [CliOption("--device-options")]
    public string? DeviceOptions { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--sse-specification")]
    public string? SseSpecification { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}