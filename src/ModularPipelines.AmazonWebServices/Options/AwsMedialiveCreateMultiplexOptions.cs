using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "create-multiplex")]
public record AwsMedialiveCreateMultiplexOptions(
[property: CliOption("--availability-zones")] string[] AvailabilityZones,
[property: CliOption("--multiplex-settings")] string MultiplexSettings,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--request-id")]
    public string? RequestId { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}