using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-client-vpn-endpoint")]
public record AwsEc2ModifyClientVpnEndpointOptions(
[property: CommandSwitch("--client-vpn-endpoint-id")] string ClientVpnEndpointId
) : AwsOptions
{
    [CommandSwitch("--server-certificate-arn")]
    public string? ServerCertificateArn { get; set; }

    [CommandSwitch("--connection-log-options")]
    public string? ConnectionLogOptions { get; set; }

    [CommandSwitch("--dns-servers")]
    public string? DnsServers { get; set; }

    [CommandSwitch("--vpn-port")]
    public int? VpnPort { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

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