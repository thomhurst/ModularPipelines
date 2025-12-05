using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-ipam-pool")]
public record AwsEc2CreateIpamPoolOptions(
[property: CliOption("--ipam-scope-id")] string IpamScopeId,
[property: CliOption("--address-family")] string AddressFamily
) : AwsOptions
{
    [CliOption("--locale")]
    public string? Locale { get; set; }

    [CliOption("--source-ipam-pool-id")]
    public string? SourceIpamPoolId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--allocation-min-netmask-length")]
    public int? AllocationMinNetmaskLength { get; set; }

    [CliOption("--allocation-max-netmask-length")]
    public int? AllocationMaxNetmaskLength { get; set; }

    [CliOption("--allocation-default-netmask-length")]
    public int? AllocationDefaultNetmaskLength { get; set; }

    [CliOption("--allocation-resource-tags")]
    public string[]? AllocationResourceTags { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--aws-service")]
    public string? AwsService { get; set; }

    [CliOption("--public-ip-source")]
    public string? PublicIpSource { get; set; }

    [CliOption("--source-resource")]
    public string? SourceResource { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}