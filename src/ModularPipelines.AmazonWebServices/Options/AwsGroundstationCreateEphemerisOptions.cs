using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("groundstation", "create-ephemeris")]
public record AwsGroundstationCreateEphemerisOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--satellite-id")] string SatelliteId
) : AwsOptions
{
    [CommandSwitch("--ephemeris")]
    public string? Ephemeris { get; set; }

    [CommandSwitch("--expiration-time")]
    public long? ExpirationTime { get; set; }

    [CommandSwitch("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CommandSwitch("--priority")]
    public int? Priority { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}