using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "associate-trunk-interface")]
public record AwsEc2AssociateTrunkInterfaceOptions(
[property: CliOption("--branch-interface-id")] string BranchInterfaceId,
[property: CliOption("--trunk-interface-id")] string TrunkInterfaceId
) : AwsOptions
{
    [CliOption("--vlan-id")]
    public int? VlanId { get; set; }

    [CliOption("--gre-key")]
    public int? GreKey { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}