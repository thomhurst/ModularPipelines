using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "create-endpoint-access")]
public record AwsRedshiftServerlessCreateEndpointAccessOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--subnet-ids")] string[] SubnetIds,
[property: CommandSwitch("--workgroup-name")] string WorkgroupName
) : AwsOptions
{
    [CommandSwitch("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CommandSwitch("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}