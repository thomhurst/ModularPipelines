using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediapackage-vod", "create-packaging-configuration")]
public record AwsMediapackageVodCreatePackagingConfigurationOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--packaging-group-id")] string PackagingGroupId
) : AwsOptions
{
    [CliOption("--cmaf-package")]
    public string? CmafPackage { get; set; }

    [CliOption("--dash-package")]
    public string? DashPackage { get; set; }

    [CliOption("--hls-package")]
    public string? HlsPackage { get; set; }

    [CliOption("--mss-package")]
    public string? MssPackage { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}