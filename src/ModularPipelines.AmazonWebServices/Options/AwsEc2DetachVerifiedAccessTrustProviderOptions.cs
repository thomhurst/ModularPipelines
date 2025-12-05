using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "detach-verified-access-trust-provider")]
public record AwsEc2DetachVerifiedAccessTrustProviderOptions(
[property: CliOption("--verified-access-instance-id")] string VerifiedAccessInstanceId,
[property: CliOption("--verified-access-trust-provider-id")] string VerifiedAccessTrustProviderId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}