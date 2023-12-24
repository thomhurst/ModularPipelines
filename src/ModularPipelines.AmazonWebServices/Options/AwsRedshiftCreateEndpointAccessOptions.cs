using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "create-endpoint-access")]
public record AwsRedshiftCreateEndpointAccessOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--subnet-group-name")] string SubnetGroupName
) : AwsOptions
{
    [CommandSwitch("--cluster-identifier")]
    public string? ClusterIdentifier { get; set; }

    [CommandSwitch("--resource-owner")]
    public string? ResourceOwner { get; set; }

    [CommandSwitch("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}