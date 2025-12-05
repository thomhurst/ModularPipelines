using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sns", "confirm-subscription")]
public record AwsSnsConfirmSubscriptionOptions(
[property: CliOption("--topic-arn")] string TopicArn,
[property: CliOption("--token")] string Token
) : AwsOptions
{
    [CliOption("--authenticate-on-unsubscribe")]
    public string? AuthenticateOnUnsubscribe { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}