using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudhsmv2", "create-hsm")]
public record AwsCloudhsmv2CreateHsmOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId,
[property: CommandSwitch("--availability-zone")] string AvailabilityZone
) : AwsOptions
{
    [CommandSwitch("--ip-address")]
    public string? IpAddress { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}