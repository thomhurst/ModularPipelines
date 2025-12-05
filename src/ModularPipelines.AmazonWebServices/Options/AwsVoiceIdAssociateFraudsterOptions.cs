using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("voice-id", "associate-fraudster")]
public record AwsVoiceIdAssociateFraudsterOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--fraudster-id")] string FraudsterId,
[property: CliOption("--watchlist-id")] string WatchlistId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}