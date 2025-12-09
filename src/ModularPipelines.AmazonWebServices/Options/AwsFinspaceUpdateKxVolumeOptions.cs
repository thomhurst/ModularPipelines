using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "update-kx-volume")]
public record AwsFinspaceUpdateKxVolumeOptions(
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--volume-name")] string VolumeName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--nas1-configuration")]
    public string? Nas1Configuration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}