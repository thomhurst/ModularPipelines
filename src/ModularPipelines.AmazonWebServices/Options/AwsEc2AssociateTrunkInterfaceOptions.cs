using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "associate-trunk-interface")]
public record AwsEc2AssociateTrunkInterfaceOptions(
[property: CommandSwitch("--branch-interface-id")] string BranchInterfaceId,
[property: CommandSwitch("--trunk-interface-id")] string TrunkInterfaceId
) : AwsOptions
{
    [CommandSwitch("--vlan-id")]
    public int? VlanId { get; set; }

    [CommandSwitch("--gre-key")]
    public int? GreKey { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}