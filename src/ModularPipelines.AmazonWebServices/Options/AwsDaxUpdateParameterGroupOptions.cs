using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dax", "update-parameter-group")]
public record AwsDaxUpdateParameterGroupOptions(
[property: CliOption("--parameter-group-name")] string ParameterGroupName,
[property: CliOption("--parameter-name-values")] string[] ParameterNameValues
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}