using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("docdb", "modify-db-subnet-group")]
public record AwsDocdbModifyDbSubnetGroupOptions(
[property: CliOption("--db-subnet-group-name")] string DbSubnetGroupName,
[property: CliOption("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CliOption("--db-subnet-group-description")]
    public string? DbSubnetGroupDescription { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}