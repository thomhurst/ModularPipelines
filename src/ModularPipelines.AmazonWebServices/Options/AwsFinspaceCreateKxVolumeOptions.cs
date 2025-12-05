using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "create-kx-volume")]
public record AwsFinspaceCreateKxVolumeOptions(
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--volume-type")] string VolumeType,
[property: CliOption("--volume-name")] string VolumeName,
[property: CliOption("--az-mode")] string AzMode,
[property: CliOption("--availability-zone-ids")] string[] AvailabilityZoneIds
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--nas1-configuration")]
    public string? Nas1Configuration { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}