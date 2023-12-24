using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "respond-to-auth-challenge")]
public record AwsCognitoIdpRespondToAuthChallengeOptions(
[property: CommandSwitch("--client-id")] string ClientId,
[property: CommandSwitch("--challenge-name")] string ChallengeName
) : AwsOptions
{
    [CommandSwitch("--session")]
    public string? Session { get; set; }

    [CommandSwitch("--challenge-responses")]
    public IEnumerable<KeyValue>? ChallengeResponses { get; set; }

    [CommandSwitch("--analytics-metadata")]
    public string? AnalyticsMetadata { get; set; }

    [CommandSwitch("--user-context-data")]
    public string? UserContextData { get; set; }

    [CommandSwitch("--client-metadata")]
    public IEnumerable<KeyValue>? ClientMetadata { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}