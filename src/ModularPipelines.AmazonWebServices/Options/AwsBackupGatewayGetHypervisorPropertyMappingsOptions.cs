using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup-gateway", "get-hypervisor-property-mappings")]
public record AwsBackupGatewayGetHypervisorPropertyMappingsOptions(
[property: CliOption("--hypervisor-arn")] string HypervisorArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}