using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediapackage", "create-origin-endpoint")]
public record AwsMediapackageCreateOriginEndpointOptions(
[property: CliOption("--channel-id")] string ChannelId,
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--authorization")]
    public string? Authorization { get; set; }

    [CliOption("--cmaf-package")]
    public string? CmafPackage { get; set; }

    [CliOption("--dash-package")]
    public string? DashPackage { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--hls-package")]
    public string? HlsPackage { get; set; }

    [CliOption("--manifest-name")]
    public string? ManifestName { get; set; }

    [CliOption("--mss-package")]
    public string? MssPackage { get; set; }

    [CliOption("--origination")]
    public string? Origination { get; set; }

    [CliOption("--startover-window-seconds")]
    public int? StartoverWindowSeconds { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--time-delay-seconds")]
    public int? TimeDelaySeconds { get; set; }

    [CliOption("--whitelist")]
    public string[]? Whitelist { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}