using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-contacts", "activate-contact-channel")]
public record AwsSsmContactsActivateContactChannelOptions(
[property: CliOption("--contact-channel-id")] string ContactChannelId,
[property: CliOption("--activation-code")] string ActivationCode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}