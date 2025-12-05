using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "delete-view-version")]
public record AwsConnectDeleteViewVersionOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--view-id")] string ViewId,
[property: CliOption("--view-version")] int ViewVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}