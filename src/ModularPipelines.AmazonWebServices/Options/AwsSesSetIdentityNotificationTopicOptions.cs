using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "set-identity-notification-topic")]
public record AwsSesSetIdentityNotificationTopicOptions(
[property: CommandSwitch("--identity")] string Identity,
[property: CommandSwitch("--notification-type")] string NotificationType
) : AwsOptions
{
    [CommandSwitch("--sns-topic")]
    public string? SnsTopic { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}