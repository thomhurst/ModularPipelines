using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup-gateway", "put-hypervisor-property-mappings")]
public record AwsBackupGatewayPutHypervisorPropertyMappingsOptions(
[property: CliOption("--hypervisor-arn")] string HypervisorArn,
[property: CliOption("--iam-role-arn")] string IamRoleArn,
[property: CliOption("--vmware-to-aws-tag-mappings")] string[] VmwareToAwsTagMappings
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}