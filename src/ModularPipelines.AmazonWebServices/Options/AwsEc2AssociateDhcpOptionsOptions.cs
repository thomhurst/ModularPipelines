using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "associate-dhcp-options")]
public record AwsEc2AssociateDhcpOptionsOptions(
[property: CommandSwitch("--dhcp-options-id")] string DhcpOptionsId,
[property: CommandSwitch("--vpc-id")] string VpcId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}