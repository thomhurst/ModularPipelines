using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "delete-nodegroup")]
public record AwsEksDeleteNodegroupOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--nodegroup-name")] string NodegroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}