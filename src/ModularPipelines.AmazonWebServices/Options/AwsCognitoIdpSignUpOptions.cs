using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "sign-up")]
public record AwsCognitoIdpSignUpOptions(
[property: CliOption("--client-id")] string ClientId,
[property: CliOption("--username")] string Username,
[property: CliOption("--password")] string Password
) : AwsOptions
{
    [CliOption("--secret-hash")]
    public string? SecretHash { get; set; }

    [CliOption("--user-attributes")]
    public string[]? UserAttributes { get; set; }

    [CliOption("--validation-data")]
    public string[]? ValidationData { get; set; }

    [CliOption("--analytics-metadata")]
    public string? AnalyticsMetadata { get; set; }

    [CliOption("--user-context-data")]
    public string? UserContextData { get; set; }

    [CliOption("--client-metadata")]
    public IEnumerable<KeyValue>? ClientMetadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}