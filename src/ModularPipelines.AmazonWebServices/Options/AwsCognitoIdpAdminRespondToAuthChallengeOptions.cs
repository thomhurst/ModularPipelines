using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "admin-respond-to-auth-challenge")]
public record AwsCognitoIdpAdminRespondToAuthChallengeOptions(
[property: CommandSwitch("--user-pool-id")] string UserPoolId,
[property: CommandSwitch("--client-id")] string ClientId,
[property: CommandSwitch("--challenge-name")] string ChallengeName
) : AwsOptions
{
    [CommandSwitch("--challenge-responses")]
    public IEnumerable<KeyValue>? ChallengeResponses { get; set; }

    [CommandSwitch("--session")]
    public string? Session { get; set; }

    [CommandSwitch("--analytics-metadata")]
    public string? AnalyticsMetadata { get; set; }

    [CommandSwitch("--context-data")]
    public string? ContextData { get; set; }

    [CommandSwitch("--client-metadata")]
    public IEnumerable<KeyValue>? ClientMetadata { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}