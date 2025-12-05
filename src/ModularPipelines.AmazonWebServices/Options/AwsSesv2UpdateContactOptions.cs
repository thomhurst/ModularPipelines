using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "update-contact")]
public record AwsSesv2UpdateContactOptions(
[property: CliOption("--contact-list-name")] string ContactListName,
[property: CliOption("--email-address")] string EmailAddress
) : AwsOptions
{
    [CliOption("--topic-preferences")]
    public string[]? TopicPreferences { get; set; }

    [CliOption("--attributes-data")]
    public string? AttributesData { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}