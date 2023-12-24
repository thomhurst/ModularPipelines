using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("voice-id", "delete-fraudster")]
public record AwsVoiceIdDeleteFraudsterOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--fraudster-id")] string FraudsterId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}