using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "update-fleet-port-settings")]
public record AwsGameliftUpdateFleetPortSettingsOptions(
[property: CliOption("--fleet-id")] string FleetId
) : AwsOptions
{
    [CliOption("--inbound-permission-authorizations")]
    public string[]? InboundPermissionAuthorizations { get; set; }

    [CliOption("--inbound-permission-revocations")]
    public string[]? InboundPermissionRevocations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}