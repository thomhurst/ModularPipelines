using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securitylake", "delete-subscriber-notification")]
public record AwsSecuritylakeDeleteSubscriberNotificationOptions(
[property: CommandSwitch("--subscriber-id")] string SubscriberId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}