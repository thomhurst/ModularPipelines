using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "update-contact")]
public record AwsSesv2UpdateContactOptions(
[property: CommandSwitch("--contact-list-name")] string ContactListName,
[property: CommandSwitch("--email-address")] string EmailAddress
) : AwsOptions
{
    [CommandSwitch("--topic-preferences")]
    public string[]? TopicPreferences { get; set; }

    [CommandSwitch("--attributes-data")]
    public string? AttributesData { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}