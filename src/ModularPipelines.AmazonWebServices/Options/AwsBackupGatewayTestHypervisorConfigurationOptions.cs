using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup-gateway", "test-hypervisor-configuration")]
public record AwsBackupGatewayTestHypervisorConfigurationOptions(
[property: CliOption("--gateway-arn")] string GatewayArn,
[property: CliOption("--host")] string Host
) : AwsOptions
{
    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}