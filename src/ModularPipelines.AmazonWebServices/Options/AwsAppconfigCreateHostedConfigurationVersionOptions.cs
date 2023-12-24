using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "create-hosted-configuration-version")]
public record AwsAppconfigCreateHostedConfigurationVersionOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--configuration-profile-id")] string ConfigurationProfileId,
[property: CommandSwitch("--content")] string Content,
[property: CommandSwitch("--content-type")] string ContentType
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--latest-version-number")]
    public int? LatestVersionNumber { get; set; }

    [CommandSwitch("--version-label")]
    public string? VersionLabel { get; set; }
}