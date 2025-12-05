using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig", "create-hosted-configuration-version")]
public record AwsAppconfigCreateHostedConfigurationVersionOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--configuration-profile-id")] string ConfigurationProfileId,
[property: CliOption("--content")] string Content,
[property: CliOption("--content-type")] string ContentType
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--latest-version-number")]
    public int? LatestVersionNumber { get; set; }

    [CliOption("--version-label")]
    public string? VersionLabel { get; set; }
}