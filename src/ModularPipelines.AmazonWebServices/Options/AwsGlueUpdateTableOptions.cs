using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "update-table")]
public record AwsGlueUpdateTableOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--table-input")] string TableInput
) : AwsOptions
{
    [CliOption("--catalog-id")]
    public string? CatalogId { get; set; }

    [CliOption("--transaction-id")]
    public string? TransactionId { get; set; }

    [CliOption("--version-id")]
    public string? VersionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}