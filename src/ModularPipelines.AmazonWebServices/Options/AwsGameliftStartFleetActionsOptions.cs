using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "start-fleet-actions")]
public record AwsGameliftStartFleetActionsOptions(
[property: CommandSwitch("--fleet-id")] string FleetId,
[property: CommandSwitch("--actions")] string[] Actions
) : AwsOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}