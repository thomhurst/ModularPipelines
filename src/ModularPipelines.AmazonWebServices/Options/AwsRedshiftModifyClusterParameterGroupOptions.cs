using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "modify-cluster-parameter-group")]
public record AwsRedshiftModifyClusterParameterGroupOptions(
[property: CliOption("--parameter-group-name")] string ParameterGroupName,
[property: CliOption("--parameters")] string[] Parameters
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}