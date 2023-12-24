using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "update-fleet-attributes")]
public record AwsGameliftUpdateFleetAttributesOptions(
[property: CommandSwitch("--fleet-id")] string FleetId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--new-game-session-protection-policy")]
    public string? NewGameSessionProtectionPolicy { get; set; }

    [CommandSwitch("--resource-creation-limit-policy")]
    public string? ResourceCreationLimitPolicy { get; set; }

    [CommandSwitch("--metric-groups")]
    public string[]? MetricGroups { get; set; }

    [CommandSwitch("--anywhere-configuration")]
    public string? AnywhereConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}