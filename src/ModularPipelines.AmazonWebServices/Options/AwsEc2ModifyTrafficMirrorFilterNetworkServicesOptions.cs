using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-traffic-mirror-filter-network-services")]
public record AwsEc2ModifyTrafficMirrorFilterNetworkServicesOptions(
[property: CliOption("--traffic-mirror-filter-id")] string TrafficMirrorFilterId
) : AwsOptions
{
    [CliOption("--add-network-services")]
    public string[]? AddNetworkServices { get; set; }

    [CliOption("--remove-network-services")]
    public string[]? RemoveNetworkServices { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}