using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "update-fleet-attributes")]
public record AwsGameliftUpdateFleetAttributesOptions(
[property: CliOption("--fleet-id")] string FleetId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--new-game-session-protection-policy")]
    public string? NewGameSessionProtectionPolicy { get; set; }

    [CliOption("--resource-creation-limit-policy")]
    public string? ResourceCreationLimitPolicy { get; set; }

    [CliOption("--metric-groups")]
    public string[]? MetricGroups { get; set; }

    [CliOption("--anywhere-configuration")]
    public string? AnywhereConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}