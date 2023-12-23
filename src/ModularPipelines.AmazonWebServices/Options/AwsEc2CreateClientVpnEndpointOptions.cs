using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-client-vpn-endpoint")]
public record AwsEc2CreateClientVpnEndpointOptions(
[property: CommandSwitch("--client-cidr-block")] string ClientCidrBlock,
[property: CommandSwitch("--server-certificate-arn")] string ServerCertificateArn,
[property: CommandSwitch("--authentication-options")] string[] AuthenticationOptions,
[property: CommandSwitch("--connection-log-options")] string ConnectionLogOptions
) : AwsOptions
{
    [CommandSwitch("--dns-servers")]
    public string[]? DnsServers { get; set; }

    [CommandSwitch("--transport-protocol")]
    public string? TransportProtocol { get; set; }

    [CommandSwitch("--vpn-port")]
    public int? VpnPort { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--vpc-id")]
    public string? VpcId { get; set; }

    [CommandSwitch("--self-service-portal")]
    public string? SelfServicePortal { get; set; }

    [CommandSwitch("--client-connect-options")]
    public string? ClientConnectOptions { get; set; }

    [CommandSwitch("--session-timeout-hours")]
    public int? SessionTimeoutHours { get; set; }

    [CommandSwitch("--client-login-banner-options")]
    public string? ClientLoginBannerOptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}