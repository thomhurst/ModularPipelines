using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("voice-id", "delete-watchlist")]
public record AwsVoiceIdDeleteWatchlistOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--watchlist-id")] string WatchlistId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}