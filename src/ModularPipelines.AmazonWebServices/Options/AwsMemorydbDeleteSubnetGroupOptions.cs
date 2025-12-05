using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memorydb", "delete-subnet-group")]
public record AwsMemorydbDeleteSubnetGroupOptions(
[property: CliOption("--subnet-group-name")] string SubnetGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}