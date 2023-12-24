using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "create-hsm-configuration")]
public record AwsRedshiftCreateHsmConfigurationOptions(
[property: CommandSwitch("--hsm-configuration-identifier")] string HsmConfigurationIdentifier,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--hsm-ip-address")] string HsmIpAddress,
[property: CommandSwitch("--hsm-partition-name")] string HsmPartitionName,
[property: CommandSwitch("--hsm-partition-password")] string HsmPartitionPassword,
[property: CommandSwitch("--hsm-server-public-certificate")] string HsmServerPublicCertificate
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}