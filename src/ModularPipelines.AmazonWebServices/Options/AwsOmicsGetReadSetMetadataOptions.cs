using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "get-read-set-metadata")]
public record AwsOmicsGetReadSetMetadataOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--sequence-store-id")] string SequenceStoreId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}