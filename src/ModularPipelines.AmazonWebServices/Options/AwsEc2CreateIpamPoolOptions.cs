using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-ipam-pool")]
public record AwsEc2CreateIpamPoolOptions(
[property: CommandSwitch("--ipam-scope-id")] string IpamScopeId,
[property: CommandSwitch("--address-family")] string AddressFamily
) : AwsOptions
{
    [CommandSwitch("--locale")]
    public string? Locale { get; set; }

    [CommandSwitch("--source-ipam-pool-id")]
    public string? SourceIpamPoolId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--allocation-min-netmask-length")]
    public int? AllocationMinNetmaskLength { get; set; }

    [CommandSwitch("--allocation-max-netmask-length")]
    public int? AllocationMaxNetmaskLength { get; set; }

    [CommandSwitch("--allocation-default-netmask-length")]
    public int? AllocationDefaultNetmaskLength { get; set; }

    [CommandSwitch("--allocation-resource-tags")]
    public string[]? AllocationResourceTags { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--aws-service")]
    public string? AwsService { get; set; }

    [CommandSwitch("--public-ip-source")]
    public string? PublicIpSource { get; set; }

    [CommandSwitch("--source-resource")]
    public string? SourceResource { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}