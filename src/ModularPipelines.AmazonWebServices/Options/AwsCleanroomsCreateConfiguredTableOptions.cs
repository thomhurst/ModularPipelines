using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "create-configured-table")]
public record AwsCleanroomsCreateConfiguredTableOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--table-reference")] string TableReference,
[property: CliOption("--allowed-columns")] string[] AllowedColumns,
[property: CliOption("--analysis-method")] string AnalysisMethod
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}