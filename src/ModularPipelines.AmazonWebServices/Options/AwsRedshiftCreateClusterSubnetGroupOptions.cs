using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "create-cluster-subnet-group")]
public record AwsRedshiftCreateClusterSubnetGroupOptions(
[property: CommandSwitch("--cluster-subnet-group-name")] string ClusterSubnetGroupName,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}