using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("voice-id", "describe-watchlist")]
public record AwsVoiceIdDescribeWatchlistOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--watchlist-id")] string WatchlistId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}