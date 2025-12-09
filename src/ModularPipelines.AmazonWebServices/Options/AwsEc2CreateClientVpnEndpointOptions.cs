using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-client-vpn-endpoint")]
public record AwsEc2CreateClientVpnEndpointOptions(
[property: CliOption("--client-cidr-block")] string ClientCidrBlock,
[property: CliOption("--server-certificate-arn")] string ServerCertificateArn,
[property: CliOption("--authentication-options")] string[] AuthenticationOptions,
[property: CliOption("--connection-log-options")] string ConnectionLogOptions
) : AwsOptions
{
    [CliOption("--dns-servers")]
    public string[]? DnsServers { get; set; }

    [CliOption("--transport-protocol")]
    public string? TransportProtocol { get; set; }

    [CliOption("--vpn-port")]
    public int? VpnPort { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--vpc-id")]
    public string? VpcId { get; set; }

    [CliOption("--self-service-portal")]
    public string? SelfServicePortal { get; set; }

    [CliOption("--client-connect-options")]
    public string? ClientConnectOptions { get; set; }

    [CliOption("--session-timeout-hours")]
    public int? SessionTimeoutHours { get; set; }

    [CliOption("--client-login-banner-options")]
    public string? ClientLoginBannerOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}