using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "delete-kx-volume")]
public record AwsFinspaceDeleteKxVolumeOptions(
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--volume-name")] string VolumeName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}