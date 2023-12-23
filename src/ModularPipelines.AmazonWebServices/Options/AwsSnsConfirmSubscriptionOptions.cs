using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sns", "confirm-subscription")]
public record AwsSnsConfirmSubscriptionOptions(
[property: CommandSwitch("--topic-arn")] string TopicArn,
[property: CommandSwitch("--token")] string Token
) : AwsOptions
{
    [CommandSwitch("--authenticate-on-unsubscribe")]
    public string? AuthenticateOnUnsubscribe { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}