using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "wait", "nodegroup-active")]
public record AwsEksWaitNodegroupActiveOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--nodegroup-name")] string NodegroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}