using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-cluster")]
public record AwsSagemakerUpdateClusterOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--instance-groups")] string[] InstanceGroups
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}