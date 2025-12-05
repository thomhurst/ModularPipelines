using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "get-kx-volume")]
public record AwsFinspaceGetKxVolumeOptions(
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--volume-name")] string VolumeName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}