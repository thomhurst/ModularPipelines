using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nimble", "update-launch-profile")]
public record AwsNimbleUpdateLaunchProfileOptions(
[property: CommandSwitch("--launch-profile-id")] string LaunchProfileId,
[property: CommandSwitch("--studio-id")] string StudioId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--launch-profile-protocol-versions")]
    public string[]? LaunchProfileProtocolVersions { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--stream-configuration")]
    public string? StreamConfiguration { get; set; }

    [CommandSwitch("--studio-component-ids")]
    public string[]? StudioComponentIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}