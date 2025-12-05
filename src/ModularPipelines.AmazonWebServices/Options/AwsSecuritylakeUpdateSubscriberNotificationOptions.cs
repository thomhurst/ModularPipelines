using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securitylake", "update-subscriber-notification")]
public record AwsSecuritylakeUpdateSubscriberNotificationOptions(
[property: CliOption("--configuration")] string Configuration,
[property: CliOption("--subscriber-id")] string SubscriberId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}