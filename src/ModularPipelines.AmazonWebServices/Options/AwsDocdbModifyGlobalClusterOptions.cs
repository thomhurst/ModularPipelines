using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("docdb", "modify-global-cluster")]
public record AwsDocdbModifyGlobalClusterOptions(
[property: CliOption("--global-cluster-identifier")] string GlobalClusterIdentifier
) : AwsOptions
{
    [CliOption("--new-global-cluster-identifier")]
    public string? NewGlobalClusterIdentifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}