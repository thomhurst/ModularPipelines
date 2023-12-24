using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup-gateway", "put-hypervisor-property-mappings")]
public record AwsBackupGatewayPutHypervisorPropertyMappingsOptions(
[property: CommandSwitch("--hypervisor-arn")] string HypervisorArn,
[property: CommandSwitch("--iam-role-arn")] string IamRoleArn,
[property: CommandSwitch("--vmware-to-aws-tag-mappings")] string[] VmwareToAwsTagMappings
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}