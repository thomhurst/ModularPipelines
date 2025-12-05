using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("docdb", "delete-global-cluster")]
public record AwsDocdbDeleteGlobalClusterOptions(
[property: CliOption("--global-cluster-identifier")] string GlobalClusterIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}