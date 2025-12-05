using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "set-risk-configuration")]
public record AwsCognitoIdpSetRiskConfigurationOptions(
[property: CliOption("--user-pool-id")] string UserPoolId
) : AwsOptions
{
    [CliOption("--client-id")]
    public string? ClientId { get; set; }

    [CliOption("--compromised-credentials-risk-configuration")]
    public string? CompromisedCredentialsRiskConfiguration { get; set; }

    [CliOption("--account-takeover-risk-configuration")]
    public string? AccountTakeoverRiskConfiguration { get; set; }

    [CliOption("--risk-exception-configuration")]
    public string? RiskExceptionConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}