using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-contacts", "delete-contact-channel")]
public record AwsSsmContactsDeleteContactChannelOptions(
[property: CliOption("--contact-channel-id")] string ContactChannelId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}