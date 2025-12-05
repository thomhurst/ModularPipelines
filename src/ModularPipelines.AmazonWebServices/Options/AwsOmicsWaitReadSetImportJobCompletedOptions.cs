using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "wait", "read-set-import-job-completed")]
public record AwsOmicsWaitReadSetImportJobCompletedOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--sequence-store-id")] string SequenceStoreId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}