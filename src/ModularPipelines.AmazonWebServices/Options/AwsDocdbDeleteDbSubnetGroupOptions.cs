using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("docdb", "delete-db-subnet-group")]
public record AwsDocdbDeleteDbSubnetGroupOptions(
[property: CliOption("--db-subnet-group-name")] string DbSubnetGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}