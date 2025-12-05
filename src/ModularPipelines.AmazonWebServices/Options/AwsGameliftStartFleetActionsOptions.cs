using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "start-fleet-actions")]
public record AwsGameliftStartFleetActionsOptions(
[property: CliOption("--fleet-id")] string FleetId,
[property: CliOption("--actions")] string[] Actions
) : AwsOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}