using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "create-conditional-forwarder")]
public record AwsDsCreateConditionalForwarderOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--remote-domain-name")] string RemoteDomainName,
[property: CliOption("--dns-ip-addrs")] string[] DnsIpAddrs
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}