using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "delete-cluster-subnet-group")]
public record AwsRedshiftDeleteClusterSubnetGroupOptions(
[property: CommandSwitch("--cluster-subnet-group-name")] string ClusterSubnetGroupName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}