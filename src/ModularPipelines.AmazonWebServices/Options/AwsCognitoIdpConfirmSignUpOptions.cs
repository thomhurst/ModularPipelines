using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "confirm-sign-up")]
public record AwsCognitoIdpConfirmSignUpOptions(
[property: CliOption("--client-id")] string ClientId,
[property: CliOption("--username")] string Username,
[property: CliOption("--confirmation-code")] string ConfirmationCode
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