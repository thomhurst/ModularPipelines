using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securitylake", "delete-subscriber-notification")]
public record AwsSecuritylakeDeleteSubscriberNotificationOptions(
[property: CliOption("--subscriber-id")] string SubscriberId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}