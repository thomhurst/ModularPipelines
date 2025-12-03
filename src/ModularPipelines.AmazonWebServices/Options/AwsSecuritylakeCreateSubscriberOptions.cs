using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securitylake", "create-subscriber")]
public record AwsSecuritylakeCreateSubscriberOptions(
[property: CliOption("--sources")] string[] Sources,
[property: CliOption("--subscriber-identity")] string SubscriberIdentity,
[property: CliOption("--subscriber-name")] string SubscriberName
) : AwsOptions
{
    [CliOption("--access-types")]
    public string[]? AccessTypes { get; set; }

    [CliOption("--subscriber-description")]
    public string? SubscriberDescription { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}