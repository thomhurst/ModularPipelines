using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "create-trusted-token-issuer")]
public record AwsSsoAdminCreateTrustedTokenIssuerOptions(
[property: CliOption("--instance-arn")] string InstanceArn,
[property: CliOption("--name")] string Name,
[property: CliOption("--trusted-token-issuer-configuration")] string TrustedTokenIssuerConfiguration,
[property: CliOption("--trusted-token-issuer-type")] string TrustedTokenIssuerType
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}