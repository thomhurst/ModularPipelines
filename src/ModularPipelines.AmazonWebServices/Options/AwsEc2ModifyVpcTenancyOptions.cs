using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-vpc-tenancy")]
public record AwsEc2ModifyVpcTenancyOptions(
[property: CliOption("--vpc-id")] string VpcId,
[property: CliOption("--instance-tenancy")] string InstanceTenancy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}