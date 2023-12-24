using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("docdb-elastic", "delete-cluster-snapshot")]
public record AwsDocdbElasticDeleteClusterSnapshotOptions(
[property: CommandSwitch("--snapshot-arn")] string SnapshotArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}