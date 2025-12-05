using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "update-trusted-token-issuer")]
public record AwsSsoAdminUpdateTrustedTokenIssuerOptions(
[property: CliOption("--trusted-token-issuer-arn")] string TrustedTokenIssuerArn
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--trusted-token-issuer-configuration")]
    public string? TrustedTokenIssuerConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}