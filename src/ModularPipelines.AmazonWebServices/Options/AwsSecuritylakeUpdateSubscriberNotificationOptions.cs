using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securitylake", "update-subscriber-notification")]
public record AwsSecuritylakeUpdateSubscriberNotificationOptions(
[property: CommandSwitch("--configuration")] string Configuration,
[property: CommandSwitch("--subscriber-id")] string SubscriberId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}