using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "accept-match")]
public record AwsGameliftAcceptMatchOptions(
[property: CliOption("--ticket-id")] string TicketId,
[property: CliOption("--player-ids")] string[] PlayerIds,
[property: CliOption("--acceptance-type")] string AcceptanceType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}