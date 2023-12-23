using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "update-conditional-forwarder")]
public record AwsDsUpdateConditionalForwarderOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--remote-domain-name")] string RemoteDomainName,
[property: CommandSwitch("--dns-ip-addrs")] string[] DnsIpAddrs
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}