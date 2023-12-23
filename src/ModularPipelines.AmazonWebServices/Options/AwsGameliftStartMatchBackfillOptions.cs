using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "start-match-backfill")]
public record AwsGameliftStartMatchBackfillOptions(
[property: CommandSwitch("--configuration-name")] string ConfigurationName,
[property: CommandSwitch("--players")] string[] Players
) : AwsOptions
{
    [CommandSwitch("--ticket-id")]
    public string? TicketId { get; set; }

    [CommandSwitch("--game-session-arn")]
    public string? GameSessionArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}