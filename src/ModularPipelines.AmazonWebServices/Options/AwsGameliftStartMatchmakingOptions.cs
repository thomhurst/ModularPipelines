using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "start-matchmaking")]
public record AwsGameliftStartMatchmakingOptions(
[property: CliOption("--configuration-name")] string ConfigurationName,
[property: CliOption("--players")] string[] Players
) : AwsOptions
{
    [CliOption("--ticket-id")]
    public string? TicketId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}