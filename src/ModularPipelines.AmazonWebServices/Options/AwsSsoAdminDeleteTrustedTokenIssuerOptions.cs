using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "delete-trusted-token-issuer")]
public record AwsSsoAdminDeleteTrustedTokenIssuerOptions(
[property: CliOption("--trusted-token-issuer-arn")] string TrustedTokenIssuerArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}