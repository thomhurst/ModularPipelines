using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "get-reference")]
public record AwsOmicsGetReferenceOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--reference-store-id")] string ReferenceStoreId,
[property: CliOption("--part-number")] int PartNumber
) : AwsOptions
{
    [CliOption("--range")]
    public string? Range { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }
}