using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "create-flow")]
public record AwsMediaconnectCreateFlowOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--entitlements")]
    public string[]? Entitlements { get; set; }

    [CliOption("--media-streams")]
    public string[]? MediaStreams { get; set; }

    [CliOption("--outputs")]
    public string[]? Outputs { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--source-failover-config")]
    public string? SourceFailoverConfig { get; set; }

    [CliOption("--sources")]
    public string[]? Sources { get; set; }

    [CliOption("--vpc-interfaces")]
    public string[]? VpcInterfaces { get; set; }

    [CliOption("--maintenance")]
    public string? Maintenance { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}