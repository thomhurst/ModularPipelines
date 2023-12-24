using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediapackage-vod", "create-packaging-configuration")]
public record AwsMediapackageVodCreatePackagingConfigurationOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--packaging-group-id")] string PackagingGroupId
) : AwsOptions
{
    [CommandSwitch("--cmaf-package")]
    public string? CmafPackage { get; set; }

    [CommandSwitch("--dash-package")]
    public string? DashPackage { get; set; }

    [CommandSwitch("--hls-package")]
    public string? HlsPackage { get; set; }

    [CommandSwitch("--mss-package")]
    public string? MssPackage { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}