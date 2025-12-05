using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "admin-update-auth-event-feedback")]
public record AwsCognitoIdpAdminUpdateAuthEventFeedbackOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--username")] string Username,
[property: CliOption("--event-id")] string EventId,
[property: CliOption("--feedback-value")] string FeedbackValue
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}