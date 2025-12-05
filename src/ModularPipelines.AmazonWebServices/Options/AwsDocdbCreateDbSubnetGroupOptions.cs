using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("docdb", "create-db-subnet-group")]
public record AwsDocdbCreateDbSubnetGroupOptions(
[property: CliOption("--db-subnet-group-name")] string DbSubnetGroupName,
[property: CliOption("--db-subnet-group-description")] string DbSubnetGroupDescription,
[property: CliOption("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}