using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-vpc-tenancy")]
public record AwsEc2ModifyVpcTenancyOptions(
[property: CommandSwitch("--vpc-id")] string VpcId,
[property: CommandSwitch("--instance-tenancy")] string InstanceTenancy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}