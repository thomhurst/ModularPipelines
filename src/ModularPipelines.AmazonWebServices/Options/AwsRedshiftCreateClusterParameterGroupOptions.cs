using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "create-cluster-parameter-group")]
public record AwsRedshiftCreateClusterParameterGroupOptions(
[property: CliOption("--parameter-group-name")] string ParameterGroupName,
[property: CliOption("--parameter-group-family")] string ParameterGroupFamily,
[property: CliOption("--description")] string Description
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}