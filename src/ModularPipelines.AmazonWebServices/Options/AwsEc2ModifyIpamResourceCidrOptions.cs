using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-ipam-resource-cidr")]
public record AwsEc2ModifyIpamResourceCidrOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--resource-cidr")] string ResourceCidr,
[property: CommandSwitch("--resource-region")] string ResourceRegion,
[property: CommandSwitch("--current-ipam-scope-id")] string CurrentIpamScopeId
) : AwsOptions
{
    [CommandSwitch("--destination-ipam-scope-id")]
    public string? DestinationIpamScopeId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}