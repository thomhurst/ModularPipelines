using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "get-read-set")]
public record AwsOmicsGetReadSetOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--sequence-store-id")] string SequenceStoreId,
[property: CliOption("--part-number")] int PartNumber
) : AwsOptions
{
    [CliOption("--file")]
    public string? File { get; set; }
}