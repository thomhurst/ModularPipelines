using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securitylake", "update-subscriber")]
public record AwsSecuritylakeUpdateSubscriberOptions(
[property: CliOption("--subscriber-id")] string SubscriberId
) : AwsOptions
{
    [CliOption("--sources")]
    public string[]? Sources { get; set; }

    [CliOption("--subscriber-description")]
    public string? SubscriberDescription { get; set; }

    [CliOption("--subscriber-identity")]
    public string? SubscriberIdentity { get; set; }

    [CliOption("--subscriber-name")]
    public string? SubscriberName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}