using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "create-ip-set")]
public record AwsWafv2CreateIpSetOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--scope")] string Scope,
[property: CliOption("--ip-address-version")] string IpAddressVersion,
[property: CliOption("--addresses")] string[] Addresses
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}