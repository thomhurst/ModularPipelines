using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("voice-id", "disassociate-fraudster")]
public record AwsVoiceIdDisassociateFraudsterOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--fraudster-id")] string FraudsterId,
[property: CommandSwitch("--watchlist-id")] string WatchlistId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}