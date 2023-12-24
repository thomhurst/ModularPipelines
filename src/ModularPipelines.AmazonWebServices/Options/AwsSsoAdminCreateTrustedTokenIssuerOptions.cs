using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "create-trusted-token-issuer")]
public record AwsSsoAdminCreateTrustedTokenIssuerOptions(
[property: CommandSwitch("--instance-arn")] string InstanceArn,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--trusted-token-issuer-configuration")] string TrustedTokenIssuerConfiguration,
[property: CommandSwitch("--trusted-token-issuer-type")] string TrustedTokenIssuerType
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}