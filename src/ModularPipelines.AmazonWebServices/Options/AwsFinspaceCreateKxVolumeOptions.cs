using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("finspace", "create-kx-volume")]
public record AwsFinspaceCreateKxVolumeOptions(
[property: CommandSwitch("--environment-id")] string EnvironmentId,
[property: CommandSwitch("--volume-type")] string VolumeType,
[property: CommandSwitch("--volume-name")] string VolumeName,
[property: CommandSwitch("--az-mode")] string AzMode,
[property: CommandSwitch("--availability-zone-ids")] string[] AvailabilityZoneIds
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--nas1-configuration")]
    public string? Nas1Configuration { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}