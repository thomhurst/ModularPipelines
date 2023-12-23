using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "create-trust")]
public record AwsDsCreateTrustOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--remote-domain-name")] string RemoteDomainName,
[property: CommandSwitch("--trust-password")] string TrustPassword,
[property: CommandSwitch("--trust-direction")] string TrustDirection
) : AwsOptions
{
    [CommandSwitch("--trust-type")]
    public string? TrustType { get; set; }

    [CommandSwitch("--conditional-forwarder-ip-addrs")]
    public string[]? ConditionalForwarderIpAddrs { get; set; }

    [CommandSwitch("--selective-auth")]
    public string? SelectiveAuth { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}