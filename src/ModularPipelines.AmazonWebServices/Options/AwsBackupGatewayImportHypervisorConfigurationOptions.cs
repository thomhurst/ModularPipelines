using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup-gateway", "import-hypervisor-configuration")]
public record AwsBackupGatewayImportHypervisorConfigurationOptions(
[property: CliOption("--host")] string Host,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}