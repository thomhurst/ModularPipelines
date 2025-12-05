using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bcm-data-exports", "get-table")]
public record AwsBcmDataExportsGetTableOptions(
[property: CliOption("--table-name")] string TableName
) : AwsOptions
{
    [CliOption("--table-properties")]
    public IEnumerable<KeyValue>? TableProperties { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}