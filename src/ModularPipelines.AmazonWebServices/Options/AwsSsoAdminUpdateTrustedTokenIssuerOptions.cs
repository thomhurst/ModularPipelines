using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "update-trusted-token-issuer")]
public record AwsSsoAdminUpdateTrustedTokenIssuerOptions(
[property: CommandSwitch("--trusted-token-issuer-arn")] string TrustedTokenIssuerArn
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--trusted-token-issuer-configuration")]
    public string? TrustedTokenIssuerConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}