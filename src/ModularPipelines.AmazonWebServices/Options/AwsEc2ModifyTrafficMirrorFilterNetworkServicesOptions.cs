using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-traffic-mirror-filter-network-services")]
public record AwsEc2ModifyTrafficMirrorFilterNetworkServicesOptions(
[property: CommandSwitch("--traffic-mirror-filter-id")] string TrafficMirrorFilterId
) : AwsOptions
{
    [CommandSwitch("--add-network-services")]
    public string[]? AddNetworkServices { get; set; }

    [CommandSwitch("--remove-network-services")]
    public string[]? RemoveNetworkServices { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}