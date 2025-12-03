using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-client-vpn-endpoint")]
public record AwsEc2ModifyClientVpnEndpointOptions(
[property: CliOption("--client-vpn-endpoint-id")] string ClientVpnEndpointId
) : AwsOptions
{
    [CliOption("--server-certificate-arn")]
    public string? ServerCertificateArn { get; set; }

    [CliOption("--connection-log-options")]
    public string? ConnectionLogOptions { get; set; }

    [CliOption("--dns-servers")]
    public string? DnsServers { get; set; }

    [CliOption("--vpn-port")]
    public int? VpnPort { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

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