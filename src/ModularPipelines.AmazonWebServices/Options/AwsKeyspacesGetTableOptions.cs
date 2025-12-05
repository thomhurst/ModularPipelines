using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyspaces", "get-table")]
public record AwsKeyspacesGetTableOptions(
[property: CliOption("--keyspace-name")] string KeyspaceName,
[property: CliOption("--table-name")] string TableName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}