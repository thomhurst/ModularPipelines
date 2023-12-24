using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("voice-id", "associate-fraudster")]
public record AwsVoiceIdAssociateFraudsterOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--fraudster-id")] string FraudsterId,
[property: CommandSwitch("--watchlist-id")] string WatchlistId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}