using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nimble", "update-launch-profile")]
public record AwsNimbleUpdateLaunchProfileOptions(
[property: CliOption("--launch-profile-id")] string LaunchProfileId,
[property: CliOption("--studio-id")] string StudioId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--launch-profile-protocol-versions")]
    public string[]? LaunchProfileProtocolVersions { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--stream-configuration")]
    public string? StreamConfiguration { get; set; }

    [CliOption("--studio-component-ids")]
    public string[]? StudioComponentIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}