using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "update-auth-event-feedback")]
public record AwsCognitoIdpUpdateAuthEventFeedbackOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--username")] string Username,
[property: CliOption("--event-id")] string EventId,
[property: CliOption("--feedback-token")] string FeedbackToken,
[property: CliOption("--feedback-value")] string FeedbackValue
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}