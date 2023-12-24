using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-contacts", "activate-contact-channel")]
public record AwsSsmContactsActivateContactChannelOptions(
[property: CommandSwitch("--contact-channel-id")] string ContactChannelId,
[property: CommandSwitch("--activation-code")] string ActivationCode
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}