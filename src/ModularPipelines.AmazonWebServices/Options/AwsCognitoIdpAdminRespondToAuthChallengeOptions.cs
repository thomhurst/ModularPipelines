using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "admin-respond-to-auth-challenge")]
public record AwsCognitoIdpAdminRespondToAuthChallengeOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--client-id")] string ClientId,
[property: CliOption("--challenge-name")] string ChallengeName
) : AwsOptions
{
    [CliOption("--challenge-responses")]
    public IEnumerable<KeyValue>? ChallengeResponses { get; set; }

    [CliOption("--session")]
    public string? Session { get; set; }

    [CliOption("--analytics-metadata")]
    public string? AnalyticsMetadata { get; set; }

    [CliOption("--context-data")]
    public string? ContextData { get; set; }

    [CliOption("--client-metadata")]
    public IEnumerable<KeyValue>? ClientMetadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}