using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "batch-get-data-quality-result")]
public record AwsGlueBatchGetDataQualityResultOptions(
[property: CliOption("--result-ids")] string[] ResultIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}