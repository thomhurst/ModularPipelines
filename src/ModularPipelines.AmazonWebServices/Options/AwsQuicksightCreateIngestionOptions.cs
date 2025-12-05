using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "create-ingestion")]
public record AwsQuicksightCreateIngestionOptions(
[property: CliOption("--data-set-id")] string DataSetId,
[property: CliOption("--ingestion-id")] string IngestionId,
[property: CliOption("--aws-account-id")] string AwsAccountId
) : AwsOptions
{
    [CliOption("--ingestion-type")]
    public string? IngestionType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}