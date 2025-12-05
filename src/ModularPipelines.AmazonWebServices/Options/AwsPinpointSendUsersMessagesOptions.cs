using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "send-users-messages")]
public record AwsPinpointSendUsersMessagesOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--send-users-message-request")] string SendUsersMessageRequest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}