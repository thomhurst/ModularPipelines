using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "create-conditional-forwarder")]
public record AwsDsCreateConditionalForwarderOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--remote-domain-name")] string RemoteDomainName,
[property: CommandSwitch("--dns-ip-addrs")] string[] DnsIpAddrs
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}