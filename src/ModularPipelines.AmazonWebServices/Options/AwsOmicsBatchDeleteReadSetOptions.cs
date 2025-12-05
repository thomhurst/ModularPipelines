using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "batch-delete-read-set")]
public record AwsOmicsBatchDeleteReadSetOptions(
[property: CliOption("--ids")] string[] Ids,
[property: CliOption("--sequence-store-id")] string SequenceStoreId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}