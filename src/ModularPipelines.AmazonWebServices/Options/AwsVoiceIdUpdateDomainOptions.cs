using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("voice-id", "update-domain")]
public record AwsVoiceIdUpdateDomainOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--server-side-encryption-configuration")] string ServerSideEncryptionConfiguration
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}