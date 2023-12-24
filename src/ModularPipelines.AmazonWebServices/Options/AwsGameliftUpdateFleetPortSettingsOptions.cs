using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "update-fleet-port-settings")]
public record AwsGameliftUpdateFleetPortSettingsOptions(
[property: CommandSwitch("--fleet-id")] string FleetId
) : AwsOptions
{
    [CommandSwitch("--inbound-permission-authorizations")]
    public string[]? InboundPermissionAuthorizations { get; set; }

    [CommandSwitch("--inbound-permission-revocations")]
    public string[]? InboundPermissionRevocations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}