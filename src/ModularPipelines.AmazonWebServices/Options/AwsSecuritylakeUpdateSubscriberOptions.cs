using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securitylake", "update-subscriber")]
public record AwsSecuritylakeUpdateSubscriberOptions(
[property: CommandSwitch("--subscriber-id")] string SubscriberId
) : AwsOptions
{
    [CommandSwitch("--sources")]
    public string[]? Sources { get; set; }

    [CommandSwitch("--subscriber-description")]
    public string? SubscriberDescription { get; set; }

    [CommandSwitch("--subscriber-identity")]
    public string? SubscriberIdentity { get; set; }

    [CommandSwitch("--subscriber-name")]
    public string? SubscriberName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}