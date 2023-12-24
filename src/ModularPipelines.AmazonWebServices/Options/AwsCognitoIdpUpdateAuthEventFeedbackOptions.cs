using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "update-auth-event-feedback")]
public record AwsCognitoIdpUpdateAuthEventFeedbackOptions(
[property: CommandSwitch("--user-pool-id")] string UserPoolId,
[property: CommandSwitch("--username")] string Username,
[property: CommandSwitch("--event-id")] string EventId,
[property: CommandSwitch("--feedback-token")] string FeedbackToken,
[property: CommandSwitch("--feedback-value")] string FeedbackValue
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}