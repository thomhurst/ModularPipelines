using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dax", "delete-subnet-group")]
public record AwsDaxDeleteSubnetGroupOptions(
[property: CliOption("--subnet-group-name")] string SubnetGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}