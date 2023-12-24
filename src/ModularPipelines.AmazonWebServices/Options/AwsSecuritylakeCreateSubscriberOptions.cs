using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securitylake", "create-subscriber")]
public record AwsSecuritylakeCreateSubscriberOptions(
[property: CommandSwitch("--sources")] string[] Sources,
[property: CommandSwitch("--subscriber-identity")] string SubscriberIdentity,
[property: CommandSwitch("--subscriber-name")] string SubscriberName
) : AwsOptions
{
    [CommandSwitch("--access-types")]
    public string[]? AccessTypes { get; set; }

    [CommandSwitch("--subscriber-description")]
    public string? SubscriberDescription { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}