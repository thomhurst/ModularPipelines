using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "set-risk-configuration")]
public record AwsCognitoIdpSetRiskConfigurationOptions(
[property: CommandSwitch("--user-pool-id")] string UserPoolId
) : AwsOptions
{
    [CommandSwitch("--client-id")]
    public string? ClientId { get; set; }

    [CommandSwitch("--compromised-credentials-risk-configuration")]
    public string? CompromisedCredentialsRiskConfiguration { get; set; }

    [CommandSwitch("--account-takeover-risk-configuration")]
    public string? AccountTakeoverRiskConfiguration { get; set; }

    [CommandSwitch("--risk-exception-configuration")]
    public string? RiskExceptionConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}