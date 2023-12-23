using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "create-multiplex")]
public record AwsMedialiveCreateMultiplexOptions(
[property: CommandSwitch("--availability-zones")] string[] AvailabilityZones,
[property: CommandSwitch("--multiplex-settings")] string MultiplexSettings,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--request-id")]
    public string? RequestId { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}