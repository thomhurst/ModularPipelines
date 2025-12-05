using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("groundstation", "create-ephemeris")]
public record AwsGroundstationCreateEphemerisOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--satellite-id")] string SatelliteId
) : AwsOptions
{
    [CliOption("--ephemeris")]
    public string? Ephemeris { get; set; }

    [CliOption("--expiration-time")]
    public long? ExpirationTime { get; set; }

    [CliOption("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CliOption("--priority")]
    public int? Priority { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}