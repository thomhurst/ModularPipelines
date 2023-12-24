using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("docdb-elastic", "get-cluster-snapshot")]
public record AwsDocdbElasticGetClusterSnapshotOptions(
[property: CommandSwitch("--snapshot-arn")] string SnapshotArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}