using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-cluster")]
public record AwsSagemakerCreateClusterOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--instance-groups")] string[] InstanceGroups
) : AwsOptions
{
    [CliOption("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}