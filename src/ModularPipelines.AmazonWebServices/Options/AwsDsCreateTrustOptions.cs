using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "create-trust")]
public record AwsDsCreateTrustOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--remote-domain-name")] string RemoteDomainName,
[property: CliOption("--trust-password")] string TrustPassword,
[property: CliOption("--trust-direction")] string TrustDirection
) : AwsOptions
{
    [CliOption("--trust-type")]
    public string? TrustType { get; set; }

    [CliOption("--conditional-forwarder-ip-addrs")]
    public string[]? ConditionalForwarderIpAddrs { get; set; }

    [CliOption("--selective-auth")]
    public string? SelectiveAuth { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}