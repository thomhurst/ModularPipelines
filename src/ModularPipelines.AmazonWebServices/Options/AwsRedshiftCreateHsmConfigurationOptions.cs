using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "create-hsm-configuration")]
public record AwsRedshiftCreateHsmConfigurationOptions(
[property: CliOption("--hsm-configuration-identifier")] string HsmConfigurationIdentifier,
[property: CliOption("--description")] string Description,
[property: CliOption("--hsm-ip-address")] string HsmIpAddress,
[property: CliOption("--hsm-partition-name")] string HsmPartitionName,
[property: CliOption("--hsm-partition-password")] string HsmPartitionPassword,
[property: CliOption("--hsm-server-public-certificate")] string HsmServerPublicCertificate
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}