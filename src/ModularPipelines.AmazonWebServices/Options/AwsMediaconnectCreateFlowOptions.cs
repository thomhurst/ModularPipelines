using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "create-flow")]
public record AwsMediaconnectCreateFlowOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CommandSwitch("--entitlements")]
    public string[]? Entitlements { get; set; }

    [CommandSwitch("--media-streams")]
    public string[]? MediaStreams { get; set; }

    [CommandSwitch("--outputs")]
    public string[]? Outputs { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--source-failover-config")]
    public string? SourceFailoverConfig { get; set; }

    [CommandSwitch("--sources")]
    public string[]? Sources { get; set; }

    [CommandSwitch("--vpc-interfaces")]
    public string[]? VpcInterfaces { get; set; }

    [CommandSwitch("--maintenance")]
    public string? Maintenance { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}