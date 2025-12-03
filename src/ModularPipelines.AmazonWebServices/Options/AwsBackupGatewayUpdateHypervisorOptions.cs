using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup-gateway", "update-hypervisor")]
public record AwsBackupGatewayUpdateHypervisorOptions(
[property: CliOption("--hypervisor-arn")] string HypervisorArn
) : AwsOptions
{
    [CliOption("--host")]
    public string? Host { get; set; }

    [CliOption("--log-group-arn")]
    public string? LogGroupArn { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}