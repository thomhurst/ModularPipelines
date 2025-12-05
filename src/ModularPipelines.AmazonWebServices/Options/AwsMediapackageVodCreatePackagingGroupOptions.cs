using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediapackage-vod", "create-packaging-group")]
public record AwsMediapackageVodCreatePackagingGroupOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--authorization")]
    public string? Authorization { get; set; }

    [CliOption("--egress-access-logs")]
    public string? EgressAccessLogs { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}