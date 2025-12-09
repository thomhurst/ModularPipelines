using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "start-match-backfill")]
public record AwsGameliftStartMatchBackfillOptions(
[property: CliOption("--configuration-name")] string ConfigurationName,
[property: CliOption("--players")] string[] Players
) : AwsOptions
{
    [CliOption("--ticket-id")]
    public string? TicketId { get; set; }

    [CliOption("--game-session-arn")]
    public string? GameSessionArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}