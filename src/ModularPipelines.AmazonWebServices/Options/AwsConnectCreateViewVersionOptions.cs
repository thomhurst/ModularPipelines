using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "create-view-version")]
public record AwsConnectCreateViewVersionOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--view-id")] string ViewId
) : AwsOptions
{
    [CliOption("--version-description")]
    public string? VersionDescription { get; set; }

    [CliOption("--view-content-sha256")]
    public string? ViewContentSha256 { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}