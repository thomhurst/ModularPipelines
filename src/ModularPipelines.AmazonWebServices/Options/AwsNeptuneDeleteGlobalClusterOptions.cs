using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune", "delete-global-cluster")]
public record AwsNeptuneDeleteGlobalClusterOptions(
[property: CliOption("--global-cluster-identifier")] string GlobalClusterIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}