using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "confirm-forgot-password")]
public record AwsCognitoIdpConfirmForgotPasswordOptions(
[property: CliOption("--client-id")] string ClientId,
[property: CliOption("--username")] string Username,
[property: CliOption("--confirmation-code")] string ConfirmationCode,
[property: CliOption("--password")] string Password
) : AwsOptions
{
    [CliOption("--secret-hash")]
    public string? SecretHash { get; set; }

    [CliOption("--analytics-metadata")]
    public string? AnalyticsMetadata { get; set; }

    [CliOption("--user-context-data")]
    public string? UserContextData { get; set; }

    [CliOption("--client-metadata")]
    public IEnumerable<KeyValue>? ClientMetadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}